using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.ScrollviewViewModel;
using Assets.Scripts.StrategyGame.conf;
using Assets.Scripts.View;
using UnityEngine;

namespace Assets.Scripts.View.scrollview
{
    /// <summary>
    /// this is parent class for barrack and power plant, it is inherited from view interface class.
    /// </summary>
 
    public class ScrollBuildingView : MonoBehaviour, IScrollBuildingView
    {
        // global game object for buildings to reach them from unity scene
        public GameObject BuildingObject;  

        // this parent has viewmodel and getter and setter methods.
        public ScrollviewBuildingController ScrollviewBuildingController { get; set; } 

        GameObject IScrollBuildingView.buildingObject
        {
            get
            {
                return this.gameObject;
            }
        }
        // Update is called once per frame

        // to check relocation with object pooling, it calls that functions from controller.
        void Update()
        {
            ScrollviewBuildingController.RelocateBuilding();
        }
    }

}
