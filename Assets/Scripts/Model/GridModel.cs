using UnityEngine;

namespace Assets.Scripts.Model
{
    public class GridModel : IGridCellModel
    {
        public GridCellTypes GridCellType { get; set; }
        public int XIndex { get; set; }
        public int YIndex { get; set; }
    }
}
