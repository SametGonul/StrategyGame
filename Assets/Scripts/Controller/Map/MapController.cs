using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Assets.Scripts.Controller.Info;
using Assets.Scripts.Controller.Scroll;
using Assets.Scripts.Model;
using Assets.Scripts.StrategyGame.conf;
using Assets.Scripts.View.map;
using Unity.Jobs;
using UnityEditor;
using UnityEditor.Experimental.UIElements.GraphView;
using UnityEngine;
using UnityEngine.Experimental.UIElements;
using Image = UnityEngine.UI.Image;
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
        private bool _soldierChecker = false;
        private int _barrackCounter = 1;

        private int _powerPlantCounter = 1;
        private List<IScrollBuildingModel> barracks = new List<IScrollBuildingModel>();
        private List<IScrollBuildingModel> powerPlants = new List<IScrollBuildingModel>();
        private List<GameObject> soldiersList = new List<GameObject>();
        private bool _soldierClickedChecker = false;

        private List<GridModel> exPath = new List<GridModel>();
        CellModelForAStar[,] cellModelAStarArray = new CellModelForAStar[Config.VerticalGridNumber, Config.HorizontalGridNumber];

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
                CheckSoldierInBuildingArea(GridCellTypes.Barrack);
                if (!_barrackCollisionChecker && !_soldierChecker)
                {
                    IScrollBuildingModel barrack = factory.CreateScrollBuildingModel(Config.BarrackName);
                    if (_activeCellPositionX != null) barrack.XIndex = (int) _activeCellPositionX;
                    if (_activeCellPositionY != null) barrack.YIndex = (int)_activeCellPositionY;
                    barrack.BuildingNumber = _barrackCounter;
                    _barrackCounter++;
                    barracks.Add(barrack);
                                  
                }
                else
                {
                    _barrackCollisionChecker = false;
                }

                _soldierChecker = false;
            }
            
            else if (ScrollController.Instance().PowerPlantEventChecker)
            {        
                ScrollController.Instance().PowerPlantEventChecker = false;
                CheckCollidedPowerPlantsOnDragFinish();
                CheckSoldierInBuildingArea(GridCellTypes.PowerPlant);
                if (!_powerPlantCollisionChecker && !_soldierChecker)
                {
                    IScrollBuildingModel powerPlant = factory.CreateScrollBuildingModel(Config.PowerPlantName);
                    if (_activeCellPositionX != null) powerPlant.XIndex = (int)_activeCellPositionX;
                    if (_activeCellPositionY != null) powerPlant.YIndex = (int)_activeCellPositionY;
                    powerPlant.BuildingNumber = _powerPlantCounter;
                    _powerPlantCounter++;
                    powerPlants.Add(powerPlant);
                    CheckSoldierInBuildingArea(GridCellTypes.PowerPlant);
                }
                else
                {
                    _powerPlantCollisionChecker = false;
                }
                _soldierChecker = false;
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


        private void CheckSoldierInBuildingArea(GridCellTypes GridCellType)
        {
            int IndexSetterWithBuildingType;
            if (GridCellType == GridCellTypes.Barrack)
            {
                IndexSetterWithBuildingType = 1;
            }
            else
            {
                IndexSetterWithBuildingType = 0;

            }
            
            for (int i = (int)_activeCellPositionX - 1; i <= (int)_activeCellPositionX + IndexSetterWithBuildingType; i++)
            {
                for (int j = (int)_activeCellPositionY - 1; j <= (int)_activeCellPositionY + 1; j++)
                {
                    if (_gridCellArray[i, j].GridCellType == GridCellTypes.Soldier)
                        _soldierChecker = true;
                }
            }
            if (_soldierChecker)
            {
                for (int i = (int)_activeCellPositionX - 1; i <= (int)_activeCellPositionX + IndexSetterWithBuildingType; i++)
                {
                    for (int j = (int)_activeCellPositionY - 1; j <= (int)_activeCellPositionY + 1; j++)
                    {
                        if (_gridCellArray[i, j].GridCellType != GridCellTypes.Soldier)
                        {
                            _gridCellArray[i, j].GridCellType = GridCellTypes.Empty;
                            _gridCellGameObjectArray[i, j].GetComponent<Image>().color = Color.green;
                        }
                        else
                        {
                            _gridCellArray[i, j].GridCellType = GridCellTypes.Soldier;
                            _gridCellGameObjectArray[i, j].GetComponent<Image>().color = Color.green;
                        }
                    }
                }
            }
            
            else
            {
                for (int i = (int)_activeCellPositionX - 1; i <= (int)_activeCellPositionX + IndexSetterWithBuildingType; i++)
                {
                    for (int j = (int)_activeCellPositionY - 1; j <= (int)_activeCellPositionY + 1; j++)
                    {
                        if (_gridCellArray[i, j].GridCellType == GridCellTypes.Empty)
                            _gridCellArray[i, j].GridCellType = GridCellType;
                    }
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
                    foreach (var soldier in soldiersList)
                    {
                        if (soldier.transform.position ==
                            _gridCellGameObjectArray[clickedXIndex, clickedYIndex].transform.position)
                        {
                            soldier.GetComponent<SoldierView>().GetSoldierController().SetSoldierXIndex(clickedXIndex);
                            soldier.GetComponent<SoldierView>().GetSoldierController().SetSoldierYIndex(clickedYIndex);
                            soldier.GetComponent<SoldierView>().GetSoldierController().SetSoldierSelected(true);
                        }
                    }

                    _soldierClickedChecker = true;
                }
                else
                {
                    _soldierClickedChecker = false;
                }
            }

            if (mouseClickType == 1)
            {
                if (_gridCellArray[clickedXIndex, clickedYIndex].GridCellType == GridCellTypes.Empty &&
                    _soldierClickedChecker)
                {
                    foreach (var soldier in soldiersList)
                    {
                        if (soldier.GetComponent<SoldierView>().GetSoldierController().GetSoldierSelected() == true)
                        { 
                            if (soldier.GetComponent<SoldierView>().GetSoldierController().GetSoldierMoving() == false)
                            {
                                exPath = GetShortestPathWithAStarAlgorithm(soldier, (int)clickedXIndex, (int)clickedYIndex);
                                if (exPath == null)
                                {
                                    _soldierClickedChecker = false;
                                }
                            
                                else if (exPath.Count == 1)
                                {
                                    soldier.GetComponent<SoldierView>().GetSoldierController().SetSoldierMovingXIndex(exPath[0].XIndex);
                                    soldier.GetComponent<SoldierView>().GetSoldierController().SetSoldierMovingYIndex(exPath[0].YIndex);


                                }
                                else if (exPath.Count > 1)
                                {
                                    soldier.GetComponent<SoldierView>().GetSoldierController().SetSoldierMovingXIndex(exPath[1].XIndex);
                                    soldier.GetComponent<SoldierView>().GetSoldierController().SetSoldierMovingYIndex(exPath[1].YIndex);
                                }
                                
                            }
                        }
                    }
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
            GameObject soldier = Object.Instantiate(exMapView.SoldierPrefab, exMapView.transform);
            soldier.transform.localPosition = _gridCellGameObjectArray[xIndex, yIndex].transform.localPosition;
            soldiersList.Add(soldier);
        }

        public GameObject[,] GetObjectList()
        {
            return _gridCellGameObjectArray;
        }

        public List<GridModel> GetExamplePath()
        {
            return exPath;
        }

        public IGridCellModel[,] GetGridCellArray()
        {
            return _gridCellArray;
        }

        private List<GridModel> GetShortestPathWithAStarAlgorithm(GameObject soldier,int TargetXIndex,int TargetYIndex)
        {
            List<CellModelForAStar> openList = new List<CellModelForAStar>();
            List<CellModelForAStar> closedList = new List<CellModelForAStar>();
            List<GridModel> shortestPath = new List<GridModel>();
            

            CreateCellArrayForAStar();
            int startGridCellXIndex = (int)soldier.GetComponent<SoldierView>().GetSoldierController().GetSoldierXIndex();
            int startGridCellYIndex = (int)soldier.GetComponent<SoldierView>().GetSoldierController().GetSoldierYIndex();
            CellModelForAStar startCell = cellModelAStarArray[startGridCellXIndex, startGridCellYIndex];
            startCell.DistanceToEnd = CalculateHValue(startGridCellXIndex, startGridCellYIndex, TargetXIndex, TargetYIndex);
            startCell.DistanceToStart = 0;

            cellModelAStarArray[startCell.XIndex, startCell.YIndex].ParentXIndex = -1;
            cellModelAStarArray[startCell.XIndex, startCell.YIndex].ParentYIndex = -1;

            openList.Add(startCell);            
            openList.Remove(startCell);

            CellModelForAStar current = startCell;
            List<CellModelForAStar> successors = FindSuccessors(current);
            foreach (var successor in successors)
            {
                successor.DistanceToEnd = CalculateHValue(successor.XIndex, successor.YIndex, TargetXIndex, TargetYIndex);
                successor.DistanceToStart = CalculateHValue(startCell.XIndex, startCell.YIndex, successor.XIndex, successor.YIndex);
                successor.TotalDistance = successor.DistanceToEnd + successor.DistanceToStart;
                if (!closedList.Contains(successor))
                {
                    cellModelAStarArray[successor.XIndex, successor.YIndex].ParentXIndex = current.XIndex;
                    cellModelAStarArray[successor.XIndex, successor.YIndex].ParentYIndex = current.YIndex;
                    openList.Add(successor);
                }
            }
            closedList.Add(startCell);

            while (openList.Count > 0)
            {
                openList.OrderBy(o => o.DistanceToEnd).ToList();
                current = openList[0];
                if (current.XIndex == TargetXIndex && current.YIndex == TargetYIndex)
                {                   
                    break;
                }
                successors = FindSuccessors(current);
                foreach (var successor in successors)
                {
                   
                    successor.DistanceToEnd = CalculateHValue(successor.XIndex, successor.YIndex, TargetXIndex, TargetYIndex);
                    successor.DistanceToStart = CalculateHValue(startCell.XIndex, startCell.YIndex, successor.XIndex,successor.YIndex);
                    successor.TotalDistance = successor.DistanceToEnd + successor.DistanceToStart;
                    if (!closedList.Contains(successor) && !openList.Contains(successor))
                    {
                        cellModelAStarArray[successor.XIndex, successor.YIndex].ParentXIndex = current.XIndex;
                        cellModelAStarArray[successor.XIndex, successor.YIndex].ParentYIndex = current.YIndex;  
                        openList.Add(successor);
                    }
                }
                openList.Remove(current);
                closedList.Add(current);
            }

            while (cellModelAStarArray[current.XIndex, current.YIndex].ParentXIndex != -1 &&
                   cellModelAStarArray[current.XIndex, current.YIndex].ParentYIndex != -1)
            {
                GridModel currentGridModelPath = new GridModel();
                currentGridModelPath.XIndex = current.XIndex;
                currentGridModelPath.YIndex = current.YIndex;
                shortestPath.Add(currentGridModelPath);

                current = cellModelAStarArray[cellModelAStarArray[current.XIndex, current.YIndex].ParentXIndex,
                    cellModelAStarArray[current.XIndex, current.YIndex].ParentYIndex];
            }

            shortestPath.Reverse();
            if (shortestPath[shortestPath.Count - 1].XIndex != TargetXIndex &&
                shortestPath[shortestPath.Count - 1].YIndex != TargetYIndex)
            {
                return null;
            }
            return shortestPath;
        }

        private void CreateCellArrayForAStar()
        {
            for (int i = 0; i < Config.VerticalGridNumber; i++)
            {
                for (int j = 0; j < Config.HorizontalGridNumber; j++)
                {
                    CellModelForAStar cellModel = new CellModelForAStar();
                    cellModelAStarArray[i, j] = cellModel;
                    cellModelAStarArray[i, j].XIndex = i;
                    cellModelAStarArray[i, j].YIndex = j;
                }
            }
        }
        private double CalculateHValue(int firstXIndex, int firstYIndex, int targetXIndex, int targetYIndex)
        {


            double xDiff;
            if (targetXIndex >= firstXIndex)
            {
                xDiff = (double )(targetXIndex - firstXIndex);
            }
            else
            {
                xDiff = (double) (firstXIndex - targetXIndex);
            }

            double yDiff;
            if (targetYIndex >= firstYIndex)
            {
                yDiff = (double) (targetYIndex - firstYIndex);
                
            }
            else
            {
                yDiff = (double)(firstYIndex - targetYIndex);
            }

            return yDiff + xDiff;
        }

        private List<CellModelForAStar> FindSuccessors(CellModelForAStar current)
        {
            List<CellModelForAStar> successors = new List<CellModelForAStar>();
            if (current.XIndex + 1 < Config.VerticalGridNumber)
            {
                if (_gridCellArray[current.XIndex + 1,current.YIndex].GridCellType == GridCellTypes.Empty)
                {
                    successors.Add(cellModelAStarArray[current.XIndex + 1, current.YIndex]);
                }
            }
            if (current.XIndex - 1 >= 0)
            {
                
                if (_gridCellArray[current.XIndex - 1, current.YIndex].GridCellType == GridCellTypes.Empty)
                {
                    successors.Add(cellModelAStarArray[current.XIndex -1, current.YIndex]);
                }
            }
            if (current.YIndex + 1 < Config.HorizontalGridNumber)
            {
                if (_gridCellArray[current.XIndex,current.YIndex + 1].GridCellType == GridCellTypes.Empty)
                {
                    successors.Add(cellModelAStarArray[current.XIndex,current.YIndex + 1]);
                }
            }
            if (current.YIndex - 1 >= 0)
            {
                if (_gridCellArray[current.XIndex,current.YIndex - 1].GridCellType == GridCellTypes.Empty)
                {
                    successors.Add(cellModelAStarArray[current.XIndex, current.YIndex - 1]);
                }
            }

            return successors;
        }
    }
}
