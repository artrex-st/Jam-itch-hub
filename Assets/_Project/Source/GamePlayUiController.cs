using Coimbra.Services.Events;
using JIH.ScreenService;
using Source;
using System;
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

    public class GamePlayUiController : BaseScreen
    {
        [SerializeField] private TextMeshProUGUI _frameTitle;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private ScreenReference _settingsScreenRef;

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

            _settingsButton.onClick.AddListener(SettingsButtonClickHandler);

            EventHandles.Add(RequestLevelNameEvent.AddListener(RequestLevelNameEventHandler));
        }

        private void RequestLevelNameEventHandler(ref EventContext context, in RequestLevelNameEvent e)
        {
            _frameTitle.text = e.LevelName;
        }

        private void SettingsButtonClickHandler()
        {
            //TODO: pause the game event
            ScreenService.LoadAdditiveSceneAsync(_settingsScreenRef);
        }
    }
}
