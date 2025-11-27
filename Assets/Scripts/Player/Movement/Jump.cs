using System.Runtime.CompilerServices;
using Animation;
using UnityEngine;
using static Global.GlobalEnumLibrary;
using static Global.GlobalFunctionsLibrary;
using static Global.GlobalVariablesLibrary;
using static Player.Movement.ForceAccumulate;
using static Player.PlayerController;

namespace Player.Movement
{
    public static class Jump
    {
        private static float _jumpPower;
        private static float _runJumpPower;
        private static float _runJumpMomentumBoost;

        public static bool GetIsJumping { get; private set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OnJumpAwake(float jumpPower, float runJumpPower, float runJumpMomentumBoost)
        {
            _jumpPower = jumpPower;
            _runJumpPower = runJumpPower;
            _runJumpMomentumBoost = runJumpMomentumBoost;
            GetIsJumping = false;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OnJumpUpdate() {}
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OnJumpFixedUpdate()
        {
            if (GetIsJumping == false) { PlayerRigidbody.linearVelocityY += 0; return; }
            
            switch (PlayerMoveState)
            {
                case MoveState.Walk:
                    SetGravityForce(Vector2.up * _jumpPower); break;
                case MoveState.Run: 
                    SetGravityForce(Vector2.up * _runJumpPower);
                    SetRunJumpBoostForce(Vector2.up * _runJumpMomentumBoost);
                    break;
                case MoveState.CrouchWalk:
                case MoveState.CrouchRun:
                default: break;
            }
            GetIsJumping = false;
        }
        
        public static void OnJump(bool jump)
        {
            if (jump == false) { GetIsJumping = false; return; }
            
            if (!IsGrounded(PlayerRigidbody))
            {
                Debug.LogError("IsGrounded is False"); 
                GetIsJumping = false;
                return;
            }
            GetIsJumping = true;
            AnimationsStateMachine.SetPlayerAnimationAndAnimationsDirection(GetPlayerController.MainAnimationBone, AnimationsStates.IsJumpingLeft, AnimationsStates.IsJumpingRight);
        }
    }
}