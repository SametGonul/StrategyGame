using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.View.scroll;
using UnityEngine;

namespace Assets.Scripts.Controller.Scroll
{
    /// <summary>
    /// this is singleton class for scrollview. one of the main controller in this game
    /// </summary>
   
    public class ScrollController
    {
        // keep building controller for every building
        private List<ScrollBuildingController> _buildingControllers = new List<ScrollBuildingController>();  
        public static ScrollController instance = null; // singleton instance

        private ScrollController()
        {
            AddScrollviewBuildingControllers(); // singleton constructor, calling this function to add all building controllers
        }

        // creating instance
        public static ScrollController Instance()
        {
            if (instance == null)
            {
                instance = new ScrollController();
            }
            return instance;        
        }
        

        // Use this for initialization
        
        //
        public void MoveBuildings(float difference)
        {
            int Speed = 500;
            for (int i = 0; i < _buildingControllers.Count(); i++)
            {
                if (difference < 0)
                {
                    _buildingControllers.ElementAt(i)._scrollBuildingView.transform.position = new Vector2(
                        _buildingControllers.ElementAt(i)._scrollBuildingView.transform.position.x,
                        _buildingControllers.ElementAt(i)._scrollBuildingView.transform.position.y + (Speed * Time.deltaTime));
                }

                else
                {
                    _buildingControllers.ElementAt(i)._scrollBuildingView.transform.position = new Vector2(
                        _buildingControllers.ElementAt(i)._scrollBuildingView.transform.position.x,
                        _buildingControllers.ElementAt(i)._scrollBuildingView.transform.position.y + (-Speed * Time.deltaTime));
                }

            }

        }

        private void AddScrollviewBuildingControllers()
        {
            IEnumerable<ScrollBuildingView> buildingList = GameObject.FindObjectsOfType<MonoBehaviour>().OfType<ScrollBuildingView>();
            for (int i = 0; i < buildingList.Count(); i++)
            {
                ScrollBuildingController scrollBuildingController = new ScrollBuildingController(buildingList.ElementAt(i));
                _buildingControllers.Add(scrollBuildingController);
            }
        }
    }
}
