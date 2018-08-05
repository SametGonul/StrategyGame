using Assets.Scripts.Controller.Map;
using Assets.Scripts.Controller.Scroll;
using Assets.Scripts.Model;
using UnityEngine;

namespace Assets.Scripts.View.scroll
{
    /// <summary>
    /// this class' purpose is to check events start and continuous from barrack.
    /// </summary>
 
    public class ScrollBarrackEventHandler : MonoBehaviour {

        // this function detects start of drag from a barrack building.
        public void BarrackDragStart()
        {
            ScrollController.Instance().ScrollDragEventChecker.BuildingEventType = BuildingEventTypes.Barrack;
        }

        // when mouse drag finisihed on map, it resets the scroll drag checker.
        public void BarrackDragFinish()
        {
            if (MapController.Instance().GetMouseEventChecker())
                MapController.Instance().DragFinished();
            ScrollController.Instance().ScrollDragEventChecker.BuildingEventType = BuildingEventTypes.None;
        }
    }
}
