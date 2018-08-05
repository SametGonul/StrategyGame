using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;

namespace Assets.Scripts.Model
{

    /// <summary>
    /// Event checker enumaration,when new build type added this will be used.
    /// </summary>
    public enum BuildingEventTypes
    {
        None, Barrack, PowerPlant
    }

    public class BuildingEventType
    {
        private BuildingEventTypes _eventType;

        public List<BuildingEventTypes> GetValidEventTypes()
        {
            BuildingEventTypes[] validEventTypes = (BuildingEventTypes[])Enum.GetValues(typeof(BuildingEventTypes));
            List<BuildingEventTypes> validEventTypesList = validEventTypes.ToList();
            validEventTypesList.Remove(BuildingEventTypes.None);
            return validEventTypesList;
        }

    }

    
}
