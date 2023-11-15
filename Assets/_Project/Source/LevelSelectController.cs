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
    public class LevelSelectController : BaseScreen
    {
        [SerializeField] private List<LevelFrameStruct> _levels;
        [SerializeField] private LevelFrameManager _levelFrame;
        [SerializeField] private Transform _levelFrameParent;
        [SerializeField] private Button _backButton;
        [SerializeField] private ScreenReference _mainMenuScreenRef;

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
            _backButton.onClick.AddListener(BackButtonClickHandler);

            EventHandles.Add(RequestLoadingLevelEvent.AddListener(HandlerRequestLoadingLevelEvent));

            SoundService.PlayMusic(SoundService.SoundLibrary.MainMenuMusic);
            PopulateLevelsFrames();
        }

        private void PopulateLevelsFrames()
        {
            foreach (LevelFrameStruct levelFrameData in _levels)
            {
                LevelFrameManager levelFrameManager = Instantiate(_levelFrame, _levelFrameParent);
                levelFrameManager.Initialize(levelFrameData);
            }
        }

        private void BackButtonClickHandler()
        {
            ScreenService.LoadSingleSceneAsync(_mainMenuScreenRef);
        }

        private void HandlerRequestLoadingLevelEvent(ref EventContext context, in RequestLoadingLevelEvent e)
        {
            ScreenService.LoadSingleSceneAsync(e.SceneReference);
        }
    }
}
