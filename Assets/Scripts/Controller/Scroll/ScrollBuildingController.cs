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

        public ScrollBuildingView _scrollBuildingView; 
        // this constructor link between view and viewmodel. It links building and its controller
        public ScrollBuildingController(ScrollBuildingView building)
        {
            _scrollBuildingView = building;
            building.ScrollBuildingController = this;
            //Debug.Log(this);
        }

        // this function makes objects pooling, when object goes down or up from the screen, it is relocated to new coordinates.
        public void RelocateBuilding () {
            if (_scrollBuildingView.transform.localPosition.y < Config.ScrollviewMinYValue)
            {
                _scrollBuildingView.transform.localPosition = new Vector2(_scrollBuildingView.transform.localPosition.x, _scrollBuildingView.transform.localPosition.y + Config.ScrollviewHeight);
            }
            else if (_scrollBuildingView.transform.localPosition.y > Config.ScrollviewMaxYValue)
            {
                _scrollBuildingView.transform.localPosition = new Vector2(_scrollBuildingView.transform.localPosition.x, _scrollBuildingView.transform.localPosition.y - Config.ScrollviewHeight);
            }
        }


    }
}
