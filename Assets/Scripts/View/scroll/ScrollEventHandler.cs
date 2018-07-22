﻿using Assets.Scripts.Controller.Scroll;
using UnityEngine;

namespace Assets.Scripts.View.scroll
{
    /// <summary>
    /// this class detects events for scrollview.
    /// </summary>

    public class ScrollEventHandler : MonoBehaviour
    {

        private float MouseStartYPosition; // first mouse input position on scrollview

        private ScrollController _scrollController;
        // Use this for initialization
        void Start ()
        {
            _scrollController = ScrollController.Instance(); // it uses singleton singleviewcontroller.
            //Debug.Log(BuildingsList.Count());
        }
	
        // Update is called once per frame
        void Update () {
	  
        }

        // This detects drag starts on scrollview not barrack and powerplant
        // assign yposition float to y position of starting input point
        public void OnDragStart()
        {  
            MouseStartYPosition = Input.mousePosition.y;  
            //Debug.Log("scrollview drag started.");
        }

        //while mouse input changing, it calculates difference on y direction and if it is changing (!=0) call scrollviewcontroller
        // move buildings functions.
        public void OnDragMause()
        {  
            float yPositionDifference = Input.mousePosition.y - MouseStartYPosition ;
            //Debug.Log(yPositionDifference);
            if (yPositionDifference != 0)
            {
                _scrollController.MoveBuildings(yPositionDifference);
            }
        
        }

    

    }
}