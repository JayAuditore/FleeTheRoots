using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roots.UI
{
    public class SettingsPanel : BasePanel
    {
        #region 字段

        private CanvasGroup canvasGroup;

        #endregion

        #region Unity回调

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
        }

        #endregion

        #region 方法

        public void OnBackClick()
        {
            UIManager.Instance.PopPanel();
        }

        public override void OnEnter()
        {
        }

        public override void OnExit()
        {
            UIManager.Instance.PushObject("SettingsPanel", this.gameObject);
        }

        public override void OnPause()
        {
            canvasGroup.blocksRaycasts = false;
        }

        public override void OnResume()
        {
            canvasGroup.blocksRaycasts = true;
        }

        #endregion
    }
}