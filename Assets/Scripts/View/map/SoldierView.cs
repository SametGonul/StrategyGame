using System;
using System.Collections.Generic;
using Assets.Scripts.Controller.Map;
using Assets.Scripts.Model;
using JetBrains.Annotations;
using UnityEngine;

namespace Assets.Scripts.View.map
{
    /// <summary>
    /// Soldier view class,specific for all soldiers.
    /// Every soldier has their own controller.
    /// </summary>
    public class SoldierView : MonoBehaviour {

        // Use this for initialization
  
        private SoldierController _soldierController;
        
        void Start ()
        {
            _soldierController = new SoldierController(this);
        }
	
        // Update is called once per frame
        void Update () {
            _soldierController.Move();
        }

        public SoldierController GetSoldierController()
        {
            return this._soldierController;
        }

    }
}
