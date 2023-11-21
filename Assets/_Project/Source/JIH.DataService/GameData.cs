namespace JIH.DataService
{
    [System.Serializable]
    public class GameData
    {
        public int CurrentLevel;
        public int UnlockedLevels;
        public float MasterVolume;
        public float MusicVolume;
        public float SfxVolume;
        public float UiSfxVolume;

        public GameData()
        {
            CurrentLevel = 0;
            UnlockedLevels = 0;
            MasterVolume = 1f;
            MusicVolume = 1f;
            SfxVolume = 1f;
            UiSfxVolume = 1f;
        }
    }
}
