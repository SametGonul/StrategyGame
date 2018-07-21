using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Assets.Scripts.StrategyGame.conf;

namespace Assets.Scripts.Models
{
    public class BarrackModel : IBuilding
    {
        public string Name
        {
            get { return Config.BarrackName; }
        }

        public int VerticalSize
        {
            get { return Config.BarrackVerticalSize; }
        }

        public int HorizontalSize
        {
            get { return Config.BarrackHorizontalSize; }
        }

      
    }

}

