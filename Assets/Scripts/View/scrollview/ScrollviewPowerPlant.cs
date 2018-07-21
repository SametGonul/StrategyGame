using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.Models;
using Assets.Scripts.StrategyGame.conf;
using UnityEngine;

namespace Assets.Scripts.View.scrollview
{
    public class ScrollviewPowerPlant : MonoBehaviour,IBuilding
    {
        public string Name { get; private set; }
        public int VerticalSize { get; private set; }
        public int HorizontalSize { get; private set; }

        public int Speed;

        // Use this for initialization
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
            transform.localPosition = new Vector2(transform.localPosition.x, transform.localPosition.y + Time.deltaTime * Speed);
            if (transform.localPosition.y < Config.ScrollviewMinYValue)
            {
                transform.localPosition = new Vector2(transform.localPosition.x, Config.ScrollviewMaxYValue);
            }
            else if (transform.localPosition.y > Config.ScrollviewMaxYValue)
            {
                transform.localPosition = new Vector2(transform.localPosition.x, Config.ScrollviewMinYValue);
            }
        }
    }


}
