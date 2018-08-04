using UnityEngine;

namespace Assets.Scripts.Model
{
    /// <summary>
    /// Grid cell model interface class
    /// Every grid cell can 4 different types.(empty,barrack,powerplant,soldier)
    /// </summary>
    public enum GridCellTypes
    {
        Empty,Barrack,PowerPlant,Soldier
    }
    public interface IGridCellModel {

        GridCellTypes GridCellType { get; set; }
        int XIndex { get; set; }
        int YIndex { get; set; }
    }
}
