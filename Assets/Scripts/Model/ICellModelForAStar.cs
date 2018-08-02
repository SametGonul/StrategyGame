using UnityEngine;

namespace Assets.Scripts.Model
{
    public interface ICellModelForAStar
    {
        int ParentXIndex { get; set; }
        int ParentYIndex { get; set; }
        double DistanceToEnd { get; set; }
        double DistanceToStart { get; set; }
        double TotalDistance { get; set; }
        int XIndex { get; set; }
        int YIndex { get; set; }
    }
}
