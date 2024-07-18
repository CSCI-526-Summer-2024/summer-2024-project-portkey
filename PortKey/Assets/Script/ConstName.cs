using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PortKey.Assets.Script
{
    public class ConstName
    {
        public const string LEFT_CAR = "CarLeft";
        public const string RIGHT_CAR = "CarRight";
        public const string ZOOM_LEFT = "ZoomLeft";
        public const string ZOOM_RIGHT = "ZoomRight";
        public const bool SEND_ANALYTICS = false;
    }
    public static class Level8Info
    {
        public static bool scoreUp = false;
        public static bool cntrFlip = false;
        public static bool lives = false;
        public static bool antiHealth = false;
        public static bool turtle = false;
        public static bool shooting = false;
    }
}