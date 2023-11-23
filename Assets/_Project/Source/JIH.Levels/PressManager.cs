using Coimbra.Services;
using Coimbra.Services.Events;
using JIH.Player;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JIH.Levels
{
    public readonly partial struct RequestPressTriggerEvent : IEvent
    {
        public readonly bool PressTrigger;
        public readonly DoorManager DoorActivated;

        public RequestPressTriggerEvent(bool pressTrigger, DoorManager doorActivated)
        {
            PressTrigger = pressTrigger;
            DoorActivated = doorActivated;
        }
    }

    [RequireComponent(typeof(Collider2D))]
    public class PressManager : MonoBehaviour
    {
        [SerializeField] private BodyType _pressType;
        [SerializeField] private DoorManager _doorManager;
        private readonly List<EventHandle> _eventHandles = new();

        private void OnDisable()
        {
            Dispose();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out IScalable colliderType) && colliderType.BodyType.Equals(_pressType))
            {
                new RequestPressTriggerEvent(true, _doorManager).Invoke(this);
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            
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
    }
}
