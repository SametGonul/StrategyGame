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
        readonly GameObject[,] _gridCellGameObjectArray;
 
        private int? _activeCellPositionX = null;
        private int? _activeCellPositionY = null;

        private readonly GridCellModel[,] _gridCellCellArray;

        private bool _barrackCollisionChecker = false;
        private bool _powerPlantCollisionChecker = false;
        private bool _soldierChecker = false;
        
        // counting barracks and powerplants for their name.
        private int _barrackCounter = 1;
        private int _powerPlantCounter = 1;

        private readonly List<IScrollBuildingModel> _barracks = new List<IScrollBuildingModel>();
        private readonly List<IScrollBuildingModel> _powerPlants = new List<IScrollBuildingModel>();
        private readonly List<GameObject> _soldiersList = new List<GameObject>();
        private bool _soldierClickedChecker = false;

        private List<GridCellModel> _soldierPath = new List<GridCellModel>();
        private readonly CellModelForAStar[,] _cellModelAStarArray = new CellModelForAStar[Config.VerticalGridNumber, Config.HorizontalGridNumber];

        private MapController()
        {
            MapView exMapView = Object.FindObjectOfType<MapView>();
            _gridCellGameObjectArray = exMapView.LocateGameObjectsOnGridCells();
            _gridCellCellArray = GridController.Instance().GetGridCellArray();
        }

        public static MapController Instance()
        {
            return _instance ?? (_instance = new MapController());
        }

        /// <summary>
        /// this function is called when drag is finished on map.
        /// It checks barrack or powerplant dragged to map.
        /// </summary>
        public void DragFinished()
        {
            _dragEventFromScrollToMap = false;
            BuildingFactory factory = new ScrollBuildingModelFactory();
            if (ScrollController.Instance().ScrollDragEventChecker.BuildingEventType == BuildingEventTypes.Barrack)
            {
                
                ScrollController.Instance().ScrollDragEventChecker.BuildingEventType = BuildingEventTypes.None;
                CheckCollidedBarraksOnDragFinish();
                CheckSoldierInBuildingArea(GridCellTypes.Barrack);
                if (!_barrackCollisionChecker && !_soldierChecker)
                {
                    IScrollBuildingModel barrack = factory.CreateScrollBuildingModel(Config.BarrackName);
                    if (_activeCellPositionX != null) barrack.XIndex = (int) _activeCellPositionX;
                    if (_activeCellPositionY != null) barrack.YIndex = (int)_activeCellPositionY;
                    barrack.BuildingNumber = _barrackCounter;
                    _barrackCounter++;
                    _barracks.Add(barrack);
                                  
                }
                else
                {
                    _barrackCollisionChecker = false;
                }

                _soldierChecker = false;
            }
            
            else if (ScrollController.Instance().ScrollDragEventChecker.BuildingEventType == BuildingEventTypes.PowerPlant)
            {        
                ScrollController.Instance().ScrollDragEventChecker.BuildingEventType = BuildingEventTypes.None;
                CheckCollidedPowerPlantsOnDragFinish();
                CheckSoldierInBuildingArea(GridCellTypes.PowerPlant);
                if (!_powerPlantCollisionChecker && !_soldierChecker)
                {
                    IScrollBuildingModel powerPlant = factory.CreateScrollBuildingModel(Config.PowerPlantName);
                    if (_activeCellPositionX != null) powerPlant.XIndex = (int)_activeCellPositionX;
                    if (_activeCellPositionY != null) powerPlant.YIndex = (int)_activeCellPositionY;
                    powerPlant.BuildingNumber = _powerPlantCounter;
                    _powerPlantCounter++;
                    _powerPlants.Add(powerPlant);
                    CheckSoldierInBuildingArea(GridCellTypes.PowerPlant);
                }
                else
                {
                    _powerPlantCollisionChecker = false;
                }
                _soldierChecker = false;
            }
            PreventCollision();
            _activeCellPositionY = null;
            _activeCellPositionX = null;     
        }

        // This function called when drag finished, when new building dragged on building on map.
        private void PreventCollision()
        {
            for (int i = 0; i < Config.VerticalGridNumber; i++)
            {
                for (int j = 0; j < Config.HorizontalGridNumber; j++)
                {
                    if (_gridCellCellArray[i, j].GridCellType == GridCellTypes.Barrack &&
                        _gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color == Color.green)
                    {
                        _gridCellCellArray[i, j].GridCellType = GridCellTypes.Empty;
                    }

                    else if (_gridCellCellArray[i, j].GridCellType == GridCellTypes.PowerPlant &&
                             _gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color == Color.green)
                    {
                        _gridCellCellArray[i, j].GridCellType = GridCellTypes.Empty;
                    }
                }
            }
        }

        // This function is called enter event of mouse on map.
        public void MouseOnMap()
        {
            if(ScrollController.Instance().ScrollDragEventChecker.BuildingEventType == BuildingEventTypes.Barrack || ScrollController.Instance().ScrollDragEventChecker.BuildingEventType == BuildingEventTypes.PowerPlant)
                _dragEventFromScrollToMap = true;
        }

        // This function is called exit event of mouse on map.
        public void MouseOutsideMap()
        {
            _dragEventFromScrollToMap = false;
        }

        public bool GetMouseEventChecker()
        {
            return _dragEventFromScrollToMap;
        }

        // Find grid cell xIndex and yIndex according to mouse position
        // Mouse position is same for every screen resolution.
        public void FindTheCell()
        {

            if (_dragEventFromScrollToMap)
            {
                float mouseRatioX = Input.mousePosition.x / Screen.width;
                float mouseRatioY = Input.mousePosition.y / Screen.height;
             
                // 0.5values sets canvas (0,0)point to middle of the screen.
                Vector2 mousePositionRatio = new Vector2(mouseRatioX - 0.5f,mouseRatioY - 0.5f);
                Canvas tempCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();

                Vector2 worldMousePosition = new Vector2(mousePositionRatio.x * tempCanvas.GetComponent<RectTransform>().sizeDelta.x, mousePositionRatio.y * tempCanvas.GetComponent<RectTransform>().sizeDelta.y);

                float yIndex = (worldMousePosition.x - Config.TheMostXCoordinate)/Config.GridSize;
                float xIndex = (Config.TheMostYCoordinate - worldMousePosition.y)/ Config.GridSize;

                int roundedXIndex = (int)Math.Floor(xIndex);
                int roundedYIndex = (int)Math.Floor(yIndex);

                if (_activeCellPositionX != roundedXIndex || _activeCellPositionY != roundedYIndex )
                {
                    if (_activeCellPositionX != null && _activeCellPositionY != null)
                    {
                        if(ScrollController.Instance().ScrollDragEventChecker.BuildingEventType == BuildingEventTypes.Barrack)
                            ColorTheBarrackOnMap((int)_activeCellPositionX, (int)_activeCellPositionY, Color.green);
                        else if (ScrollController.Instance().ScrollDragEventChecker.BuildingEventType == BuildingEventTypes.PowerPlant)
                            ColorThePowerPlantOnMap((int) _activeCellPositionX, (int) _activeCellPositionY, Color.green);
                    }
                    
                    _activeCellPositionX = roundedXIndex;
                    _activeCellPositionY = roundedYIndex;

                    if (ScrollController.Instance().ScrollDragEventChecker.BuildingEventType == BuildingEventTypes.Barrack)
                    {
                        CheckCollidedBuildingCells();
                        if (_activeCellPositionX != null && _activeCellPositionY != null)
                            ColorTheBarrackOnMap((int) _activeCellPositionX, (int) _activeCellPositionY, Color.blue);
                    }                        
                    else if (ScrollController.Instance().ScrollDragEventChecker.BuildingEventType == BuildingEventTypes.PowerPlant)
                    {
                        CheckCollidedBuildingCells();
                        if (_activeCellPositionX != null && _activeCellPositionY != null)
                            ColorThePowerPlantOnMap((int)_activeCellPositionX, (int)_activeCellPositionY, Color.yellow);
                    }                     
                }        
            }
        }

        // this function is called while barrack draggingg on map
        // color the map 3x3 blue color
        // if there is a collision with other buildings it colors grid red
        private void ColorTheBarrackOnMap(int xIndex,int yIndex,Color color)
        {
            if (xIndex > 0 && xIndex < Config.VerticalGridNumber - 1 && yIndex > 0 && yIndex < Config.HorizontalGridNumber - 1)
            {
                for (int i = xIndex-1; i <= xIndex+1; i++)
                {
                    for (int j = yIndex - 1; j <= yIndex + 1; j++)
                    {
                        if(_gridCellCellArray[i,j].GridCellType == GridCellTypes.Empty)
                            _gridCellGameObjectArray[i,j].GetComponent<Image>().color = color;
                        else if (_gridCellCellArray[i,j].GridCellType == GridCellTypes.Barrack || _gridCellCellArray[i,j].GridCellType == GridCellTypes.PowerPlant)
                        {
                            _gridCellGameObjectArray[i, j].GetComponent<Image>().color = Color.red;
                        }
                       
                    }
                }
            }
        }

        // this function is called while powerplant draggingg on map
        // color the map 2x3 yelllow color
        // if there is a collision with other buildings it colors grid red
        private void ColorThePowerPlantOnMap(int xIndex, int yIndex, Color color)
        {
            if (xIndex > 0 && xIndex < Config.VerticalGridNumber && yIndex > 0 && yIndex < Config.HorizontalGridNumber - 1)
            {
                for (int i = xIndex - 1; i <= xIndex ; i++)
                {
                    for (int j = yIndex - 1; j <= yIndex + 1 ; j++)
                    {
                        if (_gridCellCellArray[i, j].GridCellType == GridCellTypes.Empty)
                            _gridCellGameObjectArray[i, j].GetComponent<Image>().color = color;
                        else if (_gridCellCellArray[i, j].GridCellType == GridCellTypes.Barrack || _gridCellCellArray[i, j].GridCellType == GridCellTypes.PowerPlant)
                        {
                            _gridCellGameObjectArray[i, j].GetComponent<Image>().color = Color.red;
                        }
                    }
                }
            }
        }

        // On drag finish it checks if soldier is in building area.
        // Set the cell colors on grid according to collision.  
        private void CheckSoldierInBuildingArea(GridCellTypes gridCellType)
        {
            var indexSetterWithBuildingType = gridCellType == GridCellTypes.Barrack ? 1 : 0;

            if (_activeCellPositionX != null && _activeCellPositionY != null)
            {
                for (int i = (int) _activeCellPositionX - 1;
                    i <= (int) _activeCellPositionX + indexSetterWithBuildingType;
                    i++)
                {
                    for (int j = (int) _activeCellPositionY - 1; j <= (int) _activeCellPositionY + 1; j++)
                    {
                        if (_gridCellCellArray[i, j].GridCellType == GridCellTypes.Soldier)
                            _soldierChecker = true;
                    }
                }

                if (_soldierChecker)
                {
                    for (int i = (int) _activeCellPositionX - 1;
                        i <= (int) _activeCellPositionX + indexSetterWithBuildingType;
                        i++)
                    {
                        for (int j = (int) _activeCellPositionY - 1; j <= (int) _activeCellPositionY + 1; j++)
                        {
                            if (_gridCellCellArray[i, j].GridCellType != GridCellTypes.Soldier)
                            {
                                _gridCellCellArray[i, j].GridCellType = GridCellTypes.Empty;
                                _gridCellGameObjectArray[i, j].GetComponent<Image>().color = Color.green;
                            }
                            else
                            {
                                _gridCellCellArray[i, j].GridCellType = GridCellTypes.Soldier;
                                _gridCellGameObjectArray[i, j].GetComponent<Image>().color = Color.green;
                            }
                        }
                    }
                }

                else
                {
                    for (int i = (int) _activeCellPositionX - 1;
                        i <= (int) _activeCellPositionX + indexSetterWithBuildingType;
                        i++)
                    {
                        for (int j = (int) _activeCellPositionY - 1; j <= (int) _activeCellPositionY + 1; j++)
                        {
                            if (_gridCellCellArray[i, j].GridCellType == GridCellTypes.Empty)
                                _gridCellCellArray[i, j].GridCellType = gridCellType;
                        }
                    }
                }
            }
        }
        
        // Sets collided barrack and powerplant cells colors original from collision color red.
        private void CheckCollidedBuildingCells()
        {
            for (int i = 0; i < Config.VerticalGridNumber; i++)
            {
                for (int j = 0; j < Config.HorizontalGridNumber; j++)
                {
                    if (_gridCellCellArray[i, j].GridCellType == GridCellTypes.Barrack &&
                        _gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color == Color.red)
                    {
                        _gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color = Color.blue;
                       
                    }
                    else if (_gridCellCellArray[i, j].GridCellType == GridCellTypes.PowerPlant &&
                             _gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color == Color.red)
                    {
                        _gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color = Color.yellow;
                    }
                }
            }
        }

        // On drag finish for barracks, this checks it crashed with other buildings
        // set the color of cells after collision on drag finish
        private void CheckCollidedBarraksOnDragFinish()
        {
            
            for (int i = (int) _activeCellPositionX - 1; i <= (int) _activeCellPositionX + 1; i++)
            {
                for (int j = (int) _activeCellPositionY - 1; j <= (int) _activeCellPositionY + 1; j++)
                {
                    if (_gridCellCellArray[i, j].GridCellType == GridCellTypes.Barrack &&
                        _gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color == Color.red)
                    {
                        _barrackCollisionChecker = true;
                    }
                    else if (_gridCellCellArray[i, j].GridCellType == GridCellTypes.PowerPlant &&
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
                            _gridCellCellArray[i, j].GridCellType = GridCellTypes.Empty;
                        }
                        if (_gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color == Color.red)
                        {
                            if(_gridCellCellArray[i,j].GridCellType == GridCellTypes.Barrack)
                                _gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color = Color.blue;
                            else if(_gridCellCellArray[i,j].GridCellType == GridCellTypes.PowerPlant)
                                _gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color = Color.yellow;
                        }

                        if (_gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color == Color.green)
                        {
                            _gridCellCellArray[i, j].GridCellType = GridCellTypes.Empty;    
                        }

                    }
                }
            }

        }

        // On drag finish for powerplants, this checks it crashed with other buildings
        // set the color of cells after collision on drag finish
        private void CheckCollidedPowerPlantsOnDragFinish()
        {
            
            for (int i = (int)_activeCellPositionX - 1; i <= (int)_activeCellPositionX ; i++)
            {
                for (int j = (int)_activeCellPositionY - 1; j <= (int)_activeCellPositionY + 1; j++)
                {
                    if (_gridCellCellArray[i, j].GridCellType == GridCellTypes.PowerPlant &&
                        _gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color == Color.red)
                    {
                        _powerPlantCollisionChecker = true;
                    }
                    else if (_gridCellCellArray[i, j].GridCellType == GridCellTypes.Barrack &&
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
                            _gridCellCellArray[i, j].GridCellType = GridCellTypes.Empty;
                        }
                    }
                }
                for (int i = (int)_activeCellPositionX - 1; i <= (int)_activeCellPositionX ; i++)
                {
                    for (int j = (int)_activeCellPositionY - 1; j <= (int)_activeCellPositionY + 1; j++)
                    {
                        if (_gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color == Color.red)
                        {
                            if (_gridCellCellArray[i, j].GridCellType == GridCellTypes.Barrack)
                                _gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color = Color.blue;
                            else if (_gridCellCellArray[i, j].GridCellType == GridCellTypes.PowerPlant)
                                _gridCellGameObjectArray[i, j].gameObject.GetComponent<Image>().color = Color.yellow;
                        }
                    }
                }
            }

        }

        // find gridcell xIndex,yIndex and checks clicked on gameobject or not.
        public void MouseClickOnMap(int mouseClickType)
        {
            float mouseRatioX = Input.mousePosition.x / Screen.width;
            float mouseRatioY = Input.mousePosition.y / Screen.height;

            // 0.5values sets canvas (0,0)point to middle of the screen.
            Vector2 mousePositionRatio = new Vector2(mouseRatioX - 0.5f, mouseRatioY - 0.5f);
            Canvas tempCanvas = GameObject.Find("Canvas").GetComponent<Canvas>();

            Vector2 worldMousePosition = new Vector2(mousePositionRatio.x * tempCanvas.GetComponent<RectTransform>().sizeDelta.x, mousePositionRatio.y * tempCanvas.GetComponent<RectTransform>().sizeDelta.y);

            float yIndex = (worldMousePosition.x - Config.TheMostXCoordinate) / Config.GridSize;
            float xIndex = (Config.TheMostYCoordinate - worldMousePosition.y) / Config.GridSize;

            int roundedXIndex = (int)Math.Floor(xIndex);
            int roundedYIndex = (int)Math.Floor(yIndex);
            CheckClickIsGameObject(roundedXIndex, roundedYIndex,mouseClickType);
        }

        // this function check mouse clicked on the building in map or 
        // it is clicked for soldier
        // when clicked left on soldier, it is selected, after this when click empty grid cell right click, soldier starts to move.
        private void CheckClickIsGameObject(int clickedXIndex, int clickedYIndex,int mouseClickType)
        {
            
            for (int i = 0; i < _barracks.Count; i++)
            {
                if (_barracks[i].XIndex - clickedXIndex >= -1 && _barracks[i].XIndex - clickedXIndex <= 1)
                {
                    if (_barracks[i].YIndex - clickedYIndex >= -1 && _barracks[i].YIndex - clickedYIndex <= 1)
                    {
                        InfoController.Instance().ShowInfoClickedObject(_barracks[i]);
                        break;
                    }
                }
            }
            for (int i = 0; i < _powerPlants.Count; i++)
            {
                if (_powerPlants[i].XIndex - clickedXIndex >= 0 && _powerPlants[i].XIndex - clickedXIndex <= 1)
                {
                    if (_powerPlants[i].YIndex - clickedYIndex >= -1 && _powerPlants[i].YIndex - clickedYIndex <= 1)
                    {
                        InfoController.Instance().ShowInfoClickedObject(_powerPlants[i]);
                        break;
                    }          
                }
            }

            if (mouseClickType == 0)
            {
                if (_gridCellCellArray[clickedXIndex, clickedYIndex].GridCellType == GridCellTypes.Soldier)
                {
                    foreach (var soldier in _soldiersList)
                    {
                        if (soldier.transform.position ==
                            _gridCellGameObjectArray[clickedXIndex, clickedYIndex].transform.position)
                        {
                            soldier.GetComponent<SoldierView>().GetSoldierController().SetSoldierXIndex(clickedXIndex);
                            soldier.GetComponent<SoldierView>().GetSoldierController().SetSoldierYIndex(clickedYIndex);
                            soldier.GetComponent<SoldierView>().GetSoldierController().SetSoldierSelected(true);
                        }
                        else
                        {
                            soldier.GetComponent<SoldierView>().GetSoldierController().SetSoldierSelected(false);
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
                if (_gridCellCellArray[clickedXIndex, clickedYIndex].GridCellType == GridCellTypes.Empty &&
                    _soldierClickedChecker)
                {
                    foreach (var soldier in _soldiersList)
                    {
                        if (soldier.GetComponent<SoldierView>().GetSoldierController().GetSoldierSelected() == true)
                        { 
                            if (soldier.GetComponent<SoldierView>().GetSoldierController().GetSoldierMoving() == false)
                            {
                                if (soldier.GetComponent<SoldierView>().GetSoldierController().GetSoldierHasPath() ==
                                    false)
                                {
                                    _soldierPath = GetShortestPathWithAStarAlgorithm(soldier, (int)clickedXIndex, (int)clickedYIndex);
                                    soldier.GetComponent<SoldierView>().GetSoldierController().SetSoldierPath(_soldierPath);
                                    soldier.GetComponent<SoldierView>().GetSoldierController().SetSoldierHasPath(true);

                                    if (_soldierPath == null)
                                    {
                                        soldier.GetComponent<SoldierView>().GetSoldierController().SetSoldierHasPath(false);
                                    }
                                    else if (_soldierPath.Count == 1)
                                    {
                                        soldier.GetComponent<SoldierView>().GetSoldierController().SetSoldierMovingXIndex(_soldierPath[0].XIndex);
                                        soldier.GetComponent<SoldierView>().GetSoldierController().SetSoldierMovingYIndex(_soldierPath[0].YIndex);


                                    }
                                    else if (_soldierPath.Count > 1)
                                    {
                                        soldier.GetComponent<SoldierView>().GetSoldierController().SetSoldierMovingXIndex(_soldierPath[1].XIndex);
                                        soldier.GetComponent<SoldierView>().GetSoldierController().SetSoldierMovingYIndex(_soldierPath[1].YIndex);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        // Finds suitable place for soldiers.
        // when soldier is created, it starts mid-down position of barrack and surround the barracks.
        public void FindSuitableSoldierPosition(IScrollBuildingModel selectedBuilding)
        {
            int currentYIndex = selectedBuilding.YIndex;
            int currentXIndex = selectedBuilding.XIndex + 2;
            for (; currentYIndex >= selectedBuilding.YIndex - 1; currentYIndex--)
            {
                if (currentXIndex <= Config.VerticalGridNumber - 1)
                {
                    if (_gridCellCellArray[currentXIndex, currentYIndex].GridCellType == GridCellTypes.Empty)
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
                        if (_gridCellCellArray[currentXIndex, currentYIndex].GridCellType == GridCellTypes.Empty)
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

                        if (_gridCellCellArray[currentXIndex, currentYIndex].GridCellType == GridCellTypes.Empty)
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
                    if (_gridCellCellArray[currentXIndex, currentYIndex].GridCellType == GridCellTypes.Empty)
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
                    if (_gridCellCellArray[currentXIndex, currentYIndex].GridCellType == GridCellTypes.Empty)
                    {
                        CreateSoldierOnMap(currentXIndex,currentYIndex);
                        return;
                    }
                }
            }
        }

        // Create soldier on suitable grid cell
        private void CreateSoldierOnMap(int xIndex, int yIndex)
        {
            _gridCellCellArray[xIndex, yIndex].GridCellType = GridCellTypes.Soldier;
            MapView exMapView = Object.FindObjectOfType<MapView>();
            GameObject soldier = Object.Instantiate(exMapView.SoldierPrefab, exMapView.transform);
            soldier.transform.localPosition = _gridCellGameObjectArray[xIndex, yIndex].transform.localPosition;
            _soldiersList.Add(soldier);
        }


        // AStar algorithm search function and get shortest path for the soldier with given Target point
        private List<GridCellModel> GetShortestPathWithAStarAlgorithm(GameObject soldier,int TargetXIndex,int TargetYIndex)
        {
            List<CellModelForAStar> openList = new List<CellModelForAStar>();
            List<CellModelForAStar> closedList = new List<CellModelForAStar>();
            List<GridCellModel> shortestPath = new List<GridCellModel>();
            

            CreateCellArrayForAStar();
            int startGridCellXIndex = (int)soldier.GetComponent<SoldierView>().GetSoldierController().GetSoldierXIndex();
            int startGridCellYIndex = (int)soldier.GetComponent<SoldierView>().GetSoldierController().GetSoldierYIndex();
            CellModelForAStar startCell = _cellModelAStarArray[startGridCellXIndex, startGridCellYIndex];
            startCell.DistanceToEnd = CalculateDistanceValue(startGridCellXIndex, startGridCellYIndex, TargetXIndex, TargetYIndex);
            startCell.DistanceToStart = 0;

            _cellModelAStarArray[startCell.XIndex, startCell.YIndex].ParentXIndex = -1;
            _cellModelAStarArray[startCell.XIndex, startCell.YIndex].ParentYIndex = -1;

            openList.Add(startCell);            
            openList.Remove(startCell);

            CellModelForAStar current = startCell;
            List<CellModelForAStar> successors = FindSuccessors(current);
            foreach (var successor in successors)
            {
                successor.DistanceToEnd = CalculateDistanceValue(successor.XIndex, successor.YIndex, TargetXIndex, TargetYIndex);
                successor.DistanceToStart = CalculateDistanceValue(startCell.XIndex, startCell.YIndex, successor.XIndex, successor.YIndex);
                successor.TotalDistance = successor.DistanceToEnd + successor.DistanceToStart;
                if (!closedList.Contains(successor))
                {
                    _cellModelAStarArray[successor.XIndex, successor.YIndex].ParentXIndex = current.XIndex;
                    _cellModelAStarArray[successor.XIndex, successor.YIndex].ParentYIndex = current.YIndex;
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
                   
                    successor.DistanceToEnd = CalculateDistanceValue(successor.XIndex, successor.YIndex, TargetXIndex, TargetYIndex);
                    successor.DistanceToStart = CalculateDistanceValue(startCell.XIndex, startCell.YIndex, successor.XIndex,successor.YIndex);
                    successor.TotalDistance = successor.DistanceToEnd + successor.DistanceToStart;
                    if (!closedList.Contains(successor) && !openList.Contains(successor))
                    {
                        _cellModelAStarArray[successor.XIndex, successor.YIndex].ParentXIndex = current.XIndex;
                        _cellModelAStarArray[successor.XIndex, successor.YIndex].ParentYIndex = current.YIndex;  
                        openList.Add(successor);
                    }
                }
                openList.Remove(current);
                closedList.Add(current);
            }

            while (_cellModelAStarArray[current.XIndex, current.YIndex].ParentXIndex != -1 &&
                   _cellModelAStarArray[current.XIndex, current.YIndex].ParentYIndex != -1)
            {
                GridCellModel currentGridCellModelPath = new GridCellModel();
                currentGridCellModelPath.XIndex = current.XIndex;
                currentGridCellModelPath.YIndex = current.YIndex;
                shortestPath.Add(currentGridCellModelPath);

                current = _cellModelAStarArray[_cellModelAStarArray[current.XIndex, current.YIndex].ParentXIndex,
                    _cellModelAStarArray[current.XIndex, current.YIndex].ParentYIndex];
            }

            shortestPath.Reverse();
            if (shortestPath[shortestPath.Count - 1].XIndex != TargetXIndex &&
                shortestPath[shortestPath.Count - 1].YIndex != TargetYIndex)
            {
                return null;
            }
            return shortestPath;
        }

        // Create AStar gridcell models for each grid cell
        private void CreateCellArrayForAStar()
        {
            for (int i = 0; i < Config.VerticalGridNumber; i++)
            {
                for (int j = 0; j < Config.HorizontalGridNumber; j++)
                {
                    CellModelForAStar cellModel = new CellModelForAStar();
                    _cellModelAStarArray[i, j] = cellModel;
                    _cellModelAStarArray[i, j].XIndex = i;
                    _cellModelAStarArray[i, j].YIndex = j;
                }
            }
        }

        // calculates distance between object and endpoint or startingpoint.
        private double CalculateDistanceValue(int firstXIndex, int firstYIndex, int targetXIndex, int targetYIndex)
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

        // Find grid cell successors if they are not building and soldier.
        private List<CellModelForAStar> FindSuccessors(CellModelForAStar current)
        {
            List<CellModelForAStar> successors = new List<CellModelForAStar>();
            if (current.XIndex + 1 < Config.VerticalGridNumber)
            {
                if (_gridCellCellArray[current.XIndex + 1,current.YIndex].GridCellType == GridCellTypes.Empty)
                {
                    successors.Add(_cellModelAStarArray[current.XIndex + 1, current.YIndex]);
                }
            }
            if (current.XIndex - 1 >= 0)
            {
                
                if (_gridCellCellArray[current.XIndex - 1, current.YIndex].GridCellType == GridCellTypes.Empty)
                {
                    successors.Add(_cellModelAStarArray[current.XIndex -1, current.YIndex]);
                }
            }
            if (current.YIndex + 1 < Config.HorizontalGridNumber)
            {
                if (_gridCellCellArray[current.XIndex,current.YIndex + 1].GridCellType == GridCellTypes.Empty)
                {
                    successors.Add(_cellModelAStarArray[current.XIndex,current.YIndex + 1]);
                }
            }
            if (current.YIndex - 1 >= 0)
            {
                if (_gridCellCellArray[current.XIndex,current.YIndex - 1].GridCellType == GridCellTypes.Empty)
                {
                    successors.Add(_cellModelAStarArray[current.XIndex, current.YIndex - 1]);
                }
            }

            return successors;
        }


        public GameObject[,] GetObjectList()
        {
            return _gridCellGameObjectArray;
        }

        public List<GridCellModel> GetExamplePath()
        {
            return _soldierPath;
        }

        public IGridCellModel[,] GetGridCellArray()
        {
            return _gridCellCellArray;
        }

    }
}
