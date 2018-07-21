using UnityEngine;

namespace Assets.Scripts.ScrollviewViewModel
{

    /// <summary>
    /// this class to detect events for powerplants
    /// </summary>
  
    public class ScrollviewPowerPlantEventHandler : MonoBehaviour {

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
            //Debug.Log("PP drag started.");
        }

        // TO DO: this function will detect if drag continuous on map and help to build power plant on map.
        public void PowerPlantDragOn()
        {
            //Debug.Log("PP drag on.");
        }
    }
}
