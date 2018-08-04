using UnityEngine;

namespace Assets.Scripts.Model
{
    /// <summary>
    /// Grid cell model implemented from grid cell model interface.
    /// </summary>
    public class GridCellModel : IGridCellModel
    {
        public GridCellTypes GridCellType { get; set; }
        public int XIndex { get; set; }
        public int YIndex { get; set; }
    }
}
