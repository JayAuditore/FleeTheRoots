using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Roots.Event;
using Roots.Player;

namespace Roots.Floor
{
    public class BaseFloor : MonoBehaviour
    {
        #region 字段 
        
        protected Rigidbody2D rb;
        protected BoxCollider2D coll;
        protected PointsEventChannelSO PointsEventChannel;
        protected int FlowUpSpeed = 1;
        protected bool isGot = false;

        public PlayerPropertySO PlayerProperty;

        #endregion

        #region 方法

        public void FlowUp()
        {
            rb.velocity = new Vector2(0f, FlowUpSpeed);
        }

        public void UpdateFloor()
        {
            if (!isGot)
            {
                ++PlayerProperty.Floor;
                isGot = true;
            }
        }

        #endregion
    }
}