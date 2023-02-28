using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Roots.SceneController;

namespace Roots.UI
{
    public class MainPanel : BasePanel
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

        public void OnStartClick()
        {
            UIManager.Instance.Clear();
            SceneManager.Instance.LoadScene(1, null, null);
        }

        public void OnSettingsClick()
        {
            UIManager.Instance.PushPanel(PanelType.SETTINGSPANEL);
        }

        public void OnExitClick()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        public override void OnEnter()
        {
        }

        public override void OnExit()
        {
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