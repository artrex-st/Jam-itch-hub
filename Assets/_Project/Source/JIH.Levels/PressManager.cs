using Coimbra.Services;
using Coimbra.Services.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JIH.Levels
{
    public readonly partial struct RequestPressTriggerEvent : IEvent
    {
        public readonly bool PressTrigger;

        public RequestPressTriggerEvent(bool pressTrigger)
        {
            PressTrigger = pressTrigger;
        }
    }

    [RequireComponent(typeof(Collider2D))]
    public class PressManager : MonoBehaviour
    {
        private readonly List<EventHandle> _eventHandles = new();

        private void OnEnable()
        {
            //Initialize();
        }


        private void OnDisable()
        {
            Dispose();
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.gameObject.TryGetComponent(out IScalable colliderType))
            {
                new RequestPressTriggerEvent(true).Invoke(this);
            }
        }

        // private void Initialize()
        // {
        //     
        // }

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
