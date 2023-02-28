using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Roots.Utility;

namespace Roots.UI
{
    public enum PanelType
    {
        MAINPANEL,
        SETTINGSPANEL,
        SUMMARYPANEL,
        PAUSEPANEL
    }
    public class UIManager : BaseSingletonWithMono<UIManager>
    {
        #region 字段

        public GameObject CurrentPanel;
        public Transform CanvasTransform
        {
            get
            {
                if (canvasTransform == null)
                {
                    canvasTransform = GameObject.Find("Canvas").transform;
                }
                return canvasTransform;
            }
        }

        private Stack<BasePanel> panelStack;
        private Dictionary<PanelType, BasePanel> panelDict;
        private Dictionary<string, List<GameObject>> panelPool;
        [SerializeField] private Transform canvasTransform;

        #endregion

        #region Unity回调

        private void Awake()
        {
            panelDict = new Dictionary<PanelType, BasePanel>();
            panelPool = new Dictionary<string, List<GameObject>>();
            panelStack = new Stack<BasePanel>();
        }

        #endregion

        #region 方法

        /// <summary>
        /// 打开面板
        /// </summary>
        /// <param name="type">打开的是哪个面板</param>
        public void PushPanel(PanelType type)
        {
            if (panelStack == null)
            {
                panelStack = new Stack<BasePanel>();
            }
            if (panelStack.Count > 0)
            {
                BasePanel topPanel = panelStack.Peek();
                topPanel.OnPause();
            }
            BasePanel panel = GetUI(type);
            panelStack.Push(panel);
            panel.OnEnter();
        }

        /// <summary>
        /// 关闭面板
        /// </summary>
        public void PopPanel()
        {
            if (panelStack == null)
            {
                panelStack = new Stack<BasePanel>();
            }
            if (panelStack.Count <= 0)
            {
                return;
            }
            BasePanel topPanel = panelStack.Pop();
            topPanel.OnExit();
            if (panelStack.Count > 0)
            {
                BasePanel panel = panelStack.Peek();
                panel.OnResume();
            }
        }

        /// <summary>
        /// 把PanelType转换为String
        /// </summary>
        /// <param name="type">当前面板类型</param>
        /// <returns>对应的String</returns>
        private string GetPanelString(PanelType type)
        {
            switch (type)
            {
                case PanelType.MAINPANEL:
                    {
                        return "MainPanel";
                    }
                case PanelType.SUMMARYPANEL:
                    {
                        return "SummaryPanel";
                    }
                case PanelType.PAUSEPANEL:
                    {
                        return "PausePanel";
                    }
                case PanelType.SETTINGSPANEL:
                    {
                        return "SettingsPanel";
                    }
                default:
                    {
                        break;
                    }
            }
            return "\0";
        }

        /// <summary>
        /// 生成面板
        /// </summary>
        /// <param name="type">面板的类型</param>
        /// <returns>生成的面板</returns>
        private BasePanel GetUI(PanelType type)
        {
            if (panelDict == null)
            {
                panelDict = new Dictionary<PanelType, BasePanel>();
            }
            BasePanel panel;
            if (!panelDict.TryGetValue(type, out panel))
            {
                GameObject curPanel = GetObject(GetPanelString(type), CanvasTransform);
                curPanel.transform.SetParent(CanvasTransform);
                panel = curPanel.GetComponent<BasePanel>();
                panelDict.Add(type, panel);
            }
            else
            {
                panel = panelDict[type];
                GameObject curPanel = GetObject(GetPanelString(type), CanvasTransform);
            }
            return panel;
        }

        /// <summary>
        /// 生成函数
        /// </summary>
        /// <param name="name">面板名字</param>
        /// <param name="parentTransform">父节点</param>
        /// <param name="isInWorldSpace">是不是在world space生成</param>
        /// <returns>生成的面板</returns>
        public GameObject GetObject(string name, Transform parentTransform, bool isInWorldSpace = false)
        {
            GameObject obj = null;
            if (panelPool.ContainsKey(name) && panelPool[name].Count > 0)
            {
                obj = panelPool[name][0];
                panelPool[name].RemoveAt(0);
            }
            else
            {
                obj = Instantiate(Resources.Load<GameObject>(name), parentTransform, isInWorldSpace);
            }
            obj.SetActive(true);
            return obj;
        }

        /// <summary>
        /// 关闭面板
        /// </summary>
        /// <param name="name">面板名字</param>
        /// <param name="obj">放回内存池的面板</param>
        public void PushObject(string name, GameObject obj)
        {
            if (panelPool.ContainsKey(name))
            {
                panelPool[name].Add(obj);
            }
            else
            {
                panelPool.Add(name, new List<GameObject>() { obj });
            }
            obj.SetActive(false);
        }

        /// <summary>
        /// 换场景时清空内存池
        /// </summary>
        public void Clear()
        {
            panelPool.Clear();
            panelDict.Clear();
            panelStack.Clear();
        }

        #endregion
    }
}