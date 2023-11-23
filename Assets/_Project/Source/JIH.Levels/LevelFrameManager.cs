using Coimbra.Services.Events;
using JIH.DataService;
using Sirenix.OdinInspector;
using Source;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace JIH.Levels
{
    public readonly partial struct RequestLoadingLevelEvent : IEvent
    {
        public readonly ScreenReference SceneReference;

        public RequestLoadingLevelEvent(ScreenReference sceneReference)
        {
            SceneReference = sceneReference;
        }
    }

    public class LevelFrameManager : MonoBehaviour
    {
        //[FoldoutGroup("Text")]
        [SerializeField] private Image _levelThumbnailSprite;
        [SerializeField] private Sprite _levelLockThumbnailSprite;
        [SerializeField] private TextMeshProUGUI _levelName;
        [SerializeField] private ScreenReference _levelSceneReference;
        private ISaveDataService _saveDataService;
        private int _levelId;

        private Button _frameButton => GetComponent<Button>();

        public void Initialize(int levelId, ScreenReference levelFrameStruct, ISaveDataService saveDataService)
        {
            _levelId = levelId;
            _levelSceneReference = levelFrameStruct;
            _saveDataService = saveDataService;
            _levelName.text = _levelSceneReference.LevelTitle;

            //TODO: lock thumbnail level feedback
            _frameButton.interactable = _saveDataService.GameData.UnlockedLevels[_levelId];
            _levelThumbnailSprite.sprite = _frameButton.interactable ? _levelSceneReference.LevelThumbnail : _levelLockThumbnailSprite;

            _saveDataService.GameData.CurrentLevel = _levelId;
            _frameButton.onClick.AddListener(() => new RequestLoadingLevelEvent(_levelSceneReference).Invoke(this));
        }
    }
}
