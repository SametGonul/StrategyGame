using System.Collections.Generic;
using System.Linq;
using Assets.Scripts.Model;
using Assets.Scripts.StrategyGame.conf;
using Assets.Scripts.View.map;
using UnityEngine;

namespace Assets.Scripts.Controller.Map
{
    public class ShortestPath {

        private readonly CellModelForAStar[,] _cellModelAStarArray = new CellModelForAStar[Config.VerticalGridNumber, Config.HorizontalGridNumber];

        // AStar algorithm search function and get shortest path for the soldier with given Target point
        public List<GridCellModel> GetShortestPathWithAStarAlgorithm(GameObject soldier, int TargetXIndex, int TargetYIndex)
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
                    successor.DistanceToStart = CalculateDistanceValue(startCell.XIndex, startCell.YIndex, successor.XIndex, successor.YIndex);
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
                xDiff = (double)(targetXIndex - firstXIndex);
            }
            else
            {
                xDiff = (double)(firstXIndex - targetXIndex);
            }

            double yDiff;
            if (targetYIndex >= firstYIndex)
            {
                yDiff = (double)(targetYIndex - firstYIndex);

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
                if (MapController.Instance().GetGridCellArray()[current.XIndex + 1, current.YIndex].GridCellType == GridCellTypes.Empty)
                {
                    successors.Add(_cellModelAStarArray[current.XIndex + 1, current.YIndex]);
                }
            }
            if (current.XIndex - 1 >= 0)
            {

                if (MapController.Instance().GetGridCellArray()[current.XIndex - 1, current.YIndex].GridCellType == GridCellTypes.Empty)
                {
                    successors.Add(_cellModelAStarArray[current.XIndex - 1, current.YIndex]);
                }
            }
            if (current.YIndex + 1 < Config.HorizontalGridNumber)
            {
                if (MapController.Instance().GetGridCellArray()[current.XIndex, current.YIndex + 1].GridCellType == GridCellTypes.Empty)
                {
                    successors.Add(_cellModelAStarArray[current.XIndex, current.YIndex + 1]);
                }
            }
            if (current.YIndex - 1 >= 0)
            {
                if (MapController.Instance().GetGridCellArray()[current.XIndex, current.YIndex - 1].GridCellType == GridCellTypes.Empty)
                {
                    successors.Add(_cellModelAStarArray[current.XIndex, current.YIndex - 1]);
                }
            }

            return successors;
        }

    }
}
