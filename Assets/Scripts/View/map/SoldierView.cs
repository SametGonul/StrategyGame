using System;
using System.Collections;
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
        public int? _soldierXIndex = null;
        public int? _soldierYIndex = null;
        public int? _moveFinishXIndex = null;
        public int? _moveFinishYIndex = null;

        public bool _soldierSelected = false;
        public bool _soldierMoving = false;
        public bool _hasPath = false;
        private int _i = 2;

        public List<GridCellModel> _exPath;
        void Start ()
        {
            _soldierController = new SoldierController(this);
        }
	
        // Update is called once per frame
        void Update () {
            //_soldierController.Move();
        }

        public SoldierController GetSoldierController()
        {
            return this._soldierController;
        }

        /// <summary>
        /// Coroutine starter function
        /// </summary>
        public void StartMove()
        {
            StartCoroutine("Move");
        }

        /// <summary>
        /// this coroutine function move,can set time of moving from 1 grid cell to other gridcell
        /// </summary>
        /// <returns></returns>
        public IEnumerator Move()
        {
            while (!(_soldierXIndex == _moveFinishXIndex && _soldierYIndex == _moveFinishYIndex))
            {
                Debug.Log("inside while");
                if (_soldierXIndex != null && _soldierYIndex != null && _moveFinishXIndex != null &&
                                _moveFinishYIndex != null)
                {

                    if (_soldierXIndex > _moveFinishXIndex)
                    {
                        _soldierMoving = true;
                        this.transform.localPosition = new Vector2(this.transform.localPosition.x,
                            this.transform.localPosition.y + 33f);
                        if (this.transform.localPosition.y >
                            MapController.Instance().GetObjectList()[(int)_moveFinishXIndex, (int)_moveFinishYIndex].transform.localPosition.y)
                        {
                            this.transform.localPosition = MapController.Instance().GetObjectList()[(int)_moveFinishXIndex, (int)_moveFinishYIndex].transform.localPosition;
                            _soldierMoving = false;

                        }
                    }

                    else if (_soldierXIndex < _moveFinishXIndex)
                    {
                        _soldierMoving = true;
                        this.transform.localPosition = new Vector2(
                            this.transform.localPosition.x,
                            this.transform.localPosition.y - 33f);

                        if (this.transform.localPosition.y <
                            MapController.Instance().GetObjectList()[(int)_moveFinishXIndex, (int)_moveFinishYIndex].transform.localPosition.y)
                        {
                            this.transform.localPosition = MapController.Instance().GetObjectList()[(int)_moveFinishXIndex, (int)_moveFinishYIndex].transform.localPosition;
                            _soldierMoving = false;

                        }

                    }
                    else if (_soldierYIndex > _moveFinishYIndex)
                    {
                        _soldierMoving = true;
                        this.transform.localPosition = new Vector2(
                            this.transform.localPosition.x - 33f,
                            this.transform.localPosition.y);

                        if (this.transform.localPosition.x <
                            MapController.Instance().GetObjectList()[(int)_moveFinishXIndex, (int)_moveFinishYIndex].transform.localPosition.x)
                        {
                            this.transform.localPosition = MapController.Instance().GetObjectList()[(int)_moveFinishXIndex, (int)_moveFinishYIndex].transform.localPosition;
                            _soldierMoving = false;


                        }

                    }
                    else if (_soldierYIndex < _moveFinishYIndex)
                    {
                        _soldierMoving = true;
                        this.transform.localPosition = new Vector2(
                            this.transform.localPosition.x + 33f,
                            this.transform.localPosition.y);
                        if (this.transform.localPosition.x >
                            MapController.Instance().GetObjectList()[(int)_moveFinishXIndex, (int)_moveFinishYIndex].transform.localPosition.x)
                        {
                            this.transform.localPosition = MapController.Instance().GetObjectList()[(int)_moveFinishXIndex, (int)_moveFinishYIndex].transform.localPosition;
                            _soldierMoving = false;


                        }

                    }
                }

                MapController.Instance().GetGridCellArray()[(int)_soldierXIndex, (int)_soldierYIndex].GridCellType = GridCellTypes.Empty;
                MapController.Instance().GetGridCellArray()[(int)_moveFinishXIndex, (int)_moveFinishYIndex].GridCellType = GridCellTypes.Soldier;
                if (_i < _exPath.Count)
                {
                    _soldierXIndex = _moveFinishXIndex;
                    _soldierYIndex = _moveFinishYIndex;
                    _moveFinishXIndex = _exPath[_i].XIndex;
                    _moveFinishYIndex = _exPath[_i].YIndex;

                    _i++;
                }

                else if (_i == _exPath.Count)
                {
                    _soldierXIndex = _moveFinishXIndex;
                    _soldierYIndex = _moveFinishYIndex;
                    _i = 2;
                    _hasPath = false;
                }
                yield return new WaitForSeconds(2f);
            }
            
            
        }
        public List<GridCellModel> GetSoldierPath()
        {
            return _exPath;
        }

        public void SetSoldierPath(List<GridCellModel> value)
        {

            _exPath = value;
        }

        public bool GetSoldierHasPath()
        {
            return _hasPath;
        }

        public void SetSoldierHasPath(bool value)
        {
            _hasPath = value;
        }

        public bool GetSoldierMoving()
        {
            return _soldierMoving;
        }

        public void SetSoldierMoving(bool value)
        {
            _soldierMoving = value;
        }

        public bool GetSoldierSelected()
        {
            return _soldierSelected;
        }

        public void SetSoldierSelected(bool value)
        {
            _soldierSelected = value;
        }
        public int? GetSoldierXIndex()
        {
            return _soldierXIndex;
        }

        public int? GetSoldierYIndex()
        {
            return _soldierYIndex;
        }

        public int? GetSoldierMovingXIndex()
        {
            return _moveFinishXIndex;
        }

        public int? GetSoldierMovingYIndex()
        {
            return _moveFinishYIndex;
        }

        public void SetSoldierXIndex(int? value)
        {
            _soldierXIndex = value;
        }

        public void SetSoldierYIndex(int? value)
        {
            _soldierYIndex = value;
        }

        public void SetSoldierMovingXIndex(int? value)
        {
            _moveFinishXIndex = value;
        }

        public void SetSoldierMovingYIndex(int? value)
        {
            _moveFinishYIndex = value;
        }
    }
}
