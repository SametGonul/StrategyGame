using System.Runtime.Remoting.Messaging;
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

        public bool CheckCollision(int clickedXIndex, int clickedYIndex)
        {

            if (this.XIndex - clickedXIndex >= -1 &&
                this.XIndex - clickedXIndex <= 1 &&
                this.YIndex - clickedYIndex >= -1 &&
                this.YIndex - clickedYIndex <= 1)
            {
                return true;
            }

            return false;
        }
    }

}

