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
    public static class Walk
    {
        private static float _walkSpeed;
        private static float _crouchWalkSpeed;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OnWalkAwake(float walkSpeed, float crouchWalkSpeed)
        {
            _walkSpeed = walkSpeed;
            _crouchWalkSpeed = crouchWalkSpeed;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OnWalkUpdate() {}
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OnWalkFixedUpdate()
        {
            switch (PlayerMoveState)
            {
                case MoveState.Walk: SetInputForce( Vector2.right * (PlayerMoveDirection * _walkSpeed)); break;
                case MoveState.CrouchWalk: SetInputForce( Vector2.right * (PlayerMoveDirection * _crouchWalkSpeed)); break;
                case MoveState.Run: 
                case MoveState.CrouchRun:
                default: break;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OnWalk(float moveDirection)
        {
            PlayerMoveDirection = moveDirection;
            UpdatePlayerAnimationsDirection();
            
            if (Crouch.GetIsCrouching)
            {
                PlayerMoveState = MoveState.CrouchWalk;
                AnimationsStateMachine.SetPlayerAnimationAndAnimationsDirection(GetPlayerController.MainAnimationBone, AnimationsStates.IsCrouchingWalkLeft, AnimationsStates.IsCrouchingWalkRight);
                return;
            }
            PlayerMoveState = MoveState.Walk;
            AnimationsStateMachine.SetPlayerAnimationAndAnimationsDirection(GetPlayerController.MainAnimationBone, AnimationsStates.IsWalkingLeft, AnimationsStates.IsWalkingRight);
        }
    }
}