using UnityEngine;

namespace Assets.Scripts.Model
{

    /// <summary>
    /// Event checker enumaration,when new build type added this will be used.
    /// </summary>
    public enum BuildingEventTypes
    {
        None,Barrack,PowerPlant
    }

    public interface IEventModel
    {
        BuildingEventTypes BuildingEventType { get; set; }
    }


}

