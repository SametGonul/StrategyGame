using Assets.Scripts.StrategyGame.conf;

namespace Assets.Scripts.Model
{
    public abstract class BuildingFactory
    {
        public abstract IScrollBuildingModel CreateScrollBuildingModel(string buildingName);
    }
    /// <summary> it is abstract building factory class </summary> 
    public class ScrollBuildingModelFactory : BuildingFactory
    {

        // factory function,according to name it creates model. 

        public override IScrollBuildingModel CreateScrollBuildingModel(string buildingName)
        {
            switch (buildingName)
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
