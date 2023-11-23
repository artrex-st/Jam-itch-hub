using Coimbra.Services.Events;
using JIH.Levels;
using JIH.ScreenService;
using Source;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace JIH
{
    public readonly partial struct RequestLevelNameEvent : IEvent
    {
        public readonly string LevelName;

        public RequestLevelNameEvent(string levelName)
        {
            LevelName = levelName;
        }
    }

    public readonly partial struct RequestEndLevelEvent : IEvent { }

    public class LevelController : BaseScreen
    {
        [SerializeField] private TextMeshProUGUI _frameTitle;
        [SerializeField] private Button _pauseMenuButton;
        [SerializeField] private ScreenReference _nextLevelScreenRef;
        [SerializeField] private ScreenReference _pauseMenuScreenRef;

        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            base.Dispose();
        }

        private new void Initialize()
        {
            base.Initialize();

            _pauseMenuButton.onClick.AddListener(SettingsButtonClickHandler);

            EventHandles.Add(RequestLevelNameEvent.AddListener(RequestLevelNameEventHandler));
            EventHandles.Add(RequestEndLevelEvent.AddListener(HandlerRequestEndLevelEvent));
        }

        private void SettingsButtonClickHandler()
        {
            //TODO: pause the game event
            ScreenService.LoadAdditiveSceneAsync(_pauseMenuScreenRef);
        }

        private void RequestLevelNameEventHandler(ref EventContext context, in RequestLevelNameEvent e)
        {
            _frameTitle.text = e.LevelName;
        }

        private void HandlerRequestEndLevelEvent(ref EventContext context, in RequestEndLevelEvent e)
        {
            //TODO: pause the game event

            if (SaveDataService.GameData.CurrentLevel >= SaveDataService.GameData.UnlockedLevels.Count)
            {
                //TODO: End Game
                Debug.LogWarning($"<color=purple> End Levels </color>");
            }

            int x = SaveDataService.GameData.CurrentLevel++;
            SaveDataService.GameData.UnlockedLevels[x] = true;

            foreach (KeyValuePair<int,bool> level in SaveDataService.GameData.UnlockedLevels)
            {
                Debug.Log($"Level: {level.Key} está liberado?: {level.Value}");
            }

            ScreenService.LoadSingleScene(_nextLevelScreenRef);
        }
    }
}
