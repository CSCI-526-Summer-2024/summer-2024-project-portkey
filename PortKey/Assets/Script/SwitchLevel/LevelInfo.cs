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
                else
                {
                    _instance.RefreshLevel();
                }
                return _instance;
            }
        }

        public int Level { get; private set; }

        private LevelInfo()
        {
            RefreshLevel();
        }

        public void RefreshLevel()
        {
            string levelName = SceneManager.GetActiveScene().name;
            switch (levelName)
            {
                case "Tutorial":
                    Level = 0;
                    break;
                case "Level1":
                    Level = 1;
                    break;
                case "Level2":
                    Level = 2;
                    break;
                case "Level3":
                    Level = 3;
                    break;
                case "Level4":
                    Level = 4;
                    break;
                case "Level5":
                    Level = 5;
                    break;
                default:
                    Level = -1;
                    break;
            }
        }
    }
}
