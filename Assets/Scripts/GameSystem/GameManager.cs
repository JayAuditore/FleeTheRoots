using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Roots.Utility;
using Roots.Player;
using Roots.UI;

namespace Roots.GameSystem
{
    public class GameManager : BaseSingletonWithMono<GameManager>
    {
        #region 字段

        public PlayerPropertySO PlayerProperty;

        #endregion

        #region Unity回调

        private void Awake()
        {
            InitPlayerPropertySO();
        }

        private void Start()
        {
            DontDestroyOnLoad(this);
            UIManager.Instance.PushPanel(PanelType.MAINPANEL);
        }

        private void Update()
        {
            CheckHP();
        }

        #endregion

        #region 方法

        public void CheckHP()
        {
            if (PlayerProperty.HP < 0)
            {
                UIManager.Instance.PushPanel(PanelType.SUMMARYPANEL);
            }
        }

        private void InitPlayerPropertySO()
        {
            PlayerProperty.Mission = 0;
            PlayerProperty.Floor = 0;
            PlayerProperty.HP = 2;
            PlayerProperty.Points = 0;
            PlayerProperty.IsMagic = false;
            PlayerProperty.MoveInput = 0;
            PlayerProperty.IsPause = false;
            PlayerProperty.IsInvincible = false;
            PlayerProperty.InvincibleTime = 5;
            PlayerProperty.IsGameOver = false;
        }

        #endregion
    }
}