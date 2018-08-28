using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Model;
using Assets.Scripts.View.scroll;
using UnityEngine;
using Assets.Scripts.StrategyGame.conf;

namespace Assets.Scripts.Controller.Scroll
{
    /// <summary>
    /// this is singleton class for scrollview. one of the main controller in this game
    /// </summary>
   
    public class ScrollController
    {
        // keep building controller for every building
        private readonly List<ScrollBuildingController> _buildingControllers = new List<ScrollBuildingController>();  
        private static ScrollController _instance = null; // singleton instance

        public bool BarrackEventChecker = false;
        public bool PowerPlantEventChecker = false;

        public EventCheckModel ScrollDragEventChecker;
   

        private ScrollController()
        {
            AddScrollviewBuildingControllers(); // singleton constructor, calling this function to add all building controllers
            ScrollDragEventChecker = new EventCheckModel {BuildingEventType = BuildingEventTypes.None};
        }
       
        // creating instance
        public static ScrollController Instance()
        {
            return _instance ?? (_instance = new ScrollController());
        }
        
        // move buildings while drag occurs in scroll.
        public void MoveBuildings(float difference)
        {
            for (int i = 0; i < _buildingControllers.Count(); i++)
            {
                if (difference < 0)
                {
                    _buildingControllers.ElementAt(i).ScrollBuildingView.transform.position = new Vector2(
                        _buildingControllers.ElementAt(i).ScrollBuildingView.transform.position.x,
                        _buildingControllers.ElementAt(i).ScrollBuildingView.transform.position.y + (Config.ScrollBuildingSpeed * Time.deltaTime));
                }

                else
                {
                    _buildingControllers.ElementAt(i).ScrollBuildingView.transform.position = new Vector2(
                        _buildingControllers.ElementAt(i).ScrollBuildingView.transform.position.x,
                        _buildingControllers.ElementAt(i).ScrollBuildingView.transform.position.y + (-Config.ScrollBuildingSpeed * Time.deltaTime));
                }

            }
        }

        // Find and  add building objects controllers in scroll to list.
        private void AddScrollviewBuildingControllers()
        {
            IEnumerable<ScrollBuildingView> buildingList = Object.FindObjectsOfType<MonoBehaviour>().OfType<ScrollBuildingView>().ToList();
            for (int i = 0; i < buildingList.Count(); i++)
            {
                ScrollBuildingController scrollBuildingController = new ScrollBuildingController(buildingList.ElementAt(i));
                _buildingControllers.Add(scrollBuildingController);
                
            }
        }
    }
}
