using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Roots.Player.Input
{
    [CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
    public class InputReader : ScriptableObject, GameInput.IGameplayActions, GameInput.IMenuActions
    {
        #region 字段

        // Gameplay
        public event UnityAction JumpEvent = delegate { };
        public event UnityAction JumpCanceledEvent = delegate { };
        public event UnityAction LandEvent = delegate { };
        public event UnityAction<Vector2> MoveEvent = delegate { };
        public event UnityAction PauseEvent = delegate { };

        // Menu
        public event UnityAction MenuUnpauseEvent = delegate { };

        public GameInput gameInput;

        #endregion

        #region Unity回调

        private void OnEnable()
        {
            if (gameInput == null)
            {
                gameInput = new GameInput();
                gameInput.Menu.SetCallbacks(this);
                gameInput.Gameplay.SetCallbacks(this);
            }

            EnableGameplayInput();
        }

        private void OnDisable()
        {
            DisableAllInput();
        }

        #endregion

        #region 方法


        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                JumpEvent.Invoke();
            }

            if (context.phase == InputActionPhase.Canceled)
            {
                JumpCanceledEvent.Invoke();
            }
        }

        public void OnLand(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                LandEvent.Invoke();
            }
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            MoveEvent.Invoke(context.ReadValue<Vector2>());
        }

        public void OnPause(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                PauseEvent.Invoke();
            }
        }

        public void OnUnpause(InputAction.CallbackContext context)
        {
            if (context.phase == InputActionPhase.Performed)
            {
                MenuUnpauseEvent();
            }
        }

        public void EnableGameplayInput()
        {
            gameInput.Menu.Disable();
            gameInput.Gameplay.Enable();
        }

        public void DisableAllInput()
        {
            gameInput.Gameplay.Disable();
            gameInput.Menu.Disable();
        }

        #endregion
    }
}

