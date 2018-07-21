using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.View.scrollview;
using UnityEditor;
using UnityEngine;
using UnityEngine.XR.WSA.Persistence;

namespace Assets.Scripts.ScrollviewViewModel
{
    /// <summary>
    /// this is singleton class for scrollview. one of the main controller in this game
    /// </summary>
   
    public class ScrollviewController
    {
        // keep building controller for every building
        private List<ScrollviewBuildingController> _buildingControllers = new List<ScrollviewBuildingController>();  
        public static ScrollviewController instance = null; // singleton instance

        private ScrollviewController()
        {
            AddScrollviewBuildingControllers(); // singleton constructor, calling this function to add all building controllers
        }

        // creating instance
        public static ScrollviewController Instance()
        {
            if (instance == null)
            {
                instance = new ScrollviewController();
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
                ScrollviewBuildingController _scrollviewBuildingController = new ScrollviewBuildingController(buildingList.ElementAt(i));
                _buildingControllers.Add(_scrollviewBuildingController);
            }
        }
    }
}
