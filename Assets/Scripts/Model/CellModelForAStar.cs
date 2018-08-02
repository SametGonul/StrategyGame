﻿namespace Assets.Scripts.Model
{
    public class CellModelForAStar : ICellModelForAStar {
        public int ParentXIndex { get; set; }
        public int ParentYIndex { get; set; }
        public double DistanceToEnd { get; set; }
        public double DistanceToStart { get; set; }
        public double TotalDistance { get; set; }
        public int XIndex { get; set; }
        public int YIndex { get; set; }
    }
}
