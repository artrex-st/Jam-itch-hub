using Coimbra.Services;
using Coimbra.Services.Events;
using JIH.Input;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace JIH.Player
{
    public class PlayerAnimationManager : MonoBehaviour
    {
        private Animator _playerAnimator => GetComponent<Animator>();
        private SpriteRenderer _playerSprite => GetComponent<SpriteRenderer>();
        [SerializeField] private float _playerDirection;
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
            _eventHandles.Add(PerformInputXEvent.AddListener(HandlerPerformInputXEvent));
            _eventHandles.Add(CancelInputXEvent.AddListener(HandlerCancelInputXEvent));
        }

        private void HandlerPerformInputXEvent(ref EventContext context, in PerformInputXEvent e)
        {
            _playerDirection = e.AxisX;

            if (_playerDirection != 0)
            {
                _playerSprite.flipX = _playerDirection < 0;
            }

            _playerAnimator.SetBool(Animator.StringToHash("IsWalking"), _playerDirection != 0);
        }

        private void HandlerCancelInputXEvent(ref EventContext context, in CancelInputXEvent e)
        {
            _playerDirection = e.AxisX;
            _playerAnimator.SetBool(Animator.StringToHash("IsWalking"), _playerDirection != 0);
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
