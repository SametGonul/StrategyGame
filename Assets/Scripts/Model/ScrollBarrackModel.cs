using Assets.Scripts.StrategyGame.conf;

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

      
    }

}

