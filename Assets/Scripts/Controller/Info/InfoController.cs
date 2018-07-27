using System;
using System.Net.Mime;
using Assets.Scripts.Model;
using Assets.Scripts.StrategyGame.conf;
using Assets.Scripts.View.info;
using UnityEditor.Build;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Controller.Info
{
    public class InfoController
    {
        private static InfoController _instance = null;

        private InformationView _informationView;
        private IScrollBuildingModel _currentBuildingOnInfo;
        private InfoController()
        {
            _informationView = Object.FindObjectOfType<InformationView>();
        }
        public static InfoController Instance()
        {
            return _instance ?? (_instance = new InfoController());
        }

        public void ShowInfoClickedObject(IScrollBuildingModel building)
        {

            _informationView.BuildingImage.enabled = true;

            _informationView.BuildingNameText.enabled = true;
            _informationView.BuildingNameText.text = building.Name + building.BuildingNumber;

            if (building.Name == Config.BarrackName)
            {
                _informationView.BuildingImage.color = Color.blue;

                _informationView.SoldierCreateButton.enabled = true;
                _informationView.SoldierCreateButton.interactable = true;
                _informationView.SoldierCreateButton.GetComponent<Image>().enabled = true;
                _informationView.SoldierCreateButton.GetComponentInChildren<Text>().enabled = true;
            }
            else if (building.Name == Config.PowerPlantName)
            {
                _informationView.BuildingImage.color = Color.yellow;

                if (_informationView.SoldierCreateButton.enabled)
                {
                    _informationView.SoldierCreateButton.enabled = false;
                    _informationView.SoldierCreateButton.interactable = false;
                    _informationView.SoldierCreateButton.GetComponent<Image>().enabled = false;
                    _informationView.SoldierCreateButton.GetComponentInChildren<Text>().enabled = false;
                }
            }
            _currentBuildingOnInfo = building;
        }


        public void CreateSoldierOnMap()
        {
            Map.MapController.Instance().FindSuitableSoldierPosition(_currentBuildingOnInfo);
        }
    }
}
