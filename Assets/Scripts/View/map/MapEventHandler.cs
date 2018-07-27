using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Controller.Map;
using UnityEngine;

public class MapEventHandler : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MouseEnter()
    {
        MapController.Instance().MouseOnMap();
    }

    public void MouseExit()
    {
        MapController.Instance().MouseOutsideMap();
    }

    public void MouseClick()
    {
        MapController.Instance().MouseClickOnMap();
    }
}
