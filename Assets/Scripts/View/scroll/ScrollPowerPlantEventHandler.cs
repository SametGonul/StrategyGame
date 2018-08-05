using Assets.Scripts.Controller.Map;
using Assets.Scripts.Controller.Scroll;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.View.scroll
{

    /// <summary>
    /// this class to detect events for powerplants
    /// </summary>
  
    public class ScrollPowerPlantEventHandler : MonoBehaviour {



        //this function detects drag is started in powerplant.
        public void PowerPlantDragStart()
        {
            ScrollController.Instance().ScrollDragEventChecker.BuildingEventType = BuildingEventTypes.PowerPlant;
        }

        // this function called when powerplant drag is finished.
        public void PowerPlantDragFinished()
        {
            if(MapController.Instance().GetMouseEventChecker())
                MapController.Instance().DragFinished();
            ScrollController.Instance().ScrollDragEventChecker.BuildingEventType = BuildingEventTypes.None;
        }
        
    }
}
