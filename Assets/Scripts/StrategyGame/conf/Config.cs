using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Assets.Scripts.StrategyGame.conf
{

    /// <summary>
    /// This class for constant values in the game.
    /// </summary>
    public class Config : MonoBehaviour
    {
        // Infinite Scrollview Constants
        public const int ScrollviewMinYValue = -500;
        public const int ScrollviewMaxYValue = 500;
        public const int ScrollBuildingSpeed = 500;
        public const int ScrollviewHeight = 1000;

        // barrack constants
        public const int BarrackHorizontalSize = 3;
        public const int BarrackVerticalSize = 3;
        public const string BarrackName = "Barrack";
        
        // powerplant constants
        public const int PowerPlantHorizontalSize = 3;
        public const int PowerPlantVerticalSize = 2;
        public const string PowerPlantName = "PowerPlant";

        // map constants
        public const int GridSize = 32;
        public const int VerticalGridNumber = 28;
        public const int HorizontalGridNumber = 24;

        //grid constants
        public const float FirstGridStartingXCoordinate = -368.5f;
        public const float FirstGridStartingYCoordinate = 432f;
        public const float TheMostXCoordinate = -384.5f;
        public const float TheMostYCoordinate = 448f;
    }
}

