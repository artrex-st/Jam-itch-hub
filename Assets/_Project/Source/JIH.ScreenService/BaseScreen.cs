using Coimbra.Services;
using Coimbra.Services.Events;
using JIH.DataService;
using JIH.SoundService;
using Source;
using System.Collections.Generic;
using UnityEngine;

namespace JIH.ScreenService
{
    public class BaseScreen : MonoBehaviour
    {
        [Header("Config")]
        [SerializeField] protected ScreenReference _thisScreenRef;
        protected ISaveDataService SaveDataService;
        protected IScreenService ScreenService;
        protected ISoundService SoundService;
        protected readonly List<EventHandle> EventHandles = new();

        protected void Initialize()
        {
            ServiceLocator.TryGet(out SaveDataService);
            ServiceLocator.TryGet(out ScreenService);
            ServiceLocator.TryGet(out SoundService);
#if UNITY_EDITOR
            Debug.Log($"Initialize <color=white>{_thisScreenRef.SceneName}</color>");
#endif
        }

        protected void Dispose()
        {
            IEventService eventService = ServiceLocator.GetChecked<IEventService>();

            foreach (EventHandle eventHandle in EventHandles)
            {
                eventService.RemoveListener(eventHandle);
            }

            EventHandles.Clear();
#if UNITY_EDITOR
            Debug.Log($"Dispose <color=white>{_thisScreenRef.SceneName}</color>");
#endif
        }
    }
}