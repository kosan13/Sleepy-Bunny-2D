using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Player;
using Player.Movement;
using UnityEngine;
using static Global.GlobalFunctionsLibrary;
using static Global.GlobalVariablesLibrary;

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
        
        
        
        public static AnimationsStates SetPlayerAnimationAndAnimationsDirection(Transform mainAnimationBone, AnimationsStates animationsStateLeft, AnimationsStates animationsStateRight)
        {
            switch(PlayerAnimationsDirection)
            {
                case PlayerAnimationsDirectionTypes.Left:
                {
                    mainAnimationBone.transform.rotation.Set(0, 180, 0, 0);
                    SetState(animationsStateLeft);
                    return animationsStateLeft;
                }
                case PlayerAnimationsDirectionTypes.Right:
                {
                    mainAnimationBone.transform.rotation.Set(0, 0, 0, 0); 
                    SetState(animationsStateRight);
                    return animationsStateRight;
                }
                default: throw new ArgumentOutOfRangeException();
            }
        }
        


        public static bool TrySetStateIsIdling(Transform mainAnimationBone)
        {
            if (!IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (PlayerController.GetPlayerController.GetRigidbody2D.linearVelocityX != 0) return false;
            SetPlayerAnimationAndAnimationsDirection(mainAnimationBone, AnimationsStates.IsIdlingLeft, AnimationsStates.IsIdlingRight);
            return true;
        }
        public static bool TrySetStateIsPushing(Transform mainAnimationBone)
        {
            if (!IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (PlayerController.GetPlayerController.GetRigidbody2D.linearVelocityX != 0) return false;
            SetPlayerAnimationAndAnimationsDirection(mainAnimationBone, AnimationsStates.IsPushingLeft, AnimationsStates.IsPushingRight);
            return true;
        }
        public static bool TrySetStateIsPulling(Transform mainAnimationBone)
        {
            if (!IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (PlayerController.GetPlayerController.GetRigidbody2D.linearVelocityX != 0) return false;
            SetPlayerAnimationAndAnimationsDirection(mainAnimationBone, AnimationsStates.IsPullingLeft, AnimationsStates.IsPullingRight);
            return true;
        }
        public static bool TrySetStateIsWalking(Transform mainAnimationBone)
        {
            if (!IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (PlayerController.GetPlayerController.GetRigidbody2D.linearVelocityX != 0) return false;
            SetPlayerAnimationAndAnimationsDirection(mainAnimationBone, AnimationsStates.IsWalkingLeft, AnimationsStates.IsWalkingRight);
            return true;
        }
        public static bool TrySetStateIsRunning(Transform mainAnimationBone)
        {
            if (!IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (PlayerController.GetPlayerController.GetRigidbody2D.linearVelocityX != 0) return false;
            SetPlayerAnimationAndAnimationsDirection(mainAnimationBone, AnimationsStates.IsRunningLeft, AnimationsStates.IsRunningRight);
            return true;
        }
        public static bool TrySetStateCrouchWalk(Transform mainAnimationBone)
        {
            if (!IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (PlayerController.GetPlayerController.GetRigidbody2D.linearVelocityX != 0) return false;
            if (PlayerController.GetPlayerController.GetRigidbody2D.linearVelocityX < PlayerController.GetPlayerController.WalkSpeed) return false;
            SetPlayerAnimationAndAnimationsDirection(mainAnimationBone, AnimationsStates.IsCrouchingWalkLeft, AnimationsStates.IsCrouchingWalkRight);
            return true;
        }
        public static bool TrySetStateCrouchRun(Transform mainAnimationBone)
        {
            if (!IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (PlayerController.GetPlayerController.GetRigidbody2D.linearVelocityX != 0) return false;
            if (PlayerController.GetPlayerController.GetRigidbody2D.linearVelocityX < PlayerController.GetPlayerController.CrouchWalkSpeed) return false;
            SetPlayerAnimationAndAnimationsDirection(mainAnimationBone, AnimationsStates.IsCrouchingRunLeft, AnimationsStates.IsCrouchingRunRight);
            return true;
        }
        public static bool TrySetStateIsJumping(Transform mainAnimationBone)
        {
            if (!IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (PlayerController.GetPlayerController.GetRigidbody2D.linearVelocityX != 0) return false;
            SetPlayerAnimationAndAnimationsDirection(mainAnimationBone, AnimationsStates.IsJumpingLeft, AnimationsStates.IsJumpingRight);
            return true;
        }
        public static bool TrySetStateIsFalling(Transform mainAnimationBone)
        {
            if (!IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (PlayerController.GetPlayerController.GetRigidbody2D.linearVelocityX != 0) return false;
            SetPlayerAnimationAndAnimationsDirection(mainAnimationBone, AnimationsStates.IsFallingLeft, AnimationsStates.IsFallingRight);
            return true;
        }
        public static bool TrySetStateIsLanding(Transform mainAnimationBone)
        {
            if (!IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (PlayerController.GetPlayerController.GetRigidbody2D.linearVelocityX != 0) return false;
            SetPlayerAnimationAndAnimationsDirection(mainAnimationBone, AnimationsStates.IsLandingLeft, AnimationsStates.IsLandingRight);
            return true;
        }
        public static bool TrySetStateIsDying(Transform mainAnimationBone)
        {
            SetPlayerAnimationAndAnimationsDirection(mainAnimationBone, AnimationsStates.IsDyingLeft, AnimationsStates.IsDyingRight);
            return true;
        }
        public static bool TrySetStateIsIsCrouchingIdling(Transform mainAnimationBone)
        {
            if (!Crouch.GetIsCrouching) return false;
            if (!IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (PlayerController.GetPlayerController.GetRigidbody2D.linearVelocityX != 0) return false;
            SetPlayerAnimationAndAnimationsDirection(mainAnimationBone, AnimationsStates.IsCrouchingIdlingLeft, AnimationsStates.IsCrouchingIdlingRight);
            return true;
        }
        public static bool TrySetStateIsCrouching(Transform mainAnimationBone)
        {
            if (!Crouch.GetIsCrouching) return false;
            if (!IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            SetPlayerAnimationAndAnimationsDirection(mainAnimationBone, AnimationsStates.IsCrouchingLeft, AnimationsStates.IsCrouchingRight);
            return true;
        }
        public static bool TrySetStateIsStanding(Transform mainAnimationBone)
        {
            if (Crouch.GetIsCrouching) return false;
            if (!IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            SetPlayerAnimationAndAnimationsDirection(mainAnimationBone, AnimationsStates.IsStandingLeft, AnimationsStates.IsStandingRight);
            return true;
        }
    }
}