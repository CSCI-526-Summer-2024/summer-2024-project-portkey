using System;
using UnityEngine.SceneManagement;

namespace PortKey.Assets.Script.SwitchLevel
{
    public class LevelInfo
    {
        private static LevelInfo _instance;
        public static LevelInfo Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new LevelInfo();
                }
                return _instance;
            }
        }

        public int Level { get; private set; }

        private LevelInfo()
        {
            Level = GetLevelNumber();
        }

        private int GetLevelNumber()
        {
            string levelName = SceneManager.GetActiveScene().name;
            switch (levelName)
            {
                case "Level1":
                    return 1;
                case "Level2":
                    return 2;
                case "Level3":
                    return 3;
                case "Level4":
                    return 4;
                default:
                    return -1;
            }
        }
    }
}
