using Coimbra.Services.Events;
using JIH.Levels;
using JIH.ScreenService;
using Source;
using UnityEngine;
using UnityEngine.UI;

namespace JIH
{
    public class LevelSelectController : BaseScreen
    {
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
            foreach (ScreenReference levelFrameData in ScreenService.Levels)
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
            ScreenService.LoadSingleScene(e.SceneReference);
        }
    }
}
