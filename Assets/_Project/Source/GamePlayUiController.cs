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
        [SerializeField] private Button _pauseMenuButton;
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private ScreenReference _pauseMenuScreenRef;
        [SerializeField] private ScreenReference _mainMenuScreenRef;

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

            _pauseMenuButton.onClick.AddListener(PauseMenuButtonClickHandler);
            _mainMenuButton.onClick.AddListener(MainMenuButtonClickHandler);

            EventHandles.Add(RequestLevelNameEvent.AddListener(RequestLevelNameEventHandler));
        }

        private void RequestLevelNameEventHandler(ref EventContext context, in RequestLevelNameEvent e)
        {
            _frameTitle.text = e.LevelName;
        }

        private void PauseMenuButtonClickHandler()
        {
            //TODO: pause the game event
            ScreenService.LoadAdditiveSceneAsync(_pauseMenuScreenRef);
        }
        
        private void MainMenuButtonClickHandler()
        {
            ScreenService.LoadAdditiveSceneAsync(_mainMenuScreenRef);
        }
    }
}
