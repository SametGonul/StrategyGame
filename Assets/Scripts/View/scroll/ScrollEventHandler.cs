using System;
using Assets.Scripts.Controller.Scroll;
using UnityEngine;

namespace Assets.Scripts.View.scroll
{
    /// <summary>
    /// this class detects events for scrollview.
    /// </summary>

    public class ScrollEventHandler : MonoBehaviour
    {

        private float _mouseStartYPosition; // first mouse input position on scrollview

        private ScrollController _scrollController;
        void Start ()
        {
            _scrollController = ScrollController.Instance(); // it uses singleton singleviewcontroller.
        }

        // This detects drag starts on scrollview not barrack and powerplant
        // assign yposition float to y position of starting input point
        public void OnDragStart()
        {  
            _mouseStartYPosition = Input.mousePosition.y;  
        }

        //while mouse input changing, it calculates difference on y direction and if it is changing (!=0) call scrollviewcontroller
        // move buildings functions.
        public void OnDragMause()
        {  
            float yPositionDifference = Input.mousePosition.y - _mouseStartYPosition ;
            if (yPositionDifference != 0 )
            {
                _scrollController.MoveBuildings(yPositionDifference);
            } 
        }
    }
}
