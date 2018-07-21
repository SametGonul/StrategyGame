using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.StrategyGame.conf;
using JetBrains.Annotations;

namespace Assets.Scripts.Models
{
    public class PowerPlantModel : IBuilding
    {
        public string Name
        {
            get { return Config.PowerPlantName; }
        }

        public int VerticalSize
        {
            get { return Config.PowerPlantVerticalSize; }
        }

        public int HorizontalSize
        {
            get { return Config.PowerPlantHorizontalSize; }
        }
   

    }

}

