using Coimbra.Services.Events;
using UnityEngine;

namespace JIH.GamePlay
{
    public readonly partial struct RequestDieAnimationEvent : IEvent
    {
        public readonly int ParentId;

        public RequestDieAnimationEvent(int parentId)
        {
            ParentId = parentId;
        }
    }

    public interface IDamageable
    {
        public void PlayDead();
    }
}
