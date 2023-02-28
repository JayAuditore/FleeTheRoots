using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Roots.Event;
using Roots.Player;

namespace Roots.Enemy
{
    public class Thorn : MonoBehaviour
    {
        #region 字段

        private BoxCollider2D coll;
        private Animator anim;
        private GameObject player;
        private float timer;

        public EnemyEventChannelSO EnemyEventChannel;
        public PlayerPropertySO PlayerProperty;

        #endregion

        #region Unity回调

        private void Awake()
        {
            coll = GetComponent<BoxCollider2D>();
            anim = GetComponent<Animator>();
            player = GameObject.Find("Player");
        }

        private void Update()
        {
            timer += Time.deltaTime;
            if (timer >= 5f)
            {
                Attack();
            }
            if (!PlayerProperty.IsGameOver)
            {
                SynchHorizon();
            }
        }

        #endregion

        #region 方法

        public void Attack()
        {
            anim.SetBool("Start", true);
            anim.SetBool("Recover", false);
            timer = 0f;
        }

        public void SwitchToAttack()
        {
            coll.enabled = true;
            anim.SetBool("Ready", true);
            anim.SetBool("Start", false);
        }

        public void SwitchToRecovery()
        {
            coll.enabled = false;
            anim.SetBool("Done", true);
            anim.SetBool("Ready", false);
        }

        public void SwitchToIdle()
        {
            anim.SetBool("Recover", true);
            anim.SetBool("Done", false);
            timer = 0f;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            EnemyEventChannel.RaiseInjuredEvent();
        }

        public void SynchHorizon()
        {
            float x = player.transform.position.x;
            transform.position = new Vector3(x, transform.position.y, transform.position.z);
        }

        #endregion
    }
}