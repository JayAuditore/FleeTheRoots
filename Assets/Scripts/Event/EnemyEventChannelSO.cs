using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roots.Event
{
    [CreateAssetMenu(menuName = "Events/Enemy Event Channel")]
    public class EnemyEventChannelSO : ScriptableObject
    {
        #region 字段

        public Action OnInjuredRequested;

        #endregion

        #region 方法

        public void RaiseInjuredEvent()
        {
            if (OnInjuredRequested != null)
            {
                OnInjuredRequested.Invoke();
            }
        }

        #endregion
    }
}