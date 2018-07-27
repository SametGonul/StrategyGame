using Assets.Scripts.Controller.Map;
using UnityEngine;
using UnityEngine.Timeline;

namespace Assets.Scripts.View
{
    public class GameManager : MonoBehaviour
    {

        public GameObject Prefab;
        // Use this for initialization
        void Start ()
        {
            MapController.Instance();
        }

        // Update is called once per frame
        void Update () {
		
        }
    }
}
