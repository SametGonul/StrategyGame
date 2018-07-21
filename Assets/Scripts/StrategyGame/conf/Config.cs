using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.StrategyGame.conf
{
    public class Config : MonoBehaviour
    {
        // Infinite Scrollview Constants
    
        public const int ScrollviewMinYValue = -500;
        public const int ScrollviewMaxYValue = 500;

        // Buildings Constants
        // barrack constants
        public const int BarrackHorizontalSize = 3;
        public const int BarrackVerticalSize = 3;

        public const string BarrackName = "Barrack";
        
        // powerplant constants

        public const int PowerPlantHorizontalSize = 3;
        public const int PowerPlantVerticalSize = 2;
        public const string PowerPlantName = "PowerPlant";

    }

}

