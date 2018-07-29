using Assets.Scripts.Model;
using Assets.Scripts.View.map;
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

        private readonly SoldierView _soldierView;

        private int _i = 2;

        public SoldierController(SoldierView soldierView)
        {
            _soldierView = soldierView;
        }
       
        public void Move()
        {
            if (_soldierXIndex != null && _soldierYIndex != null && _moveFinishXIndex != null &&
                _moveFinishYIndex != null)
            {
                _soldierMoving = true;

                if (_soldierXIndex > _moveFinishXIndex)
                {

                    _soldierView.transform.localPosition = new Vector2(_soldierView.transform.localPosition.x, _soldierView.transform.localPosition.y + 200 * Time.deltaTime);
                    if (_soldierView.transform.localPosition.y >
                        MapController.Instance().GetObjectList()[(int)_moveFinishXIndex, (int)_moveFinishYIndex].transform.localPosition.y)
                    {
                        _soldierView.transform.localPosition = MapController.Instance().GetObjectList()[(int)_moveFinishXIndex, (int)_moveFinishYIndex].transform.localPosition;
                        _soldierSelected = false;
                        ContinueMove();
                    }
                }

                else if (_soldierXIndex < _moveFinishXIndex)
                {
                    _soldierView.transform.localPosition = new Vector2(
                        _soldierView.transform.localPosition.x,
                        _soldierView.transform.localPosition.y - 200 * Time.deltaTime);

                    if (_soldierView.transform.localPosition.y <
                        MapController.Instance().GetObjectList()[(int)_moveFinishXIndex, (int)_moveFinishYIndex].transform.localPosition.y)
                    {
                        _soldierView.transform.localPosition = MapController.Instance().GetObjectList()[(int)_moveFinishXIndex, (int)_moveFinishYIndex].transform.localPosition;

                        _soldierSelected = false;
                        ContinueMove();
                    }

                }
                else if (_soldierYIndex > _moveFinishYIndex)
                {
                    _soldierView.transform.localPosition = new Vector2(
                        _soldierView.transform.localPosition.x - 200 * Time.deltaTime,
                        _soldierView.transform.localPosition.y);

                    if (_soldierView.transform.localPosition.x <
                        MapController.Instance().GetObjectList()[(int)_moveFinishXIndex, (int)_moveFinishYIndex].transform.localPosition.x)
                    {
                        _soldierView.transform.localPosition = MapController.Instance().GetObjectList()[(int)_moveFinishXIndex, (int)_moveFinishYIndex].transform.localPosition;

                        _soldierSelected = false;
                        ContinueMove();

                    }

                }
                else if (_soldierYIndex < _moveFinishYIndex)
                {
                    _soldierView.transform.localPosition = new Vector2(
                        _soldierView.transform.localPosition.x + 200 * Time.deltaTime,
                        _soldierView.transform.localPosition.y);
                    if (_soldierView.transform.localPosition.x >
                        MapController.Instance().GetObjectList()[(int)_moveFinishXIndex, (int)_moveFinishYIndex].transform.localPosition.x)
                    {
                        _soldierView.transform.localPosition = MapController.Instance().GetObjectList()[(int)_moveFinishXIndex, (int)_moveFinishYIndex].transform.localPosition;

                        _soldierSelected = false;
                        ContinueMove();
                    }

                }

            }
        }

        private void ContinueMove()
        {
            List<List<int>> exPath = MapController.Instance().GetExamplePath();
            MapController.Instance().GetGridCellArray()[(int)_soldierXIndex, (int)_soldierYIndex].GridCellType = GridCellTypes.Empty;
            MapController.Instance().GetGridCellArray()[(int)_moveFinishXIndex, (int)_moveFinishYIndex].GridCellType = GridCellTypes.Soldier;
            if (_i < MapController.Instance().GetExamplePath().Count)
            {

                _soldierXIndex = _moveFinishXIndex;
                _soldierYIndex = _moveFinishYIndex;
                _moveFinishXIndex = exPath[_i][0];
                _moveFinishYIndex = exPath[_i][1];
                _soldierSelected = true;
                _i++;
            }
            else
            {
                _soldierMoving = false;
            }

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
