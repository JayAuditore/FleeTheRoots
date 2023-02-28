using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Roots.Utility;

namespace Roots.SceneController
{
    public class SceneManager : BaseSingletonWithMono<SceneManager>
    {
        #region 字段

        [SerializeField] private int Index;
        private Action<float> onPageChange;
        private Action onFinish;

        #endregion

        #region Unity回调

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        #endregion

        #region 方法

        /// <summary>
        /// 加载场景
        /// </summary>
        /// <param name="index">要加载的场景索引号</param>
        /// <param name="onpagechange">跳转场景时候的回调函数，如果没有就null</param>
        /// <param name="onfinish">跳转结束之后的回调，如果没有就null</param>
        public void LoadScene(int index, Action<float> onpagechange, Action onfinish)
        {
            this.Index = index;
            this.onPageChange = onpagechange;
            this.onFinish = onfinish;

            //开启协程
            StartCoroutine(LoadScenes());
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <returns></returns>
        private IEnumerator LoadScenes()
        {
            yield return 0;
            //异步加载场景
            AsyncOperation asyncOperation = UnityEngine.SceneManagement.SceneManager.LoadSceneAsync(Index);

            while (!asyncOperation.isDone)
            {
                yield return 0;
                //传递异步加载进度，如果完成，progress=1
                onPageChange?.Invoke(asyncOperation.progress);
            }

            yield return new WaitForSeconds(2f);
            //在完成加载之后的操作
            onFinish?.Invoke();
            yield break;
        }

        #endregion
    }
}