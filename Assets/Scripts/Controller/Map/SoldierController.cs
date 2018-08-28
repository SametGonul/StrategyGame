using Assets.Scripts.Model;
using Assets.Scripts.View.map;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Controller.Map
{
    public class SoldierController
    {

        private int? _soldierXIndex = null;
        private int? _soldierYIndex = null;
        private int? _moveFinishXIndex = null;
        private int? _moveFinishYIndex = null;

        private bool _soldierSelected = false;
        private bool _soldierMoving = false;
        private bool _hasPath = false;
        private readonly SoldierView _soldierView;
        private int _i = 2; 

        private List<GridCellModel> _exPath;

        public SoldierController(SoldierView soldierView)
        {
            _soldierView = soldierView;
        }
       
        // this function called update of soldier view when soldier is given end point, it starts to move
        // It goes 1 grid cell from the path when conditions are provided. 
        public void Move()
        {
            
            if (_soldierXIndex != null && _soldierYIndex != null && _moveFinishXIndex != null &&
                _moveFinishYIndex != null)
            {
                if (_soldierXIndex > _moveFinishXIndex)
                {
                    _soldierMoving = true;
                    _soldierView.transform.localPosition = new Vector2(_soldierView.transform.localPosition.x, 
                        _soldierView.transform.localPosition.y + 200 * Time.deltaTime);
                    if (_soldierView.transform.localPosition.y >
                        MapController.Instance().GetObjectList()[(int)_moveFinishXIndex, (int)_moveFinishYIndex].transform.localPosition.y)
                    {
                        _soldierView.transform.localPosition = MapController.Instance().GetObjectList()[(int)_moveFinishXIndex, (int)_moveFinishYIndex].transform.localPosition;
                        _soldierMoving = false;
                        ContinueMove();
                    }
                }

                else if (_soldierXIndex < _moveFinishXIndex)
                {
                    _soldierMoving = true;
                    _soldierView.transform.localPosition = new Vector2(
                        _soldierView.transform.localPosition.x,
                        _soldierView.transform.localPosition.y - 200 * Time.deltaTime);

                    if (_soldierView.transform.localPosition.y <
                        MapController.Instance().GetObjectList()[(int)_moveFinishXIndex, (int)_moveFinishYIndex].transform.localPosition.y)
                    {
                        _soldierView.transform.localPosition = MapController.Instance().GetObjectList()[(int)_moveFinishXIndex, (int)_moveFinishYIndex].transform.localPosition;
                        _soldierMoving = false;
                        ContinueMove();
                    }

                }
                else if (_soldierYIndex > _moveFinishYIndex)
                {
                    _soldierMoving = true;
                    _soldierView.transform.localPosition = new Vector2(
                        _soldierView.transform.localPosition.x - 200 * Time.deltaTime,
                        _soldierView.transform.localPosition.y);

                    if (_soldierView.transform.localPosition.x <
                        MapController.Instance().GetObjectList()[(int)_moveFinishXIndex, (int)_moveFinishYIndex].transform.localPosition.x)
                    {
                        _soldierView.transform.localPosition = MapController.Instance().GetObjectList()[(int)_moveFinishXIndex, (int)_moveFinishYIndex].transform.localPosition;
                        _soldierMoving = false;
                        ContinueMove();

                    }

                }
                else if (_soldierYIndex < _moveFinishYIndex)
                {
                    _soldierMoving = true;
                    _soldierView.transform.localPosition = new Vector2(
                        _soldierView.transform.localPosition.x + 200 * Time.deltaTime,
                        _soldierView.transform.localPosition.y);
                    if (_soldierView.transform.localPosition.x >
                        MapController.Instance().GetObjectList()[(int)_moveFinishXIndex, (int)_moveFinishYIndex].transform.localPosition.x)
                    {
                        _soldierView.transform.localPosition = MapController.Instance().GetObjectList()[(int)_moveFinishXIndex, (int)_moveFinishYIndex].transform.localPosition;
                        _soldierMoving = false;
                        ContinueMove();
                    }

                }
            }
        }

        // this function is called while path is not finished
        private void ContinueMove()
        {
            if (_soldierXIndex != null && _soldierYIndex != null && _moveFinishXIndex != null && _moveFinishYIndex != null)
            {
                MapController.Instance().GetGridCellArray()[(int) _soldierXIndex, (int) _soldierYIndex].GridCellType = GridCellTypes.Empty;
                MapController.Instance().GetGridCellArray()[(int) _moveFinishXIndex, (int) _moveFinishYIndex].GridCellType = GridCellTypes.Soldier;
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
            }
        }

        private IEnumerator Timer()
        {
            yield return new WaitForSeconds(2f);

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
