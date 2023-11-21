using Coimbra.Services;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Audio;
using JIH.DataService;
using JIH.ScreenService;
using JIH.SoundService;
using System.Collections.Generic;

namespace Source
{
    public sealed class StartupController : MonoBehaviour
    {
        [Header("Menu screen")]
        [SerializeField] private ScreenReference _firstScreenRef;
        [Header("Level List")]
        [SerializeField] private List<ScreenReference> _levels;

        [FoldoutGroup("Save Data config")]
        [SerializeField] private string _fileName;
        [FoldoutGroup("Save Data config")]
        [SerializeField] private bool _useEncryption;

        [FoldoutGroup("Sound config")]
        [SerializeField] private SoundLibrary _library;
        [FoldoutGroup("Sound config")]
        [SerializeField] private AudioMixer _audioMixer;
        [FoldoutGroup("Sound config")]
        [SerializeField] private AudioMixerGroup _musicMixerGroup;
        [FoldoutGroup("Sound config")]
        [SerializeField] private AudioMixerGroup _sfxMixerGroup;
        [FoldoutGroup("Sound config")]
        [SerializeField] private AudioMixerGroup _uiSfxMixerGroup;

        private IScreenService _screenService;

        private void Awake()
        {
            SpawnPersistingDataService();
            SpawnScreenService();
            SpawnSoundService();
            ServiceLocator.TryGet(out _screenService);
            _screenService.LoadSingleSceneAsync(_firstScreenRef);
        }

        private void SpawnPersistingDataService()
        {
            GameObject saveDataServiceObject = new GameObject(nameof(SaveDataService));
            DontDestroyOnLoad(saveDataServiceObject);
            SaveDataService saveDataService = saveDataServiceObject.AddComponent<SaveDataService>();
#if !UNITY_EDITOR
            _useEncryption = true;
#endif
            saveDataService.Initialize(_fileName, _useEncryption);
        }

        private void SpawnScreenService()
        {
            GameObject screenServiceObject = new GameObject(nameof(ScreenService));
            DontDestroyOnLoad(screenServiceObject);
            ScreenService screenService = screenServiceObject.AddComponent<ScreenService>();
            screenService.Initialize(_levels);
        }

        private void SpawnSoundService()
        {
            GameObject soundServiceObject = new GameObject(nameof(SoundService));
            DontDestroyOnLoad(soundServiceObject);
            SoundService soundService = soundServiceObject.AddComponent<SoundService>();
            soundService.Initialize(_library, _audioMixer, _musicMixerGroup, _sfxMixerGroup, _uiSfxMixerGroup);
        }
    }
}
