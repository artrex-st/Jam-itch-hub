using Cinemachine;
using Coimbra.Services.Events;
using Cysharp.Threading.Tasks;
using JIH.GamePlay;
using JIH.Levels;
using JIH.Player;
using JIH.ScreenService;
using Sirenix.OdinInspector;
using Source;
using System;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;

namespace JIH
{
    public enum DamageType
    {
        BaseType,
    }

    public readonly partial struct RequestLevelNameEvent : IEvent
    {
        public readonly string LevelName;

        public RequestLevelNameEvent(string levelName)
        {
            LevelName = levelName;
        }
    }

    public readonly partial struct RequestEndLevelEvent : IEvent { }

    public readonly partial struct RequestPauseEvent : IEvent
    {
        public readonly bool IsGamePause;

        public RequestPauseEvent(bool isGamePause)
        {
            IsGamePause = isGamePause;
        }
    }

    public readonly partial struct RequestDamageEvent : IEvent
    {
        public readonly DamageType DamageType;
        public readonly IDamageable DamageableScript;

        public RequestDamageEvent(IDamageable damageableScript, DamageType damageType = JIH.DamageType.BaseType)
        {
            DamageableScript = damageableScript;
            DamageType = damageType;
        }
    }

    public class LevelController : BaseScreen
    {
        [FoldoutGroup("Spawn Config")]
        [SerializeField] private PlayerController _player;
        [FoldoutGroup("Spawn Config")]
        [SerializeField] private Transform _playerSpawnPoint;
        [FoldoutGroup("Spawn Config")]
        [SerializeField] private CinemachineVirtualCamera _vitualCamera;
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
            EventHandles.Add(RequestDamageEvent.AddListener(HandlerRequestDamageEvent));
            StartGameAsync();
        }

        private async void StartGameAsync()
        {
            await UniTask.Delay(TimeSpan.FromSeconds(1));
            PlayerController player = Instantiate(_player, _playerSpawnPoint.position, quaternion.identity);
            _vitualCamera.Follow = player.transform;
            _vitualCamera.LookAt = player.transform;
            new RequestPauseEvent(false).Invoke(this);
        }

        private void SettingsButtonClickHandler()
        {
            new RequestPauseEvent(true).Invoke(this);
            ScreenService.LoadAdditiveSceneAsync(_pauseMenuScreenRef);
        }

        private void RequestLevelNameEventHandler(ref EventContext context, in RequestLevelNameEvent e)
        {
            _frameTitle.text = e.LevelName;
        }

        private void HandlerRequestEndLevelEvent(ref EventContext context, in RequestEndLevelEvent e)
        {
            //new RequestPauseEvent(true).Invoke(this);

            if (SaveDataService.GameData.CurrentLevel >= SaveDataService.GameData.UnlockedLevels.Count)
            {
                //TODO: End Game
                Debug.LogWarning($"<color=purple> End Levels </color>");
            }

            SaveDataService.GameData.CurrentLevel++;
            int x = SaveDataService.GameData.CurrentLevel++;
            SaveDataService.GameData.UnlockedLevels[x] = true;

            foreach (KeyValuePair<int,bool> level in SaveDataService.GameData.UnlockedLevels)
            {
                Debug.Log($"Level: {level.Key} está liberado?: {level.Value}");
            }

            SaveDataService.SaveGame();
            ScreenService.LoadSingleScene(_nextLevelScreenRef);
        }

        private void HandlerRequestDamageEvent(ref EventContext context, in RequestDamageEvent e)
        {
            e.DamageableScript.PlayDead();
        }
    }
}
