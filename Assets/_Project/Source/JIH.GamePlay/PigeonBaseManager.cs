using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JIH.GamePlay
{
    public class PigeonBaseManager : MonoBehaviour, IDamageable
    {
        [SerializeField] protected List<Transform> _patrolPoints;
        [SerializeField] protected float _moveDelay = 2f;
        [SerializeField] protected float _acceleration = 2f;
        protected Vector2 _frameVelocity;

        private Collider2D _collider2D => GetComponent<Collider2D>();
        private Rigidbody2D _rigidbody2D => GetComponent<Rigidbody2D>();

        private bool _gameIsPause = true;
        private int _currentPatrolPoint = 0;

        public void PlayDead()
        {
            new RequestDieAnimationEvent(transform.GetInstanceID()).Invoke(this);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IDamageable damagedObject))
            {
                new RequestDamageEvent(damagedObject).Invoke(this);
            }
        }

        protected async void MoveAsync()
        {
            while (_gameIsPause)
            {
                await UniTask.Delay(TimeSpan.FromSeconds(_moveDelay));
                _frameVelocity = Vector2.MoveTowards(_frameVelocity, _patrolPoints[1].position, _acceleration * Time.fixedDeltaTime);
            }
        }
    }
}
