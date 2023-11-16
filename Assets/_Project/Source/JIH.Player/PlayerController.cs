using Coimbra.Services.Events;
using Cysharp.Threading.Tasks;
using JIH.Input;
using JIH.Levels;
using System.Collections.Generic;
using UnityEngine;

namespace JIH.Player
{
    public struct FrameInput
    {
        public bool JumpDown;
        public bool JumpHeld;
        public Vector2 Move;
        public float Time;
    }

    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour, IScalable
    {
        [SerializeField] private List<ScriptableStats> _stats;
        [SerializeField] private ScriptableStats _currentStats;
        // dash
        [SerializeField] private float _dashAcceleration = 600;
        [SerializeField] private float _dashMaxSpeed = 42;
        [SerializeField] private float _dashDuration = 0.7f;
        [SerializeField] private float _dashCooldown = 2f;
        private bool isDashing = false;
        // end dash
        private Rigidbody2D _rigidbody2D;
        private CapsuleCollider2D _collider2D;
        private FrameInput _frameInput = new FrameInput();
        private Vector2 _frameVelocity;
        private bool _cachedQueryStartInColliders;
        private float _time;
        private float _axisXCache;
        //collisions
        private float _frameLeftGrounded = float.MinValue;
        private bool _grounded;
        //Jumping
        private bool _jumpToConsume;
        private bool _bufferedJumpUsable;
        private bool _doubleJumpToConsume;
        //
        private bool _endedJumpEarly;
        private bool _coyoteUsable;
        private float _timeJumpWasPressed;
        private InputManager _inputManager;
        private readonly List<EventHandle> _eventHandles = new();

        private bool HasBufferedJump => _bufferedJumpUsable && _time < _timeJumpWasPressed + _currentStats.JumpBuffer;
        private bool CanUseCoyote => _coyoteUsable && !_grounded && _time < _frameLeftGrounded + _currentStats.CoyoteTime;

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

            _eventHandles.Add(StartInputXEvent.AddListener(HandlerStartInputXEvent));
            _eventHandles.Add(PerformInputXEvent.AddListener(HandlerPerformInputXEvent));
            _eventHandles.Add(CancelInputXEvent.AddListener(HandlerCancelInputXEvent));

            _eventHandles.Add(StartInputYEvent.AddListener(HandlerStartInputYEvent));
            _eventHandles.Add(PerformInputYEvent.AddListener(HandlerPerformInputYEvent));
            _eventHandles.Add(CancelInputYEvent.AddListener(HandlerCancelInputYEvent));
            
            _eventHandles.Add(RequestInputPressEvent.AddListener(HandlerRequestInputPressEvent));
            _currentStats = _stats[0];
        }

        private void CheckCollisions()
        {
            Physics2D.queriesStartInColliders = false;

            bool groundHit = Physics2D.CapsuleCast(_collider2D.bounds.center, _collider2D.size, _collider2D.direction, 0, Vector2.down, _currentStats.GrounderDistance, ~_currentStats.PlayerLayer);
            bool ceilingHit = Physics2D.CapsuleCast(_collider2D.bounds.center, _collider2D.size, _collider2D.direction, 0, Vector2.up, _currentStats.GrounderDistance, ~_currentStats.PlayerLayer);

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
                _doubleJumpToConsume = true;

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

            if (_grounded || CanUseCoyote || _doubleJumpToConsume)
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
            _frameVelocity.y = _currentStats.JumpPower;

            if (!_grounded && _doubleJumpToConsume)
            {
                _doubleJumpToConsume = false;
            }
            //Jumped?.Invoke();
        }

        private void HandleDirection()
        {
            if (isDashing)
            {
                Debug.Log($"Handle Dash");
                return;
            }

            Debug.Log($"Handle Direction");
            if (_frameInput.Move.x == 0)
            {
                float deceleration = _grounded ? _currentStats.GroundDeceleration : _currentStats.AirDeceleration;
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _frameInput.Move.x * _currentStats.MaxSpeed, _currentStats.Acceleration * Time.fixedDeltaTime);
            }
        }

        private async UniTaskVoid Dash()
        {
            isDashing = true;

            float elapsedTime = 0f;

            while (elapsedTime < _dashDuration)
            {
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _axisXCache * _dashMaxSpeed, _dashAcceleration * Time.fixedDeltaTime);
                elapsedTime += Time.deltaTime;
                await UniTask.Yield();
            }

            isDashing = false;
        }

        private void HandleGravity()
        {
            if (_grounded && _frameVelocity.y <= 0f)
            {
                _frameVelocity.y = _currentStats.GroundingForce;
            }
            else
            {
                float inAirGravity = _currentStats.FallAcceleration;
                if (_endedJumpEarly && _frameVelocity.y > 0)
                {
                    inAirGravity *= _currentStats.JumpEndEarlyGravityModifier;
                }

                _frameVelocity.y = isDashing ? 0 : Mathf.MoveTowards(_frameVelocity.y, -_currentStats.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
            }
        }

        private void ApplyMovement()
        {
            _rigidbody2D.velocity = _frameVelocity;
        }

        private void HandlerStartInputXEvent(ref EventContext context, in StartInputXEvent e)
        {
            _axisXCache = _frameInput.Move.x = e.AxisX;
            GatherInput();
        }

        private void HandlerPerformInputXEvent(ref EventContext context, in PerformInputXEvent e)
        {
            _axisXCache = _frameInput.Move.x = e.AxisX;
            GatherInput();
        }

        private void HandlerCancelInputXEvent(ref EventContext context, in CancelInputXEvent e)
        {
            _frameInput.Move.x = e.AxisX;
        }

        private void HandlerStartInputYEvent(ref EventContext context, in StartInputYEvent e)
        {
            _frameInput.Move.y = e.AxisY;
            _frameInput.JumpDown = e.AxisY > 0;
            _frameInput.JumpHeld = false;
            GatherInput();
        }

        private void HandlerPerformInputYEvent(ref EventContext context, in PerformInputYEvent e)
        {
            _frameInput.Move.y = e.AxisY;
            _frameInput.JumpDown = e.AxisY > 0;
            _frameInput.JumpHeld = e.AxisY > 0;
            GatherInput();
        }

        private void HandlerCancelInputYEvent(ref EventContext context, in CancelInputYEvent e)
        {
            _frameInput.Move.y = e.AxisY;
            _frameInput.JumpDown = e.AxisY > 0;
            _frameInput.JumpHeld = e.AxisY > 0;
        }

        private void HandlerRequestInputPressEvent(ref EventContext context, in RequestInputPressEvent e)
        {
            if (!isDashing)
            {
                Dash().Forget();
            }
        }

        private void GatherInput()
        {
            if (_currentStats.SnapInput)
            {
                _frameInput.Move.x = Mathf.Abs(_frameInput.Move.x) < _currentStats.HorizontalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.x);
                _frameInput.Move.y = Mathf.Abs(_frameInput.Move.y) < _currentStats.VerticalDeadZoneThreshold ? 0 : Mathf.Sign(_frameInput.Move.y);
            }

            if (_frameInput.JumpDown)
            {
                _jumpToConsume = true;
                _timeJumpWasPressed = _time;
            }
        }
    }
}
