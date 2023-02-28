using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roots.Floor
{
    public class NormalFloor : BaseFloor
    {
        #region 字段


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

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.CompareTag("Player"))
            {
                var coll = collision.gameObject.GetComponent<Rigidbody2D>();
                coll.velocity = new Vector2(coll.velocity.x, this.rb.velocity.y);
                UpdateFloor();
            }
        }

        #endregion
    }
}