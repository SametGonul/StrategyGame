using Assets.Scripts.StrategyGame.conf;

namespace Assets.Scripts.Model
{


    /// <summary> it is abstract building factory class </summary> 
    public class ScrollBuildingModelFactory : IScrollBuildingModel
    {

        // implementation of class members
        public string Name { get; private set; }
        public int VerticalSize { get; private set; }
        public int HorizontalSize { get; private set; }

        // factory function,according to name it creates model. 
        public IScrollBuildingModel CreateBuilding(string name)
        {
            switch (name)
            {
                case Config.BarrackName:
                    return new ScrollBarrackModel();
                case Config.PowerPlantName:
                    return new ScrollPowerPlantModel();
                default:
                    return null;
            }
        }

    }
}
