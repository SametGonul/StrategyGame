using Assets.Scripts.StrategyGame.conf;
using UnityEngine;

namespace Assets.Scripts.Model
{
    /// <summary>
    /// This powerplant model inherited from building interfaces
    /// These get methods return their contstant values
    /// </summary>
    public class ScrollPowerPlantModel : IScrollBuildingModel
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

        public int XIndex { get; set; }
        public int YIndex { get; set; }
        public int BuildingNumber { get; set; }

        public Vector2 LocationCenter { get; set; }
    }

}

