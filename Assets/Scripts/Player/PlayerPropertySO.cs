using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Roots.Player
{
    [System.Serializable]
    [CreateAssetMenu(menuName = "Player/Property")]
    public class PlayerPropertySO : ScriptableObject
    {
        public int JumpForce;
        public int JumpCount;
        public int Points;
        public int Floor;
        public int InvincibleTime;
        public int HP;
        public int Mission;
        public int WalkSpeed;
        public float MoveInput;
        public bool LandPress;
        public bool JumpPress;
        public bool IsMagic;
        public bool IsPause = false;
        public bool IsInvincible = false;
        public bool IsGameOver = false;
    }
}