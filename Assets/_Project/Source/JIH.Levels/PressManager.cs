using Coimbra.Services;
using Coimbra.Services.Events;
using JIH.Player;
using System;
using System.Collections.Generic;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Serialization;

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

    public enum PressPlateSkinType
    {
        Common,
        Heavy,
        Light
    }

    [RequireComponent(typeof(Collider2D))]
    public class PressManager : MonoBehaviour
    {
        [SerializeField] private BodyType _pressType;
        [SerializeField] private PressPlateSkinType _pressPlateSkinType = PressPlateSkinType.Common;
        [SerializeField] private DoorManager _doorManager;
        [SerializeField] private Animator _animator;
        [SerializeField] private List<AnimatorController> _pressAnimations;
        private Collider2D _collider2D => GetComponent<Collider2D>();

        private readonly int _pressTriggerHash = Animator.StringToHash("Press");
        private readonly List<EventHandle> _eventHandles = new();

        private void OnEnable()
        {
            Initialize();
        }

        private void Initialize()
        {
            switch (_pressPlateSkinType)
            {
                case PressPlateSkinType.Common:
                    _animator.runtimeAnimatorController = _pressAnimations[0];
                    break;

                case PressPlateSkinType.Heavy:
                    _animator.runtimeAnimatorController = _pressAnimations[1];
                    break;

                case PressPlateSkinType.Light:
                    _animator.runtimeAnimatorController = _pressAnimations[2];
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        private void OnDisable()
        {
            Dispose();
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.TryGetComponent(out IScalable colliderType) && colliderType.BodyType.Equals(_pressType))
            {
                new RequestPressTriggerEvent(true, _doorManager).Invoke(this);

                if (_animator != null)
                {
                    _animator.SetTrigger(_pressTriggerHash);
                    _collider2D.enabled = false;
                }
                else
                {
                    Debug.LogWarning($"{transform.name} there is no animation!");
                }
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            
        }

        private void SetLayerWeight(string layerName, float weight)
        {
            int layerIndex = _animator.GetLayerIndex(layerName);
            _animator.SetLayerWeight(layerIndex, weight);
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
