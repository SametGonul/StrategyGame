using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Models;
using Assets.Scripts.ScrollviewViewModel;
using Assets.Scripts.View;
using Assets.Scripts.View.scrollview;
using UnityEngine;
using UnityEngine.SceneManagement;
using Assets.Scripts.StrategyGame.conf;
using GameObject = UnityEngine.GameObject;

public class ScrollviewEventHandler : MonoBehaviour
{

    private float MouseStartYPosition;

    private ScrollviewController _scrollviewController;
	// Use this for initialization
	void Start ()
	{
	    _scrollviewController = new ScrollviewController();
        //Debug.Log(BuildingsList.Count());
	}
	
	// Update is called once per frame
	void Update () {
	  
    }

    public void OnDragStart()
    {  
        MouseStartYPosition = Input.mousePosition.y;  
        //Debug.Log("scrollview drag started.");
    }

    public void OnDragMause()
    {  
        float yPositionDifference = Input.mousePosition.y - MouseStartYPosition ;
        //Debug.Log(yPositionDifference);
        if (yPositionDifference != 0)
        {
            _scrollviewController.MoveBuildings(yPositionDifference);
        }
        
    }

    

}
