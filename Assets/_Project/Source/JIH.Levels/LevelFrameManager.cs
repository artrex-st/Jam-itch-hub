using Coimbra.Services.Events;
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
        [SerializeField] private TextMeshProUGUI _levelName;
        [SerializeField] private ScreenReference _levelSceneReference;

        private Button _frameButton => GetComponent<Button>();

        public void Initialize(ScreenReference levelFrameStruct)
        {
            _levelSceneReference = levelFrameStruct;
            _levelName.text = levelFrameStruct.LevelTitle;
            _levelThumbnailSprite.sprite = levelFrameStruct.LevelThumbnail != null ? levelFrameStruct.LevelThumbnail : _levelThumbnailSprite.sprite;
            _frameButton.onClick.AddListener(() => new RequestLoadingLevelEvent(_levelSceneReference).Invoke(this));
        }
    }
}
