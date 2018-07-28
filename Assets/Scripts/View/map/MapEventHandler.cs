using Assets.Scripts.Controller.Map;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.Scripts.View.map
{
    public class MapEventHandler : MonoBehaviour {

        private int _mouseClickType;

        public void MouseEnter()
        {
            MapController.Instance().MouseOnMap();
        }

        public void MouseExit()
        {
            MapController.Instance().MouseOutsideMap();
        }

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
