using JIH.ScreenService;
using Source;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace JIH
{
    public class PauseMenuController : BaseScreen
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private ScreenReference _mainMenuScreenRef;
        [SerializeField] private ScreenReference _settingsScreenRef;

        private void OnEnable()
        {
            Initialize();
        }

        private void OnDisable()
        {
            Dispose();
        }

        private new void Initialize()
        {
            base.Initialize();
            _closeButton.onClick.AddListener(CloseButtonClickHandler);
            _mainMenuButton.onClick.AddListener(MainMenuButtonClickHandler);
            _settingsButton.onClick.AddListener(SettingsButtonClickHandler);

        }

        private void CloseButtonClickHandler()
        {
            AsyncOperation openSceneOperationAsync = ScreenService.UnLoadSceneAsync(_thisScreenRef);
            openSceneOperationAsync.completed += operation => new RequestPauseEvent(false).Invoke(this);
        }

        private void MainMenuButtonClickHandler()
        {
            ScreenService.LoadSingleSceneAsync(_mainMenuScreenRef);
        }

        private void SettingsButtonClickHandler()
        {
            AsyncOperation openSceneOperationAsync = ScreenService.LoadAdditiveSceneAsync(_settingsScreenRef);
        }
    }
}
