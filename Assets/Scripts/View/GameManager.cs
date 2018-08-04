using Assets.Scripts.Controller.Map;
using UnityEngine;
using UnityEngine.Timeline;

namespace Assets.Scripts.View
{
    /// <summary>
    /// Main game manager class
    /// In the start of this function map controller is called.
    /// </summary>
    public class GameManager : MonoBehaviour
    {

        public GameObject Prefab;
        // Use this for initialization
        void Start ()
        {
            MapController.Instance();
        }

    }
}
