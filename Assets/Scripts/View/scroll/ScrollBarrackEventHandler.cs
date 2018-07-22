using UnityEngine;

namespace Assets.Scripts.View.scroll
{
    /// <summary>
    /// this class' purpose is to check events start and continuous from barrack.
    /// </summary>
 
    public class ScrollBarrackEventHandler : MonoBehaviour {

        // Use this for initialization
        void Start () {
		
        }
	
        // Update is called once per frame
        void Update () {
		
        }
        // TO DO: this function will detect drag will be started in barrack.
        public void BarrackDragStart()
        {
            //Debug.Log("Barrack drag start.");
        }

        // TO DO: this function will detect if drag continuous on map and help to build barrack on map.
        public void BarrackDragOn()
        {
            //Debug.Log("Barrack dragging on.");
        }
    }
}
