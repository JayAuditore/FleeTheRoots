using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Roots.SceneController;
using Roots.Player;

namespace Roots.UI
{
    public class PausePanel : BasePanel
    {
        #region 字段

        private CanvasGroup canvasGroup;

        public PlayerPropertySO PlayerProperty;

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
            UIManager.Instance.Clear();
            SceneManager.Instance.LoadScene(0, null, null);
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
            PlayerProperty.IsPause = false;
            UIManager.Instance.PushObject("PausePanel", this.gameObject);
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