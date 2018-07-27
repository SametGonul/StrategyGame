using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Model;
using Assets.Scripts.StrategyGame.conf;
using UnityEngine;

namespace Assets.Scripts.Controller.Map
{
    public class GridController {

        private static GridController _instance = null;
        private GridModel[,] GridCellArray = new GridModel[Config.VerticalGridNumber,Config.HorizontalGridNumber];
        private GridController()
        {
            FillCell();
        }

        public static GridController Instance()
        {
            return _instance ?? (_instance = new GridController());
        }


        private void FillCell()
        {
            for (int i = 0; (i) < Config.VerticalGridNumber; i++)
            {
                for (int j = 0; j < Config.HorizontalGridNumber; j++)
                {
                    GridModel GridCell = new GridModel();
                    GridCellArray[i, j] = GridCell;
                }             
            }       
        }

        public GridModel[,] GetGridCellArray()
        {
            return GridCellArray;
        }

        public void CheckEnums()
        {
            for (int i = 0; i < Config.VerticalGridNumber; i++)
            {
                for (int j = 0; j < Config.HorizontalGridNumber; j++)
                {
                    Debug.Log(GridCellArray[i,j].GridCellType);
                }
            }
        }
    }
}
