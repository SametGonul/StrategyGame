using Assets.Scripts.View.scrollview;
using System.Linq;
using Assets.Scripts.StrategyGame.conf;
using UnityEngine;

namespace Assets.Scripts.ScrollviewViewModel
{
    public class ScrollviewBuildingController
    {

        public ScrollBuildingView _scrollBuildingView;
        public ScrollviewBuildingController(ScrollBuildingView building)
        {
            _scrollBuildingView = building;
            building.ScrollviewBuildingController = this;
        }
        public void RelocateBuilding () {
            if (_scrollBuildingView.transform.localPosition.y < Config.ScrollviewMinYValue)
            {
                _scrollBuildingView.transform.localPosition = new Vector2(_scrollBuildingView.transform.localPosition.x, _scrollBuildingView.transform.localPosition.y + Config.ObjectPoolConstant);
            }
            else if (_scrollBuildingView.transform.localPosition.y > Config.ScrollviewMaxYValue)
            {
                _scrollBuildingView.transform.localPosition = new Vector2(_scrollBuildingView.transform.localPosition.x, _scrollBuildingView.transform.localPosition.y - Config.ObjectPoolConstant);
            }
        }


    }
}
