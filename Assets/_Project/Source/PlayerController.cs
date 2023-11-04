using JIH.Input;
using Coimbra.Services.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Source
{
    [RequireComponent(typeof(Rigidbody2D), typeof(Collider2D))]
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private ScriptableMoveStats _stats;
        private InputManager _inputManager;
        private readonly List<EventHandle> _eventHandles = new();
        private Rigidbody2D _rigidbody2D => GetComponent<Rigidbody2D>();
        private CapsuleCollider2D _collider2D => GetComponent<CapsuleCollider2D>();
        private bool _grounded;
        private float _time;
        private bool _cachedQueryStartInColliders;
        private Vector2 _frameVelocity;
        private Vector2 _playerDirection;

        public void Start()
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
            HandleDirection();
            HandleGravity();
            HandleJump();
            ApplyMovement();
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
                //TODO: Event to Jump (animations, Vfx, calculations, etc)
            }
            else if (_grounded && !groundHit)
            {
                _grounded = false;
                //TODO: Event to finish Jump (animations, Vfx, calculations, etc)
            }

            Physics2D.queriesStartInColliders = _cachedQueryStartInColliders;
        }

        private void HandleDirection()
        {
            if (_playerDirection.x == 0)
            {
                float deceleration = _grounded ? _stats.GroundResistence : _stats.AirResistence;
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, 0, deceleration * Time.fixedDeltaTime);
            }
            else
            {
                _frameVelocity.x = Mathf.MoveTowards(_frameVelocity.x, _playerDirection.x * _stats.MaxSpeed, _stats.Acceleration * Time.fixedDeltaTime);
            }
        }

        private void HandleGravity()
        {
            if (_grounded && _frameVelocity.y <= 0f)
            {
                _frameVelocity.y = _stats.ArtificialGravity;
            }
            else
            {
                float inAirGravity = _stats.AirArtificialGravity;
                _frameVelocity.y = Mathf.MoveTowards(_frameVelocity.y, -_stats.MaxFallSpeed, inAirGravity * Time.fixedDeltaTime);
            }
        }

        private void HandleJump()
        {
            if (_grounded && _playerDirection.y > 0)
            {
                ExecuteJump();
            }
        }

        private void ExecuteJump()
        {
            _frameVelocity.y = _stats.JumpPower;
            //TODO: Event to Jump (animations, Vfx, calculations, etc)
        }

        private void ApplyMovement()
        {
            _rigidbody2D.velocity = _frameVelocity;
        }

        private void Initialize()
        {
            _cachedQueryStartInColliders = Physics2D.queriesStartInColliders;
            _inputManager = gameObject.AddComponent<InputManager>();
            _eventHandles.Add(RequestInputMoveEvent.AddListener(HandlerRequestInputMoveEvent));
        }

        private void HandlerRequestInputMoveEvent(ref EventContext context, in RequestInputMoveEvent e)
        {
            _playerDirection = new Vector2(e.MoveAxisMovement.x, e.MoveAxisMovement.y);
        }
    }
}
