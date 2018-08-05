using Assets.Scripts.StrategyGame.conf;

namespace Assets.Scripts.Model
{
    public abstract class BuildingFactory
    {
        public abstract IScrollBuildingModel CreateScrollBuildingModel(BuildingEventTypes eventType);
    }
    /// <summary> it is abstract building factory class </summary> 
    public class ScrollBuildingModelFactory : BuildingFactory
    {

        // factory function,according to name it creates model. 

        public override IScrollBuildingModel CreateScrollBuildingModel(BuildingEventTypes eventType)
        {
            switch (eventType)
            {
                case BuildingEventTypes.Barrack:
                    return new ScrollBarrackModel();
                case BuildingEventTypes.PowerPlant:
                    return new ScrollPowerPlantModel();
                default:
                    return null;
            }
        }
    }
}
