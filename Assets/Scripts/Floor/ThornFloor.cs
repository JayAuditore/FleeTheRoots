using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Roots.Event;

namespace Roots.Floor
{
    public class ThornFloor : BaseFloor
    {
        #region 字段

        public EnemyEventChannelSO EnemyEventChannel;

        #endregion

        #region Unity回调

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            coll = GetComponent<BoxCollider2D>();
        }

        private void Update()
        {
            FlowUp();
            if (transform.position.y > 6)
            {
                FloorManager.Instance.ReturnFloor(gameObject, FloorType.THORN);
            }
        }

        #endregion

        #region 方法

        private void OnCollisionStay2D(Collision2D collision)
        {
            var coll = collision.gameObject.GetComponent<Rigidbody2D>();
            coll.velocity = new Vector2(coll.velocity.x, this.rb.velocity.y);
            UpdateFloor();
            if (!PlayerProperty.IsInvincible)
            {
                EnemyEventChannel.RaiseInjuredEvent();
            }
        }

        #endregion
    }
}