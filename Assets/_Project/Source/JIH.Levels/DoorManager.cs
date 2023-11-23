using Coimbra.Services;
using Coimbra.Services.Events;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace JIH.Levels
{
    [RequireComponent(typeof(Collider2D))]
    public class DoorManager : MonoBehaviour
    {
        [SerializeField] private float _dorAnimationTime = 2;
        private SpriteRenderer _sprite => GetComponent<SpriteRenderer>();
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
        }

        private void RequestPressTriggerEventHandler(ref EventContext context, in RequestPressTriggerEvent e)
        {
            if (e.DoorActivated == this)
            {
                transform.DOMove(new Vector3(transform.position.x, transform.position.y - transform.localScale.y), _dorAnimationTime).OnComplete(() => _sprite.DOFade(0, _dorAnimationTime));
            }
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
