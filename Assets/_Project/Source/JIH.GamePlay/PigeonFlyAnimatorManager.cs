using Coimbra.Services;
using Coimbra.Services.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JIH.GamePlay
{
    public class PigeonFlyAnimatorManager : MonoBehaviour
    {
        private Animator _pigeonFlyAnimator => GetComponent<Animator>();
        private readonly List<EventHandle> _eventHandles = new();
        private static readonly int Die = Animator.StringToHash("Die");

        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            Dispose();
        }

        private void Initialize()
        {
            _eventHandles.Add(RequestDieAnimationEvent.AddListener(HandlerRequestDieAnimationEvent));
        }

        private void Dispose()
        {
            IEventService eventService = ServiceLocator.GetChecked<IEventService>();

            foreach (EventHandle eventHandle in _eventHandles)
            {
                eventService.RemoveListener(eventHandle);
            }

            _eventHandles.Clear();
        }

        private void HandlerRequestDieAnimationEvent(ref EventContext context, in RequestDieAnimationEvent e)
        {
            if (e.ParentId.Equals(transform.parent.GetInstanceID()))
            {
                _pigeonFlyAnimator.SetTrigger(Die);
            }
        }
    }
}
