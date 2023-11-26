using JIH.GamePlay;
using System;
using UnityEngine;

namespace JIH.Levels
{
    public class SpikeManager : MonoBehaviour
    {
        private Collider2D _collider2D => GetComponent<Collider2D>();

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.TryGetComponent(out IDamageable damagedObject))
            {
                new RequestDamageEvent(damagedObject).Invoke(this);
            }
        }
    }
}
