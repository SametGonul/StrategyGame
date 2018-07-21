using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Assets.Scripts.StrategyGame.conf;

namespace Assets.Scripts.Models
{
    public class BuildingFactory : IBuilding
    {
        public string Name { get; private set; }
        public int VerticalSize { get; private set; }
        public int HorizontalSize { get; private set; }

        public IBuilding CreateBuilding(string name)
        {
            switch (name)
            {
                case Config.BarrackName:
                    return new BarrackModel();
                case Config.PowerPlantName:
                    return new PowerPlantModel();
                default:
                    return null;
            }
        }

    }

}
