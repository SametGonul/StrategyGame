using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using UnityEngine;
using Assets.Scripts.StrategyGame.conf;

namespace Assets.Scripts.Models
{


    /// <summary> it is abstract building factory class </summary> 
    public class BuildingFactory : IBuilding
    {

        // implementation of class members
        public string Name { get; private set; }
        public int VerticalSize { get; private set; }
        public int HorizontalSize { get; private set; }

        // factory function,according to name it creates model. 
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
