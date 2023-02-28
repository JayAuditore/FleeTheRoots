using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Roots.Player;
using Roots.SceneController;

namespace Roots.UI
{
    public class GameSummaryPanel : BasePanel
    {
        #region 字段

        private CanvasGroup canvasGroup;

        public Text Point;
        public Text Mission;
        public PlayerPropertySO PlayerProperty;

        #endregion

        #region Unity回调

        private void Awake()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            Point.text = PlayerProperty.Points.ToString();
            Mission.text = PlayerProperty.Mission.ToString();
        }

        #endregion


        #region 方法

        public void OnRestartClick()
        {
            SceneManager.Instance.LoadScene(1, null, null);
        }

        public void OnBackClick()
        {
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
            UIManager.Instance.PushObject("SummaryPanel", this.gameObject);
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