using Assets.Scripts.StrategyGame.conf;
using Assets.Scripts.View.scroll;
using UnityEngine;

namespace Assets.Scripts.Controller.Scroll
{
    /// <summary>
    /// This class is a view for barracks and powerplants. Every single building has linked their own controller.
    /// </summary>
    public class ScrollBuildingController
    {

        public ScrollBuildingView ScrollBuildingView; 
        // this constructor link between view and viewmodel. It links building and its controller
        public ScrollBuildingController(ScrollBuildingView building)
        {
            ScrollBuildingView = building;
            building.ScrollBuildingController = this;
            //Debug.Log(this);
        }

        // this function makes objects pooling, when object goes down or up from the screen, it is relocated to new coordinates.
        public void RelocateBuilding () {
            if (ScrollBuildingView.transform.localPosition.y < Config.ScrollviewMinYValue)
            {
                ScrollBuildingView.transform.localPosition = new Vector2(ScrollBuildingView.transform.localPosition.x, ScrollBuildingView.transform.localPosition.y + Config.ScrollviewHeight);
            }
            else if (ScrollBuildingView.transform.localPosition.y > Config.ScrollviewMaxYValue)
            {
                ScrollBuildingView.transform.localPosition = new Vector2(ScrollBuildingView.transform.localPosition.x, ScrollBuildingView.transform.localPosition.y - Config.ScrollviewHeight);
            }
        }


    }
}
