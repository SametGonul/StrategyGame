namespace Assets.Scripts.Model
{
    /// <summary>
    /// Cell model which is used in A* algorithm for every grid cell
    /// </summary>
    public class CellModelForAStar {
        public int ParentXIndex { get; set; }
        public int ParentYIndex { get; set; }
        public double DistanceToEnd { get; set; }
        public double DistanceToStart { get; set; }
        public double TotalDistance { get; set; }
        public int XIndex { get; set; }
        public int YIndex { get; set; }
    }
}
