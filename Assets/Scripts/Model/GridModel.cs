using UnityEngine;

namespace Assets.Scripts.Model
{
    public class GridModel : IGridCellModel
    {
        private GridCellTypes _gridCellType;
        public GridCellTypes GridCellType
        {
            get { return _gridCellType; }
            set { _gridCellType = value; }
        }
    }
}
