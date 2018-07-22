using Assets.Scripts.StrategyGame.conf;

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
   

    }

}

