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
    public static class Run
    {
        private static float _runSpeed;
        private static float _crouchRunSpeed;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OnRunAwake(float runSpeed, float crouchRunSpeed)
        {
            _runSpeed = runSpeed;
            _crouchRunSpeed = crouchRunSpeed;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OnRunUpdate() {}

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OnRunFixedUpdate()
        {
            switch (PlayerMoveState)
            {
                case MoveState.Run: SetInputForce( Vector2.right * (PlayerMoveDirection * _runSpeed)); break;
                case MoveState.CrouchRun: SetInputForce( Vector2.right * (PlayerMoveDirection * _crouchRunSpeed)); break;
                case MoveState.Walk: 
                case MoveState.CrouchWalk:
                default: break;
            }
        }
        
        public static void OnRun(float moveDirection)
        {
            if (IsGrounded(PlayerRigidbody)) Walk.OnWalk(moveDirection);
            PlayerMoveDirection = moveDirection;
            UpdatePlayerAnimationsDirection();
            
            if (Crouch.GetIsCrouching)
            {
                PlayerMoveState = moveDirection == 0 ? MoveState.CrouchWalk : MoveState.CrouchRun;
                AnimationsStateMachine.SetPlayerAnimationAndAnimationsDirection(GetPlayerController.MainAnimationBone, AnimationsStates.IsCrouchingRunLeft, AnimationsStates.IsCrouchingRunRight);
                return;
            }
            PlayerMoveState = moveDirection == 0 ? MoveState.Walk : MoveState.Run;
            AnimationsStateMachine.SetPlayerAnimationAndAnimationsDirection(GetPlayerController.MainAnimationBone, AnimationsStates.IsRunningLeft, AnimationsStates.IsRunningRight);
        }
    }
}