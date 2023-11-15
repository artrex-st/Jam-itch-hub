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

    [System.Serializable]
    public struct LevelFrameStruct
    {
        public Sprite levelSprite;
        public string levelName;
        public ScreenReference screenReference;
    }

    public class LevelFrameManager : MonoBehaviour
    {
        //[FoldoutGroup("Text")]
        [SerializeField] private Image _levelSprite;
        [SerializeField] private TextMeshProUGUI _levelName;
        [SerializeField] private ScreenReference _levelSceneReference;

        private Button _frameButton => GetComponent<Button>();

        public void Initialize(LevelFrameStruct levelFrameStruct)
        {
            _levelSprite.sprite = levelFrameStruct.levelSprite;
            _levelName.text = levelFrameStruct.levelName;
            _levelSceneReference = levelFrameStruct.screenReference;
            _frameButton.onClick.AddListener(() => new RequestLoadingLevelEvent(_levelSceneReference).Invoke(this));
        }
    }
}
