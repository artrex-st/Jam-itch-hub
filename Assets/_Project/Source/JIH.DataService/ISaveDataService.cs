using Coimbra.Services;
using System.Collections.Generic;

namespace JIH.DataService
{
    public interface ISaveDataService: IService
    {
        public GameData GameData { get; }
        public void Initialize(string fileName, bool useEncryption, Dictionary<int, bool> levels);
        void SaveData(GameData data);
        public void SaveGame();
    }
}
