using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.UI;
using JIH.ScreenService;

namespace Source
{
    public class SettingsMenuController : BaseScreen
    {
        [SerializeField] private Button _closeButton;
        [SerializeField] private Button _mainMenuButton;
        [SerializeField] private ScreenReference _mainMenuScreenRef;
        [FoldoutGroup("Settings UiOverlay elements")]
        [SerializeField] private Slider _sliderMaster;
        [FoldoutGroup("Settings UiOverlay elements")]
        [SerializeField] private Slider _sliderMusic;
        [FoldoutGroup("Settings UiOverlay elements")]
        [SerializeField] private Slider _sliderSfx;
        [FoldoutGroup("Settings UiOverlay elements")]
        [SerializeField] private Slider _sliderUiSfx;

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

            _sliderMaster.onValueChanged.AddListener(OnMasterVolumeChangeHandler);
            _sliderMusic.onValueChanged.AddListener(OnMusicVolumeChangeHandler);
            _sliderSfx.onValueChanged.AddListener(OnSfxVolumeChangeHandler);
            _sliderUiSfx.onValueChanged.AddListener(OnUiSfxVolumeChangeHandler);

            SyncSlidersFromMixers();
        }

        private void MainMenuButtonClickHandler()
        {
            ScreenService.LoadSingleSceneAsync(_mainMenuScreenRef);
        }

        private void OnMasterVolumeChangeHandler(float value)
        {
            SoundService.MasterVolume = value;
        }

        private void OnMusicVolumeChangeHandler(float value)
        {
            SoundService.MusicVolume = value;
        }

        private void OnSfxVolumeChangeHandler(float value)
        {
            SoundService.SfxVolume = value;
        }

        private void OnUiSfxVolumeChangeHandler(float value)
        {
            SoundService.UiSfxVolume = value;
        }

        private void CloseButtonClickHandler()
        {
            SaveSoundVolume();
            AsyncOperation openSceneOperationAsync = ScreenService.UnLoadSceneAsync(_thisScreenRef);
        }

        private void SyncSlidersFromMixers()
        {
            _sliderMaster.value = SaveDataService.GameData.MasterVolume;
            _sliderMusic.value = SaveDataService.GameData.MusicVolume;
            _sliderSfx.value = SaveDataService.GameData.SfxVolume;
            _sliderUiSfx.value = SaveDataService.GameData.UiSfxVolume;
        }

        private void SaveSoundVolume()
        {
            SaveDataService.GameData.MasterVolume = SoundService.MasterVolume;
            SaveDataService.GameData.MusicVolume = SoundService.MusicVolume;
            SaveDataService.GameData.SfxVolume = SoundService.SfxVolume;
            SaveDataService.GameData.UiSfxVolume = SoundService.UiSfxVolume;
            SaveDataService.SaveGame();
        }
    }
}
