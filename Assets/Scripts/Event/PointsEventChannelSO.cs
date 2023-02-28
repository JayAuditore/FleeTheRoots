using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roots.Event
{
    [CreateAssetMenu(menuName = "Events/Points Event Channel")]
    public class PointsEventChannelSO : ScriptableObject
    {
        #region 字段

        public Action OnPointsRequested;
        public Action OnFloorRequested;

        #endregion

        #region 方法

        public void RaisePointsEvent()
        {
            if (OnPointsRequested != null)
            {
                OnPointsRequested.Invoke();
            }
        }

        public void RaiseFloorEvent()
        {
            if (OnFloorRequested != null)
            {
                OnFloorRequested.Invoke();
            }
        }

        #endregion
    }
}