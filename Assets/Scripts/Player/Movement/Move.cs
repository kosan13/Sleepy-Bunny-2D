using System.Runtime.CompilerServices;
using Animation;
using UnityEngine;

using static Global.GlobalFunctionsLibrary;
using static Player.PlayerController;

namespace Player.Movement
{
    public enum MoveState
    {
        Walk,
        Run,
        CrouchWalk,
        CrouchRun
    }
    public static class Move
    {
        private static Rigidbody2D _rigidbody2D;
        private static CapsuleCollider2D _playerCollider;
        private static CapsuleCollider2D _playerCrouchCollider;
        private static float _walkSpeed;
        private static float _runSpeed;
        private static float _crouchWalkSpeed;
        private static float _crouchRunSpeed;

        public static float GetMoveDirection { get; private set; }
        public static MoveState GetMoveState { get; private set; }
        public static bool GetIsCrouching { get; private set; }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OnMovementAwake(Rigidbody2D rigidbody2D, CapsuleCollider2D playerCollider, CapsuleCollider2D playerCrouchCollider, float walkSpeed, float runSpeed, float crouchWalkSpeed, float crouchRunSpeed)
        {
            _rigidbody2D = rigidbody2D;
            _playerCollider = playerCollider;
            _playerCrouchCollider = playerCrouchCollider;
            _walkSpeed = walkSpeed;
            _runSpeed = runSpeed;
            _crouchWalkSpeed = crouchWalkSpeed;
            _crouchRunSpeed = crouchRunSpeed;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OnMovementUpdate() {}
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OnMovementFixedUpdate() => ApplyForces();
        
        public static void OnWalk(float moveDirection)
        {
            GetMoveDirection = moveDirection;
            
            if (GetIsCrouching)
            {
                GetMoveState = MoveState.CrouchWalk;
                AnimationsStateMachine.SetPlayerAnimationAndAnimationsDirection(_rigidbody2D, GetPlayerController.MainAnimationBone, AnimationsStates.IsCrouchingWalkLeft, AnimationsStates.IsCrouchingWalkRight);
                return;
            }
            
            GetMoveState = MoveState.Walk;
            AnimationsStateMachine.SetPlayerAnimationAndAnimationsDirection(_rigidbody2D, GetPlayerController.MainAnimationBone, AnimationsStates.IsWalkingLeft, AnimationsStates.IsWalkingRight);
        }
        public static void OnRun(float moveDirection)
        {
            if (IsGrounded(_rigidbody2D)) OnWalk(moveDirection);
            GetMoveDirection = moveDirection;
            
            if (GetIsCrouching)
            {
                GetMoveState = moveDirection == 0 ? MoveState.CrouchWalk : MoveState.CrouchRun;
                AnimationsStateMachine.SetPlayerAnimationAndAnimationsDirection(_rigidbody2D, GetPlayerController.MainAnimationBone, AnimationsStates.IsCrouchingRunLeft, AnimationsStates.IsCrouchingRunRight);
                return;
            }
            
            GetMoveState = moveDirection == 0 ? MoveState.Walk : MoveState.Run;
            AnimationsStateMachine.SetPlayerAnimationAndAnimationsDirection(_rigidbody2D, GetPlayerController.MainAnimationBone, AnimationsStates.IsRunningLeft, AnimationsStates.IsRunningRight);
        }
        public static void OnCrouch(bool relistButton = false)
        {
            if (relistButton) return;
            GetIsCrouching = !GetIsCrouching;
            if (GetIsCrouching)
            {
                _playerCollider.enabled = false;
                _playerCrouchCollider.enabled = true;
            }
            else
            {
                _playerCrouchCollider.enabled = false;
                _playerCollider.enabled = true;
            }
        }

        private static void ApplyForces()
        {
            if (Jump.GetIsJumping) return;
            Debug.Log("MoveState is Value: " + GetMoveState);
            // Move Force
            _rigidbody2D.linearVelocityX = GetMoveState switch
            {
                MoveState.Walk => GetMoveDirection * _walkSpeed,
                MoveState.Run => GetMoveDirection * _runSpeed,
                MoveState.CrouchWalk => GetMoveDirection * _crouchWalkSpeed,
                MoveState.CrouchRun => GetMoveDirection * _crouchRunSpeed,
                _ => _rigidbody2D.linearVelocityX
            };
        }
    }
}