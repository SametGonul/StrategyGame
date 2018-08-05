using System.Collections.Generic;
using Assets.Scripts.Controller.Map;
using Assets.Scripts.Model;
using Assets.Scripts.StrategyGame.conf;
using UnityEngine;

namespace Assets.Scripts.View.map
{
    /// <summary>
    /// This class is component of the map gameobject in unity.
    /// </summary>
    public class MapView : MonoBehaviour
    {

        // Grid prefab for every grid.
        public GameObject GridPrefab;
        public GameObject SoldierPrefab;

        private GameObject[,] _gridCellGameObjectsArray;
        // Use this for initialization
        void Start () {
            _gridCellGameObjectsArray = new GameObject[Config.VerticalGridNumber, Config.HorizontalGridNumber];
        }
	
        // Update is called once per frame
        void Update () {
		    MapController.Instance().FindTheCell();
        }

        // This function locates gameobjects on gridcells and return gameobject 2d array to use gameobject colors.
        public GameObject[,] LocateGameObjectsOnGridCells()
        {
             
            float startingX = Config.FirstGridStartingXCoordinate;
            float startingY = Config.FirstGridStartingYCoordinate;

            for (int i = 0; i < Config.VerticalGridNumber; i++)
            {
                for (int j = 0; j < Config.HorizontalGridNumber; j++)
                {
                    GameObject newObject = Instantiate(GridPrefab, transform);
                    newObject.transform.localPosition = new Vector2(startingX, startingY);
                    startingX += Config.GridSize;
                    _gridCellGameObjectsArray[i, j] =  newObject;
                }
                startingX = Config.FirstGridStartingXCoordinate;
                startingY -= Config.GridSize;
            }

            return _gridCellGameObjectsArray;
        }

    }

 
}
