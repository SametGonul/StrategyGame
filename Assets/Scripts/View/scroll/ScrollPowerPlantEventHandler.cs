using Assets.Scripts.Controller.Map;
using Assets.Scripts.Controller.Scroll;
using UnityEngine;

namespace Assets.Scripts.View.scroll
{

    /// <summary>
    /// this class to detect events for powerplants
    /// </summary>
  
    public class ScrollPowerPlantEventHandler : MonoBehaviour {

        // Use this for initialization
        void Start () {
		
        }
	
        // Update is called once per frame
        void Update()
        {

        }

        // TO DO: this function will detect drag will be started in powerplant.
        public void PowerPlantDragStart()
        {
            ScrollController.Instance().PowerPlantEventChecker = true;
        }

        public void PowerPlantDragFinished()
        {
            if(MapController.Instance().GetMouseEventChecker())
                MapController.Instance().DragFinished();
            ScrollController.Instance().PowerPlantEventChecker = false;
        }
        
    }
}
