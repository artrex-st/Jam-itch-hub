using Coimbra.Services.Events;
using JIH.Input;
using System.Collections.Generic;
using UnityEngine;

namespace JIH.Player
{
    public struct FrameInput
    {
        public bool JumpDown;
        public bool JumpHeld;
        public Vector2 Move;
    }

    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private ScriptableStats _stats;
        private Rigidbody2D _rigidbody2D;
        private CapsuleCollider2D _collider2D;
        private FrameInput _frameInput = new FrameInput();
        private Vector2 _frameVelocity;
        private bool _cachedQueryStartInColliders;
        // public Vector2 FrameInput => _frameInput.Move;
        // public event Action<bool, float> GroundedChanged;
        // public event Action Jumped;
        private float _time;
        //collisions
        private float _frameLeftGrounded = float.MinValue;
        private bool _grounded;
        //Jumping
        private bool _jumpToConsume;
        private bool _bufferedJumpUsable;
        private bool _endedJumpEarly;
        private bool _coyoteUsable;
        private float _timeJumpWasPressed;
        private InputManager _inputManager;
        private readonly List<EventHandle> _eventHandles = new();

        private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + _stats.JumpBuffer;
        private bool CanUseCoyote => _coyoteUsable && !_grounded && _time < _frameLeftGrounded + _stats.CoyoteTime;

        private void Awake()
        {
            Initialize();
        }

        private void Update()
        {
            _time += Time.deltaTime;
        }

        private void FixedUpdate()
        {
            CheckCollisions();
            HandleJump();
            HandleDirection();
            HandleGravity();
            ApplyMovement();
        }

        private void Initialize()
        {
            _rigidbody2D = GetComponent<Rigidbody2D>();
            _collider2D = GetComponent<CapsuleCollider2D>();
            _inputManager = gameObject.AddComponent<InputManager>();
            _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;

            _eventHandles.Add(StartInputMoveEvent.AddListener(HandlerStartInputMoveEvent));
            _eventHandles.Add(PerformInputMoveEvent.AddListener(HandlerPerformInputMoveEvent));
            _eventHandles.Add(CancelInputMoveEvent.AddListener(HandlerCancelInputMoveEvent));
        }

        private void CheckCollisions()
        {
            Physics2D.queriesStartInColliders = false;

            bool groundHit = Physics2D.CapsuleCast(_collider2D.bounds.center, _collider2D.size, _collider2D.direction, 0, Vector2.down, _stats.GrounderDistance, ~_stats.PlayerLayer);
            bool ceilingHit = Physics2D.CapsuleCast(_collider2D.bounds.center, _collider2D.size, _collider2D.direction, 0, Vector2.up, _stats.GrounderDistance, ~_stats.PlayerLayer);

            if (ceilingHit)
            {
                _frameVelocity.y = Mathf.Min(0, _frameVelocity.y);
            }

            if (!_grounded && groundHit)
            {
                _grounded = true;
                _coyoteUsable = true;
                _bufferedJumpUsable = true;
                _endedJumpEarly = false;
                //TODO: EVENT
            }
            else if (_grounded && !groundHit)
            {
                _grounded = false;
                _frameLeftGrounded = _time;
                //TODO: EVENT
            }

            Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
        }

        private void HandleJump()
        {
            if (!_endedJumpEarly && !_grounded && !_frameInput.JumpHeld && _rigidbody2D.velocity.y > 0)
            {
                _endedJumpEarly = true;
            }

            if (!_jumpToConsume && !HasBufferedJump)
            {
                return;
            }

            if (_grounded || CanUseCoyote)
            {
                ExecuteJump();
            }

            _jumpToConsume = false;
        }

        private void ExecuteJump()
        {
            _endedJumpEarly = false;
            _timeJumpWasPressed = 0;
            _bufferedJumpUsable = false;
            _coyoteUsable = false;
            _frameVelocity.y = _stats.JumpPower;
            //Jumped?.Invoke();
        }

        private void HandleDirection()
        {
            if (_frameInput.Move.x == 0)
            {
                float deceleration = _grounded ? _stats.GroundDeceleration : _stats.AirDeceleration;
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _stats.MaxSpeed, _stats.Acceleration * Time.fixedDeltaTime);
            }
        }

        private void HandleGravity()
        {
            if (_grounded && _frameVelocity.y <= 0f)
            {
                _frameVelocity.y = _stats.GroundingForce;
            }
            else
            {
                float inAirGravity = _stats.FallAcceleration;
                if (_endedJumpEarly && _frameVelocity.y > 0)
                {
                    inAirGravity *= _stats.JumpEndEarlyGravityModifier;
                }

                _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_stats.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
            }
        }

        private void ApplyMovement()
        {
            _rigidbody2D.velocity = _frameVelocity;
        }

        private void HandlerStartInputMoveEvent(ref EventContext context, in StartInputMoveEvent e)
        {
            _frameInput = new FrameInput()
            {
                JumpDown = e.MoveAxisMovement.y > 0,
                JumpHeld = false,
                Move = e.MoveAxisMovement
            };

            GatherInput();
        }

        private void HandlerPerformInputMoveEvent(ref EventContext context, in PerformInputMoveEvent e)
        {
            _frameInput = new FrameInput()
            {
                JumpDown = e.MoveAxisMovement.y > 0,
                JumpHeld = e.MoveAxisMovement.y > 0,
                Move = e.MoveAxisMovement
            };

            GatherInput();
        }

        private void HandlerCancelInputMoveEvent(ref EventContext context, in CancelInputMoveEvent e)
        {
            _frameInput = new FrameInput()
            {
                JumpDown = false,
                JumpHeld = false,
                Move = e.MoveAxisMovement
            };

        }

        private void GatherInput()
        {
            if (_stats.SnapInput)
            {
                _frameInput.Move.x = Mathf.Abs(_frameInput.Move.x) < _stats.HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.x);
                _frameInput.Move.y = Mathf.Abs(_frameInput.Move.y) < _stats.VerticalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.y);
            }

            if (_frameInput.JumpDown)
            {
                _jumpToConsume = true;
                _timeJumpWasPressed = _time;
            }
        }
    }
}
