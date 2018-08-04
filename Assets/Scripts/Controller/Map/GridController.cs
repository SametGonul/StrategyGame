using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Model;
using Assets.Scripts.StrategyGame.conf;
using UnityEngine;

namespace Assets.Scripts.Controller.Map
{
    /// <summary>
    /// This class for grid on the map.
    /// </summary>
    public class GridController {

        private static GridController _instance = null;

        private readonly GridCellModel[,] _gridCellCellArray = new GridCellModel[Config.VerticalGridNumber,Config.HorizontalGridNumber];

        // In the constructor create grid which includes grid cells.(static 28x24)
        private GridController()
        {
            FillCell();
        }

        public static GridController Instance()
        {
            return _instance ?? (_instance = new GridController());
        }

        /// <summary>
        /// This fills cells to grid.
        /// </summary>
        private void FillCell()
        {
            for (int i = 0; (i) < Config.VerticalGridNumber; i++)
            {
                for (int j = 0; j < Config.HorizontalGridNumber; j++)
                {
                    GridCellModel gridCellCell = new GridCellModel();
                    _gridCellCellArray[i, j] = gridCellCell;
                    _gridCellCellArray[i, j].XIndex = i;
                    _gridCellCellArray[i, j].YIndex = j;
                }             
            }       
        }

        public GridCellModel[,] GetGridCellArray()
        {
            return _gridCellCellArray;
        }
    }
}
