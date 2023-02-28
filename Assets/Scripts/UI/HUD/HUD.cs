using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Roots.Utility;
using Roots.Player;

namespace Roots.UI.HUD
{
    public class HUD : BaseSingletonWithMono<HUD>
    {
        #region 字段

        public GameObject HPPanel;
        public Text Points;
        public Text Floor;
        public PlayerPropertySO PlayerProperty;

        #endregion

        #region Unity回调

        private void Update()
        {
            UpdatePointsAndFloor();
        }

        #endregion

        #region 方法

        public void UpdatePointsAndFloor()
        {
            Points.text = PlayerProperty.Points.ToString();
            Floor.text = PlayerProperty.Floor.ToString();
        }

        public void OnSettingsClick()
        {
            UIManager.Instance.PushPanel(PanelType.PAUSEPANEL);
        }

        #endregion
    }
}