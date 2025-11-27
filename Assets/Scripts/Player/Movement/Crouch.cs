using System.Runtime.CompilerServices;
using Animation;
using UnityEngine;

using static Player.PlayerController;

namespace Player.Movement
{
    public static class Crouch
    {
        
        private static CapsuleCollider2D _playerCollider;
        private static CapsuleCollider2D _playerCrouchCollider;
        public static bool GetIsCrouching { get; private set; }
        
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OnCrouchAwake(CapsuleCollider2D playerCollider, CapsuleCollider2D playerCrouchCollider)
        {
            _playerCollider = playerCollider;
            _playerCrouchCollider = playerCrouchCollider;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OnCrouchUpdate() {}
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OnCrouchFixedUpdate() {}
        
        
        public static void OnCrouch(bool relistButton = false)
        {
            if (relistButton) return;
            GetIsCrouching = !GetIsCrouching;
            if (GetIsCrouching)
            {
                _playerCollider.enabled = false;
                _playerCrouchCollider.enabled = true;
                AnimationsStateMachine.SetPlayerAnimationAndAnimationsDirection(GetPlayerController.MainAnimationBone, AnimationsStates.IsCrouchingLeft, AnimationsStates.IsCrouchingRight);
            }
            else
            {
                _playerCrouchCollider.enabled = false;
                _playerCollider.enabled = true;
                AnimationsStateMachine.SetPlayerAnimationAndAnimationsDirection(GetPlayerController.MainAnimationBone, AnimationsStates.IsStandingLeft, AnimationsStates.IsStandingRight);
            }
        }
    }
}