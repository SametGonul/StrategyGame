using Assets.Scripts.Controller.Map;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.View.map
{   /// <summary>
    /// This class handles events on map. 
    /// </summary>
    public class MapEventHandler : MonoBehaviour {

        private int _mouseClickType;

        // This function called when mouse enter the map.
        public void MouseEnter()
        {
            MapController.Instance().MouseOnMap();
        }

        // This function called when mouse exit from the map
        public void MouseExit()
        {
            MapController.Instance().MouseOutsideMap();
        }

        //This function defines mouse click button(left or right)
        // if left sets variable 0, else if right sets 1
        public void MouseClick()
        {
            if (Input.GetMouseButtonUp(0))
            {
                _mouseClickType = 0;
            }

            else if (Input.GetMouseButtonUp(1))
                _mouseClickType = 1;

            MapController.Instance().MouseClickOnMap(_mouseClickType);
        }
    }
}
