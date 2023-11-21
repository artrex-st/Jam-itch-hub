using UnityEngine;

namespace Source
{
    [CreateAssetMenu(menuName = "ScreenService/Screen Reference")]
    public sealed class ScreenReference : ScriptableObject
    {
        [SerializeField] private string _sceneName;
        [SerializeField] private string _levelTitle;
        [SerializeField] private Sprite _levelThumbnail;

        public string SceneName => _sceneName;
        public string LevelTitle => _levelTitle;
        public Sprite LevelThumbnail => _levelThumbnail;
    }
}
