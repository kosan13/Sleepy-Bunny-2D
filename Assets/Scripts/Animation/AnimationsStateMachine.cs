using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Player;
using Player.Movement;
using UnityEngine;

using static Global.GlobalFunctionsLibrary;
namespace Animation
{
    public partial class AnimationsStateMachine : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private AnimationMachineStates[] machineStates;

        public Dictionary<AnimationsStates, AnimationMachineStates> AnimationMachineStatesMapping { get; } = new ();

        public static AnimationsStateMachine StateMachine { get; private set; }
        public Animator Animator => animator; 

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnEnable() => StateMachine = this;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnDisable() => StateMachine = StateMachine == this ? null : StateMachine;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnValidate() { if (animator is null) Debug.LogWarning("animator is null in the AnimationsStateMachine"); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Start()
        {
            foreach (AnimationMachineStates state in StateMachine.machineStates)
            {
                state.GenerateHash();
                AnimationMachineStatesMapping.Add(state.AnimationsStates, state);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetState(AnimationsStates newState) { foreach (AnimationMachineStates state in StateMachine.machineStates) state.Poll(newState); }
        
        
        
        public static AnimationsStates SetPlayerAnimationAndAnimationsDirection(Rigidbody2D rigidbody2D, Transform mainAnimationBone, AnimationsStates animationsStateLeft, AnimationsStates animationsStateRight)
        {
            PlayerAnimationsDirection playerAnimationsDirection = rigidbody2D.linearVelocityX switch
            {
                > 0 => PlayerAnimationsDirection.Right,
                < 0 => PlayerAnimationsDirection.Left,
                _ => PlayerAnimationsDirection.Right
            };
            switch(playerAnimationsDirection)
            {
                case PlayerAnimationsDirection.Left:
                {
                    mainAnimationBone.transform.rotation.Set(0, 180, 0, 0);
                    SetState(animationsStateLeft);
                    return animationsStateLeft;
                }
                case PlayerAnimationsDirection.Right:
                {
                    mainAnimationBone.transform.rotation.Set(0, 0, 0, 0); 
                    SetState(animationsStateRight);
                    return animationsStateRight;
                }
                default: throw new ArgumentOutOfRangeException();
            }
        }
        


        public static bool TrySetStateIsIdling(Rigidbody2D rigidbody2D, Transform mainAnimationBone)
        {
            if (!IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (PlayerController.GetPlayerController.GetRigidbody2D.linearVelocityX != 0) return false;
            SetPlayerAnimationAndAnimationsDirection(rigidbody2D, mainAnimationBone, AnimationsStates.IsIdlingLeft, AnimationsStates.IsIdlingRight);
            return true;
        }
        public static bool TrySetStateIsPushing(Rigidbody2D rigidbody2D, Transform mainAnimationBone)
        {
            if (!IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (PlayerController.GetPlayerController.GetRigidbody2D.linearVelocityX != 0) return false;
            SetPlayerAnimationAndAnimationsDirection(rigidbody2D, mainAnimationBone, AnimationsStates.IsPushingLeft, AnimationsStates.IsPushingRight);
            return true;
        }
        public static bool TrySetStateIsPulling(Rigidbody2D rigidbody2D, Transform mainAnimationBone)
        {
            if (!IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (PlayerController.GetPlayerController.GetRigidbody2D.linearVelocityX != 0) return false;
            SetPlayerAnimationAndAnimationsDirection(rigidbody2D, mainAnimationBone, AnimationsStates.IsPullingLeft, AnimationsStates.IsPullingRight);
            return true;
        }
        public static bool TrySetStateIsWalking(Rigidbody2D rigidbody2D, Transform mainAnimationBone)
        {
            if (!IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (PlayerController.GetPlayerController.GetRigidbody2D.linearVelocityX != 0) return false;
            SetPlayerAnimationAndAnimationsDirection(rigidbody2D, mainAnimationBone, AnimationsStates.IsWalkingLeft, AnimationsStates.IsWalkingRight);
            return true;
        }
        public static bool TrySetStateIsRunning(Rigidbody2D rigidbody2D, Transform mainAnimationBone)
        {
            if (!IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (PlayerController.GetPlayerController.GetRigidbody2D.linearVelocityX != 0) return false;
            SetPlayerAnimationAndAnimationsDirection(rigidbody2D, mainAnimationBone, AnimationsStates.IsRunningLeft, AnimationsStates.IsRunningRight);
            return true;
        }
        public static bool TrySetStateCrouchWalk(Rigidbody2D rigidbody2D, Transform mainAnimationBone)
        {
            if (!IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (PlayerController.GetPlayerController.GetRigidbody2D.linearVelocityX != 0) return false;
            SetPlayerAnimationAndAnimationsDirection(rigidbody2D, mainAnimationBone, AnimationsStates.IsCrouchingWalkLeft, AnimationsStates.IsCrouchingWalkRight);
            return true;
        }
        public static bool TrySetStateCrouchRun(Rigidbody2D rigidbody2D, Transform mainAnimationBone)
        {
            if (!IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (PlayerController.GetPlayerController.GetRigidbody2D.linearVelocityX != 0) return false;
            SetPlayerAnimationAndAnimationsDirection(rigidbody2D, mainAnimationBone, AnimationsStates.IsCrouchingRunLeft, AnimationsStates.IsCrouchingRunRight);
            return true;
        }
        public static bool TrySetStateIsJumping(Rigidbody2D rigidbody2D, Transform mainAnimationBone)
        {
            if (!IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (PlayerController.GetPlayerController.GetRigidbody2D.linearVelocityX != 0) return false;
            SetPlayerAnimationAndAnimationsDirection(rigidbody2D, mainAnimationBone, AnimationsStates.IsJumpingLeft, AnimationsStates.IsJumpingRight);
            return true;
        }
        public static bool TrySetStateIsFalling(Rigidbody2D rigidbody2D, Transform mainAnimationBone)
        {
            if (!IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (PlayerController.GetPlayerController.GetRigidbody2D.linearVelocityX != 0) return false;
            SetPlayerAnimationAndAnimationsDirection(rigidbody2D, mainAnimationBone, AnimationsStates.IsFallingLeft, AnimationsStates.IsFallingRight);
            return true;
        }
        public static bool TrySetStateIsLanding(Rigidbody2D rigidbody2D, Transform mainAnimationBone)
        {
            if (!IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (PlayerController.GetPlayerController.GetRigidbody2D.linearVelocityX != 0) return false;
            SetPlayerAnimationAndAnimationsDirection(rigidbody2D, mainAnimationBone, AnimationsStates.IsLandingLeft, AnimationsStates.IsLandingRight);
            return true;
        }
        public static bool TrySetStateIsDying(Rigidbody2D rigidbody2D, Transform mainAnimationBone)
        {
            SetPlayerAnimationAndAnimationsDirection(rigidbody2D, mainAnimationBone, AnimationsStates.IsDyingLeft, AnimationsStates.IsDyingRight);
            return true;
        }
    }
}