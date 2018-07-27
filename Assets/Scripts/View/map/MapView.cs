using System.Collections.Generic;
using Assets.Scripts.Controller.Map;
using Assets.Scripts.Model;
using Assets.Scripts.StrategyGame.conf;
using UnityEngine;

namespace Assets.Scripts.View.map
{
    public class MapView : MonoBehaviour
    {

        // Grid prefab for every grid.
        public GameObject GridPrefab; 

        // Use this for initialization
        void Start () {
		
        }
	
        // Update is called once per frame
        void Update () {
		    MapController.Instance().FindTheCell();
        }

        public GameObject[,] LocateGrids()
        {
            GameObject[,] _gridCellGameArray = new GameObject[Config.VerticalGridNumber,Config.HorizontalGridNumber];
            float StartingX = Config.FirstGridStartingXCoordinate;
            float StartingY = Config.FirstGridStartingYCoordinate;

            for (int i = 0; i < Config.VerticalGridNumber; i++)
            {
                for (int j = 0; j < Config.HorizontalGridNumber; j++)
                {
                    GameObject newObject = Instantiate(GridPrefab, transform);
                    newObject.transform.localPosition = new Vector2(StartingX, StartingY);
                    StartingX += Config.GridSize;

                    _gridCellGameArray[i, j] =  newObject;
                    //Debug.Log(MapController.Instance()._gridCellArray[i,j]);

                }
                StartingX = Config.FirstGridStartingXCoordinate;
                StartingY -= Config.GridSize;
            }

            return _gridCellGameArray;
        }

    }

 
}
