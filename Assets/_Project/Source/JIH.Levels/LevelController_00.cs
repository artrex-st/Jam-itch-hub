using Coimbra.Services;
using Coimbra.Services.Events;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace JIH.Levels
{
    public class LevelController_00 : MonoBehaviour
    {

        [SerializeField] private TextMeshProUGUI _feedbackPlaceHolder;
        private readonly List<EventHandle> _eventHandles = new();

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
            _eventHandles.Add(RequestPressTriggerEvent.AddListener(RequestPressTriggerEventHandler));
            _feedbackPlaceHolder.text = $"Press close";
        }

        private void RequestPressTriggerEventHandler(ref EventContext context, in RequestPressTriggerEvent e)
        {
            _feedbackPlaceHolder.text = $"open: {e.PressTrigger}";
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
