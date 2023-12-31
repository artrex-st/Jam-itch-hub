using Coimbra;
using System.Collections.Generic;
using UnityEngine;

namespace JIH.DataService
{
    public sealed class SaveDataService : Actor, ISaveDataService
    {
        public GameData GameData { get; private set; }
        private bool _useEncryption;
        private FileDataHandler _fileDataHandler;
        private Dictionary<int, bool> _levels;

        public void Initialize(string fileName, bool useEncryption, Dictionary<int, bool> levels)
        {
            _levels = levels;
            _useEncryption = useEncryption;
#if UNITY_EDITOR
            _fileDataHandler = new FileDataHandler(Application.dataPath + "/Saves/", fileName, _useEncryption);
#else
            _fileDataHandler = new FileDataHandler(Application.persistentDataPath, fileName, _useEncryption);
#endif
            LoadGame();
        }

        public void SaveData(GameData data)
        {
            GameData = data;
        }

        public void SaveGame()
        {
            _fileDataHandler.Save(GameData);
        }

        private void NewGame()
        {
            GameData = new GameData
            {
                UnlockedLevels = _levels,
            };
        }

        private void LoadGame()
        {
            GameData = _fileDataHandler.Load();

            if(GameData == null)
            {
                Debug.Log($"<color=green>New Game</color>, set all to default");
                NewGame();
            }
        }

        private new void OnApplicationQuit()
        {
            base.OnApplicationQuit();
            SaveGame();
        }
    }
}

