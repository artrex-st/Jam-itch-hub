using System.Collections.Generic;

namespace JIH.DataService
{
    [System.Serializable]
    public class GameData
    {
        public int CurrentLevel;
        public Dictionary<int, bool> UnlockedLevels;
        public float MasterVolume;
        public float MusicVolume;
        public float SfxVolume;
        public float UiSfxVolume;

        public GameData()
        {
            CurrentLevel = 0;
            UnlockedLevels = new Dictionary<int, bool>();
            MasterVolume = 1f;
            MusicVolume = 1f;
            SfxVolume = 1f;
            UiSfxVolume = 1f;
        }
    }
}
