using UnityEngine;

namespace Assets.Scripts.Model
{
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
