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

        //this function is called when create soldier button is clicked on information view. 
        public void CreateSoldierOnMap()
        {
            InfoController.Instance().CreateSoldierOnMap();
        }
    }
}
