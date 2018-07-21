using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.ScrollviewViewModel;
using Assets.Scripts.StrategyGame.conf;
using Assets.Scripts.View;
using UnityEngine;

namespace Assets.Scripts.View.scrollview
{
    public class ScrollBuildingView : MonoBehaviour, IScrollBuildingView
    {
        public GameObject BuildingObject;
        public ScrollviewBuildingController ScrollviewBuildingController { get; set; }

        GameObject IScrollBuildingView.buildingObject
        {
            get
            {
                return this.gameObject;
            }
        }
        // Update is called once per frame
        void Update()
        {
            ScrollviewBuildingController.RelocateBuilding();
        }
    }

}
