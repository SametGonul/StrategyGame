using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Persistence;


namespace Assets.Scripts.Models
{
    public interface IBuilding
    {
        string Name { get; }
        int VerticalSize { get; }
        int HorizontalSize { get; }
        
    }
}

