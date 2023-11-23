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
            _levelThumbnailSprite.sprite = _levelSceneReference.LevelThumbnail != null ? _levelSceneReference.LevelThumbnail : _levelThumbnailSprite.sprite;

            //TODO: lock thumbnail level feedback
            _frameButton.interactable = _saveDataService.GameData.UnlockedLevels[_levelId];
            Debug.Log($"Level: {_levelId} está liberado?: {_frameButton.interactable}");
            _frameButton.onClick.AddListener(() => new RequestLoadingLevelEvent(_levelSceneReference).Invoke(this));
        }
    }
}
