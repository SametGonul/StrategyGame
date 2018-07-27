using Assets.Scripts.StrategyGame.conf;
using UnityEngine;

namespace Assets.Scripts.Model
{

    /// <summary>
    /// Barrack model class, these get methods returns it's contstant values.
    /// </summary>
    public class ScrollBarrackModel : IScrollBuildingModel
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

        public int XIndex { get; set; }
        public int YIndex { get; set; }
        public int BuildingNumber { get; set; }
    }

}

