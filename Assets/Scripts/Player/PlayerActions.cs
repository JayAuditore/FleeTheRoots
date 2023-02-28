using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Roots.Player.Input;
using Roots.UI;
using Roots.Event;
using Roots.UI.HUD;

namespace Roots.Player
{
    public class PlayerActions : MonoBehaviour
    {
        #region 字段

        private Vector2 inputVector;
        [SerializeField] private LayerMask mask;
        [SerializeField] private Transform headTransform;
        [SerializeField] private Transform footTransform;
        [SerializeField] private PlayerPropertySO PlayerProperty;
        [SerializeField] private EnemyEventChannelSO FloorAttackChannel;
        [SerializeField] private EnemyEventChannelSO ThornAttackChannel;
        [SerializeField] private PointsEventChannelSO PointsEventChannel;
        [SerializeField] private InputReader inputReader;
        private Rigidbody2D rb;
        private SpriteRenderer spriteRender;

        #endregion

        #region Unity回调

        private void OnEnable()
        {
            inputReader.JumpEvent += OnJumpInitiated;
            inputReader.JumpCanceledEvent += OnJumpCanceled;
            inputReader.LandEvent += OnLand;
            inputReader.PauseEvent += OpenMenu;
            inputReader.MoveEvent += OnMove;
        }

        private void Awake()
        {
            spriteRender = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
        }

        void Start()
        {
            InitPlayerPropertySO();
            FloorAttackChannel.OnInjuredRequested += GetInjured;
            ThornAttackChannel.OnInjuredRequested += GetInjured;
        }

        void Update()
        {
            if (!PlayerProperty.IsGameOver)
            {
                CheckOutOfEdge();
            }
        }

        private void FixedUpdate()
        {
            HorizonMove();
        }

        private void OnDisable()
        {
            inputReader.JumpEvent -= OnJumpInitiated;
            inputReader.JumpCanceledEvent -= OnJumpCanceled;
            inputReader.LandEvent -= OnLand;
            inputReader.PauseEvent -= OpenMenu;
        }

        #endregion

        #region 方法

        private void GetInjured()
        {
            var curHP = HUD.Instance.HPPanel.transform.GetChild(PlayerProperty.HP);
            curHP.gameObject.SetActive(false);
            --PlayerProperty.HP;
            int times = (int)(PlayerProperty.InvincibleTime / 0.2f);
            StartCoroutine(Invincible(times, 0.2f));
        }

        /// <summary>
        /// 闪烁+无敌
        /// </summary>
        /// <param name="numBlinks">闪烁的次数</param>
        /// <param name="seconds">每次闪烁的用时</param>
        /// <returns></returns>
        private IEnumerator Invincible(int numBlinks, float seconds)
        {
            PlayerProperty.IsInvincible = true;
            for (int i = 0; i < numBlinks * 2; i++)
            {
                spriteRender.enabled = !spriteRender.enabled;
                yield return new WaitForSeconds(seconds);
            }
            spriteRender.enabled = true;
            PlayerProperty.IsInvincible = false;
            yield break;
        }

        private void HorizonMove()
        {
            float speed = inputVector.x * PlayerProperty.WalkSpeed;
            rb.velocity = new Vector2(speed, rb.velocity.y);
            if (speed > 0)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = true;
            }
            else if (speed < 0)
            {
                gameObject.GetComponent<SpriteRenderer>().flipX = false;
            }
        }

        private void OnLand()
        {
            Physics2D.IgnoreLayerCollision(6, 7);
            StartCoroutine(RestartCollsion());
        }

        private IEnumerator RestartCollsion()
        {
            //如果 headTransform - footTransform <= -地板厚度，就恢复碰撞
            while (headTransform.position.y - footTransform.position.y > -3f)
            {
                yield return null;
            }
            Physics2D.IgnoreLayerCollision(6, 7, false);
            yield break;
        }

        private void OpenMenu()
        {
            UIManager.Instance.PushPanel(PanelType.PAUSEPANEL);
        }

        private void OnMove(Vector2 movement)
        {
            if (PlayerProperty.IsMagic)
            {
                inputVector = -movement;
            }
            else
            {
                inputVector = movement;
            }
        }

        private void OnJumpInitiated()
        {
            var hitInfo = Physics2D.OverlapCircle(footTransform.position, 0.1f, mask);
            if (PlayerProperty.JumpCount > 0 && hitInfo != null)
            {
                --PlayerProperty.JumpCount;
                rb.velocity = new Vector2(rb.velocity.x, PlayerProperty.JumpForce);
            }
        }

        private void OnJumpCanceled()
        {
            if (PlayerProperty.JumpCount == 0)
            {
                ++PlayerProperty.JumpCount;
            }
        }

        private void InitPlayerPropertySO()
        {
            PlayerProperty.JumpCount = 1;
            PlayerProperty.Mission = 0;
            PlayerProperty.Floor = 0;
            PlayerProperty.HP = 2;
            PlayerProperty.Points = 0;
            PlayerProperty.IsMagic = false;
            PlayerProperty.MoveInput = 0;
            PlayerProperty.IsPause = false;
            PlayerProperty.IsInvincible = false;
            PlayerProperty.InvincibleTime = 5;
            PlayerProperty.IsGameOver = false;
        }

        private void CheckOutOfEdge()
        {
            if (transform.position.y < -6)
            {
                UIManager.Instance.PushPanel(PanelType.SUMMARYPANEL);
                PlayerProperty.IsGameOver = true;
            }
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.transform.CompareTag("Floor"))
            {
                PointsEventChannel.RaiseFloorEvent();
            }
        }

        #endregion
    }
}