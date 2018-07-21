using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.WSA.Persistence;


namespace Assets.Scripts.Models
{

    /// <summary>
    /// It is an interface for buildings
    /// it includes name,vertical size,horizotnal size.
    /// </summary>
    public interface IBuilding
    {
        string Name { get; }
        int VerticalSize { get; }
        int HorizontalSize { get; }
        
    }
}

