using Assets.Scripts.Controller.Scroll;
using UnityEngine;

namespace Assets.Scripts.View.scroll
{
    /// <summary>
    /// this is parent class for barrack and power plant, it is inherited from view interface class.
    /// </summary>
 
    public class ScrollBuildingView : MonoBehaviour, IScrollBuildingView
    {
        // global game object for buildings to reach them from unity scene
        public GameObject BuildingObject;  

        // this parent has viewmodel and getter and setter methods.
        public ScrollBuildingController ScrollBuildingController { get; set; } 

        GameObject IScrollBuildingView.BuildingObject
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
            ScrollBuildingController.RelocateBuilding();
        }
    }

}
