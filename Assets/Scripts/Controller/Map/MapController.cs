using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Assets.Scripts.Controller.Info;
using Assets.Scripts.Controller.Scroll;
using Assets.Scripts.Model;
using Assets.Scripts.StrategyGame.conf;
using Assets.Scripts.View.map;
using Unity.Jobs;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Controller.Map
{
    /// <summary>
    /// This is singleton class for map. It is one of the main controller class.
    /// </summary>
    public class MapController
    {
        private static MapController _instance = null;

        private bool _dragEventFromScrollToMap = false;
        GameObject[,] _gridCellGameObjectArray;
 
        private int? _activeCellPositionX = null;
        private int? _activeCellPositionY = null;

        private GridModel[,] _gridCellArray;

        private bool _barrackCollisionChecker = false;
        private bool _powerPlantCollisionChecker = false;

        private int _barrackCounter = 1;

        private int _powerPlantCounter = 1;
        private List<IScrollBuildingModel> barracks = new List<IScrollBuildingModel>();
        private List<IScrollBuildingModel> powerPlants = new List<IScrollBuildingModel>();
        private List<GameObject> soldiersList = new List<GameObject>();
        private bool _soldierClickedChecker = false;

        private int? _soldierXIndex = null;
        private int? _soldierYIndex = null;
        private int? _moveFinishXIndex = null;
        private int? _moveFinishYIndex = null;
        private int? _soldierIndexInList = null;

        private MapController()
        {
            MapView exMapView = Object.FindObjectOfType<MapView>();
            _gridCellGameObjectArray = exMapView.LocateGrids();
            _gridCellArray = GridController.Instance().GetGridCellArray();
        }

        public static MapController Instance()
        {
            return _instance ?? (_instance = new MapController());
        }


        public void DragFinished()
        {
            _dragEventFromScrollToMap = false;
            BuildingFactory factory = new ScrollBuildingModelFactory();
            if (ScrollController.Instance().BarrackEventChecker)
            {
                ScrollController.Instance().BarrackEventChecker = false;
                CheckCollidedBarraksOnDragFinish();          
                if (!_barrackCollisionChecker)
                {
                    IScrollBuildingModel barrack = factory.CreateScrollBuildingModel(Config.BarrackName);
                    if (_activeCellPositionX != null) barrack.XIndex = (int) _activeCellPositionX;
                    if (_activeCellPositionY != null) barrack.YIndex = (int)_activeCellPositionY;
                    barrack.BuildingNumber = _barrackCounter;
                    _barrackCounter++;
                    barracks.Add(barrack);
                    LocateBarrackOnMap();                  
                }
                else
                {
                    _barrackCollisionChecker = false;
                }
            }
            
            else if (ScrollController.Instance().PowerPlantEventChecker)
            {        
                ScrollController.Instance().PowerPlantEventChecker = false;
                CheckCollidedPowerPlantsOnDragFinish();
                if (!_powerPlantCollisionChecker)
                {
                    IScrollBuildingModel powerPlant = factory.CreateScrollBuildingModel(Config.PowerPlantName);
                    if (_activeCellPositionX != null) powerPlant.XIndex = (int)_activeCellPositionX;
                    if (_activeCellPositionY != null) powerPlant.YIndex = (int)_activeCellPositionY;
                    powerPlant.BuildingNumber = _powerPlantCounter;
                    _powerPlantCounter++;
                    powerPlants.Add(powerPlant);
                    LocatePowerPlantOnMap();
                }
                else
                {
                    _powerPlantCollisionChecker = false;
                }
            }
            _activeCellPositionY = null;
            _activeCellPositionX = null;     
        }

        public void MouseOnMap()
        {
            if(ScrollController.Instance().BarrackEventChecker || ScrollController.Instance().PowerPlantEventChecker)
                _dragEventFromScrollToMap = true;
        }

        public void MouseOutsideMap()
        {
            _dragEventFromScrollToMap = false;
        }

        public bool GetMouseEventChecker()
        {
            return _dragEventFromScrollToMap;
        }
        public void FindTheCell()
        {
            float xIndex;
            float yIndex;

            if (_dragEventFromScrollToMap)
            {
                float mouseRatioX = Input.mousePosition.x / Screen.width;
                float mouseRatioY = Input.mousePosition.y / Screen.height;
             
                // 0.5values sets canvas (0,0)point to middle of the screen.
                Vector2 mousePositionRatio = new Vector2(mouseRatioX - 0.5f,mouseRatioY - 0.5f);
                Canvas tempCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();

                Vector2 worldMousePosition = new Vector2(mousePositionRatio.x * tempCanvas.GetComponent<RectTransform>().sizeDelta.x, mousePositionRatio.y * tempCanvas.GetComponent<RectTransform>().sizeDelta.y);

                yIndex = (worldMousePosition.x - Config.TheMostXCoordinate)/Config.GridSize;
                xIndex = (Config.TheMostYCoordinate - worldMousePosition.y)/ Config.GridSize;

                int roundedXIndex = (int)Math.Floor(xIndex);
                int roundedYIndex = (int)Math.Floor(yIndex);

                if (_activeCellPositionX != roundedXIndex || _activeCellPositionY != roundedYIndex )
                {
                    if (_activeCellPositionX != null && _activeCellPositionY != null)
                    {
                        if(ScrollController.Instance().BarrackEventChecker)
                            ColorTheBarrackOnMap((int)_activeCellPositionX, (int)_activeCellPositionY, Color.green);
                        else if (ScrollController.Instance().PowerPlantEventChecker)
                            ColorThePowerPlantOnMap((int) _activeCellPositionX, (int) _activeCellPositionY, Color.green);
                    }
                    
                    _activeCellPositionX = roundedXIndex;
                    _activeCellPositionY = roundedYIndex;

                    if (ScrollController.Instance().BarrackEventChecker)
                    {
                        CheckCollidedBuildingCells();
                        ColorTheBarrackOnMap((int)_activeCellPositionX, (int)_activeCellPositionY, Color.blue);
                    }                        
                    else if (ScrollController.Instance().PowerPlantEventChecker)
                    {
                        CheckCollidedBuildingCells();
                        ColorThePowerPlantOnMap((int)_activeCellPositionX, (int)_activeCellPositionY, Color.yellow);
                    }                     
                }        
            }
        }

        private void ColorTheBarrackOnMap(int xIndex,int yIndex,Color color)
        {
            if (xIndex > 0 && xIndex < Config.VerticalGridNumber - 1 && yIndex > 0 && yIndex < Config.HorizontalGridNumber - 1)
            {
                for (int i = xIndex-1; i <= xIndex+1; i++)
                {
                    for (int j = yIndex - 1; j <= yIndex + 1; j++)
                    {
                        if(_gridCellArray[i,j].GridCellType == GridCellTypes.Empty)
                            _gridCellGameObjectArray[i,j].GetComponent<Image>().color = color;
                        else if (_gridCellArray[i,j].GridCellType == GridCellTypes.Barrack || _gridCellArray[i,j].GridCellType == GridCellTypes.PowerPlant)
                        {
                            _gridCellGameObjectArray[i, j].GetComponent<Image>().color = Color.red;
                        }
                    }
                }
            }
        }

        private void ColorThePowerPlantOnMap(int xIndex, int yIndex, Color color)
        {
            if (xIndex > 0 && xIndex < Config.VerticalGridNumber && yIndex > 0 && yIndex < Config.HorizontalGridNumber - 1)
            {
                for (int i = xIndex - 1; i <= xIndex ; i++)
                {
                    for (int j = yIndex - 1; j <= yIndex + 1 ; j++)
                    {
                        if (_gridCellArray[i, j].GridCellType == GridCellTypes.Empty)
                            _gridCellGameObjectArray[i, j].GetComponent<Image>().color = color;
                        else if (_gridCellArray[i, j].GridCellType == GridCellTypes.Barrack || _gridCellArray[i, j].GridCellType == GridCellTypes.PowerPlant)
                        {
                            _gridCellGameObjectArray[i, j].GetComponent<Image>().color = Color.red;
                        }
                    }
                }
            }
        }

        private void LocateBarrackOnMap()
        {
            for (int i = (int)_activeCellPositionX - 1; i <= (int)_activeCellPositionX + 1; i++)
            {
                for (int j = (int)_activeCellPositionY - 1; j <= (int)_activeCellPositionY + 1; j++)
                {
                    if(_gridCellArray[i,j].GridCellType == GridCellTypes.Empty)
                        _gridCellArray[i, j].GridCellType = GridCellTypes.Barrack;  
                }
            }
        }

        private void LocatePowerPlantOnMap()
        {
            for (int i = (int)_activeCellPositionX - 1; i <= (int)_activeCellPositionX ; i++)
            {
                for (int j = (int)_activeCellPositionY - 1; j <= (int)_activeCellPositionY + 1; j++)
                {
                    if(_gridCellArray[i,j].GridCellType == GridCellTypes.Empty)
                        _gridCellArray[i, j].GridCellType = GridCellTypes.PowerPlant;
                }
            }
        }

        private void CheckCollidedBuildingCells()
        {
            for (int i = 0; i < Config.VerticalGridNumber; i++)
            {
                for (int j = 0; j < Config.HorizontalGridNumber; j++)
                {
                    if (_gridCellArray[i, j].GridCellType == GridCellTypes.Barrack &&
                        _gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color == Color.red)
                    {
                        _gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color = Color.blue;
                       
                    }
                    else if (_gridCellArray[i, j].GridCellType == GridCellTypes.PowerPlant &&
                             _gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color == Color.red)
                    {
                        _gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color = Color.yellow;
                    }
                }
            }
        }

        private void CheckCollidedBarraksOnDragFinish()
        {
            
            for (int i = (int) _activeCellPositionX - 1; i <= (int) _activeCellPositionX + 1; i++)
            {
                for (int j = (int) _activeCellPositionY - 1; j <= (int) _activeCellPositionY + 1; j++)
                {
                    if (_gridCellArray[i, j].GridCellType == GridCellTypes.Barrack &&
                        _gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color == Color.red)
                    {
                        _barrackCollisionChecker = true;
                    }
                    else if (_gridCellArray[i, j].GridCellType == GridCellTypes.PowerPlant &&
                             _gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color == Color.red)
                    {
                        _barrackCollisionChecker = true;
                    }
                }
            }

            if (_barrackCollisionChecker)
            {
                for (int i = (int) _activeCellPositionX - 1; i <= (int) _activeCellPositionX + 1; i++)
                {
                    for (int j = (int) _activeCellPositionY - 1; j <= (int) _activeCellPositionY + 1; j++)
                    {
                        if (_gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color == Color.blue)
                        {
                            _gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color = Color.green;
                            _gridCellArray[i, j].GridCellType = GridCellTypes.Empty;
                        }
                        if (_gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color == Color.red)
                        {
                            if(_gridCellArray[i,j].GridCellType == GridCellTypes.Barrack)
                                _gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color = Color.blue;
                            else if(_gridCellArray[i,j].GridCellType == GridCellTypes.PowerPlant)
                                _gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color = Color.yellow;
                        }

                    }
                }
            }

        }
        private void CheckCollidedPowerPlantsOnDragFinish()
        {
            
            for (int i = (int)_activeCellPositionX - 1; i <= (int)_activeCellPositionX ; i++)
            {
                for (int j = (int)_activeCellPositionY - 1; j <= (int)_activeCellPositionY + 1; j++)
                {
                    if (_gridCellArray[i, j].GridCellType == GridCellTypes.PowerPlant &&
                        _gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color == Color.red)
                    {
                        _powerPlantCollisionChecker = true;
                    }
                    else if (_gridCellArray[i, j].GridCellType == GridCellTypes.Barrack &&
                             _gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color == Color.red)
                    {
                        _powerPlantCollisionChecker = true;
                    }
                }
            }

            if (_powerPlantCollisionChecker)
            {
                for (int i = (int)_activeCellPositionX - 1; i <= (int)_activeCellPositionX ; i++)
                {
                    for (int j = (int)_activeCellPositionY - 1; j <= (int)_activeCellPositionY + 1; j++)
                    {
                        if (_gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color == Color.yellow)
                        {
                            _gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color = Color.green;
                            _gridCellArray[i, j].GridCellType = GridCellTypes.Empty;
                        }
                    }
                }
                for (int i = (int)_activeCellPositionX - 1; i <= (int)_activeCellPositionX ; i++)
                {
                    for (int j = (int)_activeCellPositionY - 1; j <= (int)_activeCellPositionY + 1; j++)
                    {
                        if (_gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color == Color.red)
                        {
                            if (_gridCellArray[i, j].GridCellType == GridCellTypes.Barrack)
                                _gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color = Color.blue;
                            else if (_gridCellArray[i, j].GridCellType == GridCellTypes.PowerPlant)
                                _gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color = Color.yellow;
                        }
                    }
                }
            }

        }

        public void MouseClickOnMap(int mouseClickType)
        {
            float mouseRatioX = Input.mousePosition.x / Screen.width;
            float mouseRatioY = Input.mousePosition.y / Screen.height;

            // 0.5values sets canvas (0,0)point to middle of the screen.
            Vector2 mousePositionRatio = new Vector2(mouseRatioX - 0.5f, mouseRatioY - 0.5f);
            Canvas tempCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();

            float yIndex;
            float xIndex;
            Vector2 worldMousePosition = new Vector2(mousePositionRatio.x * tempCanvas.GetComponent<RectTransform>().sizeDelta.x, mousePositionRatio.y * tempCanvas.GetComponent<RectTransform>().sizeDelta.y);

            yIndex = (worldMousePosition.x - Config.TheMostXCoordinate) / Config.GridSize;
            xIndex = (Config.TheMostYCoordinate - worldMousePosition.y) / Config.GridSize;

            int roundedXIndex = (int)Math.Floor(xIndex);
            int roundedYIndex = (int)Math.Floor(yIndex);
            CheckClickIsGameObject(roundedXIndex, roundedYIndex,mouseClickType);
        }

        private void CheckClickIsGameObject(int clickedXIndex, int clickedYIndex,int mouseClickType)
        {
            
            for (int i = 0; i < barracks.Count; i++)
            {
                if (barracks[i].XIndex - clickedXIndex >= -1 && barracks[i].XIndex - clickedXIndex <= 1)
                {
                    if (barracks[i].YIndex - clickedYIndex >= -1 && barracks[i].YIndex - clickedYIndex <= 1)
                    {
                        InfoController.Instance().ShowInfoClickedObject(barracks[i]);
                        break;
                    }
                }
            }
            for (int i = 0; i < powerPlants.Count; i++)
            {
                if (powerPlants[i].XIndex - clickedXIndex >= 0 && powerPlants[i].XIndex - clickedXIndex <= 1)
                {
                    if (powerPlants[i].YIndex - clickedYIndex >= -1 && powerPlants[i].YIndex - clickedYIndex <= 1)
                    {
                        InfoController.Instance().ShowInfoClickedObject(powerPlants[i]);
                        break;
                    }          
                }
            }

            if (mouseClickType == 0)
            {
                if (_gridCellArray[clickedXIndex, clickedYIndex].GridCellType == GridCellTypes.Soldier)
                {
                    _soldierXIndex = clickedXIndex; 
                    _soldierYIndex = clickedYIndex;
                    _soldierClickedChecker = true;
                    Debug.Log(_soldierClickedChecker);
                }
                else
                {
                    _soldierClickedChecker = false;
                }
            }

            if (mouseClickType == 1)
            {
                Debug.Log(_soldierClickedChecker);
                if (_gridCellArray[clickedXIndex, clickedYIndex].GridCellType == GridCellTypes.Empty &&
                    _soldierClickedChecker)
                {
                    _moveFinishXIndex = clickedXIndex;
                    _moveFinishYIndex = clickedYIndex;

                    DetermineTheSoldierIndex((int)_soldierXIndex,(int)_soldierYIndex);
                }
            }
        }

        public void FindSuitableSoldierPosition(IScrollBuildingModel selectedBuilding)
        {
            int currentYIndex = selectedBuilding.YIndex;
            int currentXIndex = selectedBuilding.XIndex + 2;
            for (; currentYIndex >= selectedBuilding.YIndex - 1; currentYIndex--)
            {
                if (currentXIndex <= Config.VerticalGridNumber - 1)
                {
                    if (_gridCellArray[currentXIndex, currentYIndex].GridCellType == GridCellTypes.Empty)
                    {
                        CreateSoldierOnMap(currentXIndex, currentYIndex);
                        return;
                    }
                }
            }

            if (currentYIndex >= 0)
            {
                for (; currentXIndex >= selectedBuilding.XIndex - 1; currentXIndex--)
                {
                    if (currentXIndex >= 0 && currentXIndex < Config.VerticalGridNumber)
                    {
                        if (_gridCellArray[currentXIndex, currentYIndex].GridCellType == GridCellTypes.Empty)
                        {
                            CreateSoldierOnMap(currentXIndex, currentYIndex);
                            return;
                        }
                    }

                }
            }

            currentXIndex = selectedBuilding.XIndex - 2;
            currentYIndex = selectedBuilding.YIndex - 2;
            if (currentYIndex < 0)
            {
                currentYIndex = currentYIndex + 1;
            }
            if (currentXIndex >= 0 && currentYIndex >= 0)
            {
                for (; currentYIndex <= selectedBuilding.YIndex + 1; currentYIndex++)
                {
                    if (currentYIndex < Config.HorizontalGridNumber)
                    {

                        if (_gridCellArray[currentXIndex, currentYIndex].GridCellType == GridCellTypes.Empty)
                        {
                            CreateSoldierOnMap(currentXIndex, currentYIndex);
                            return;
                        }
                    }
                }
            }

            currentXIndex = selectedBuilding.XIndex - 2;
            currentYIndex = selectedBuilding.YIndex + 2;

            if (currentXIndex < 0)
            {
                currentXIndex = 0;
            }
            if (currentXIndex >= 0 && currentYIndex < Config.HorizontalGridNumber)
            {

                for (; currentXIndex < selectedBuilding.XIndex + 2; currentXIndex++)
                {
                    if (_gridCellArray[currentXIndex, currentYIndex].GridCellType == GridCellTypes.Empty)
                    {
                        CreateSoldierOnMap(currentXIndex, currentYIndex);
                        return;
                    }
                }
            }

            currentXIndex = selectedBuilding.XIndex + 2;
            currentYIndex = selectedBuilding.YIndex + 2;

            if (currentYIndex >= Config.HorizontalGridNumber)
            {
                currentYIndex = currentYIndex - 1;
            }
            if (currentXIndex < Config.VerticalGridNumber && currentYIndex < Config.HorizontalGridNumber)
            {
                for (; currentYIndex > selectedBuilding.YIndex; currentYIndex--)
                {
                    if (_gridCellArray[currentXIndex, currentYIndex].GridCellType == GridCellTypes.Empty)
                    {
                        CreateSoldierOnMap(currentXIndex,currentYIndex);
                        return;
                    }
                }
            }
        }

        private void CreateSoldierOnMap(int xIndex, int yIndex)
        {
            _gridCellArray[xIndex, yIndex].GridCellType = GridCellTypes.Soldier;
            MapView exMapView = Object.FindObjectOfType<MapView>();
            GameObject Soldier = exMapView.CreateSoldierObject(xIndex, yIndex);
            soldiersList.Add(Soldier);
        }

        private void DetermineTheSoldierIndex(int startXIndex, int startYIndex)
        {

            for (int i = 0; i < soldiersList.Count; i++)
            {
                if (_gridCellGameObjectArray[startXIndex, startYIndex].transform.localPosition ==
                    soldiersList[i].transform.localPosition)
                {
                    _soldierIndexInList = i;
                }
            }
        }

        public void Move()
        {

            if (_soldierXIndex != null && _soldierYIndex != null && _moveFinishXIndex != null &&
                _moveFinishYIndex != null)
            {
                soldiersList[(int) _soldierIndexInList].transform.localPosition = new Vector2(soldiersList[(int)_soldierIndexInList].transform.localPosition.x, soldiersList[(int)_soldierIndexInList].transform.localPosition.y - 200*Time.deltaTime);
                if (soldiersList[(int) _soldierIndexInList].transform.localPosition.y <
                    _gridCellGameObjectArray[(int) _moveFinishXIndex, (int) _moveFinishYIndex].transform.localPosition
                        .y)
                {
                    soldiersList[(int) _soldierIndexInList].transform.localPosition =
                        _gridCellGameObjectArray[(int) _moveFinishXIndex, (int) _moveFinishYIndex].transform
                            .localPosition;
                    _gridCellArray[(int)this._soldierXIndex, (int) _soldierYIndex].GridCellType = GridCellTypes.Empty;
                    _gridCellArray[(int) _moveFinishXIndex, (int) _moveFinishYIndex].GridCellType =
                        GridCellTypes.Soldier;
                    _soldierXIndex = _moveFinishXIndex;
                    _soldierYIndex = _moveFinishYIndex;
                    _moveFinishXIndex = null;

                }
            }
        }
    }
}
