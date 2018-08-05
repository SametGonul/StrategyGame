using System;
using System.Net.Mime;
using Assets.Scripts.Model;
using Assets.Scripts.StrategyGame.conf;
using Assets.Scripts.View.info;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace Assets.Scripts.Controller.Info
{   
    /// <summary>
    /// Info Controller singleton class. This class connects info model and info view.
    /// </summary>
    public class InfoController
    {
        private static InfoController _instance = null;

        private readonly InformationView _informationView;
        private IScrollBuildingModel _currentBuildingOnInfo;
        

        // Constructor for info controller
        private InfoController()
        {
            _informationView = Object.FindObjectOfType<InformationView>();
        }
        
        /// <summary>
        /// First time create singleton instance then reach it.
        /// </summary>
        /// <returns> signleton instance </returns>
        public static InfoController Instance()
        {
            return _instance ?? (_instance = new InfoController());
        }

        /// <summary>
        /// According to parameter name that shows information of building in the view
        /// And set buttons and images components.
        /// </summary>
        /// <param name="building"></param>
        public void ShowInfoClickedObject(IScrollBuildingModel building)
        {

            _informationView.BuildingImage.enabled = true;

            _informationView.BuildingNameText.enabled = true;
            _informationView.BuildingNameText.text = building.Name + building.BuildingNumber;
            
            // Comparison parameter building name with the static building names.
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


        /// <summary>
        /// when soldier create button is clicked this function is called.
        /// </summary>
        public void CreateSoldierOnMap()
        {
            Map.MapController.Instance().FindSuitableSoldierPosition(_currentBuildingOnInfo);
        }
    }
}
