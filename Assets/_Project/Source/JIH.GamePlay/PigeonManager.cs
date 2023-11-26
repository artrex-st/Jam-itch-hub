using UnityEngine;

namespace JIH.GamePlay
{
    public class PigeonManager : MonoBehaviour, IDamageable
    {
        public void PlayDead()
        {
            Debug.Log($"Apply Damage Logic on {transform.name}");
            new RequestDieAnimationEvent(transform.GetInstanceID()).Invoke(this);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            
        }
    }
}
