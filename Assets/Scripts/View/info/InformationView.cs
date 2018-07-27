using Assets.Scripts.Controller.Info;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.View.info
{
    public class InformationView : MonoBehaviour
    {
        public Text BuildingNameText;
        public Image BuildingImage;
        public Button SoldierCreateButton;

        // Use this for initialization
        void Start () {
		
        }
	
        // Update is called once per frame
        void Update () {
		
        }

        public void CreateSoldierOnMap()
        {
            InfoController.Instance().CreateSoldierOnMap();
        }
    }
}
