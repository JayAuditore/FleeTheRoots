using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roots.UI
{
    public abstract class BasePanel : MonoBehaviour
    {
        /// <summary>
        /// 打开面板时调用
        /// </summary>
        public abstract void OnEnter();

        /// <summary>
        /// 当面板不在栈顶时调用
        /// </summary>
        public abstract void OnPause();

        /// <summary>
        /// 当面板再次位于栈顶时调用
        /// </summary>
        public abstract void OnResume();

        /// <summary>
        /// 关闭面板时调用
        /// </summary>
        public abstract void OnExit();
    }
}