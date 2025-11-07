using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Player;
using Player.Movement;
using UnityEngine;

namespace Animation
{
    public partial class AnimationsStateMachine : MonoBehaviour
    {
        [SerializeField] private Animator animator;
        [SerializeField] private AnimationMachineStates[] machineStates;

        private Dictionary<AnimationsStates, AnimationMachineStates> _animationMachineStatesMapping;
        public Dictionary<AnimationsStates, AnimationMachineStates> AnimationMachineStatesMapping => _animationMachineStatesMapping;

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
                _animationMachineStatesMapping.Add(state.AnimationsStates, state);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetState(AnimationsStates newState) { foreach (AnimationMachineStates state in StateMachine.machineStates) state.Poll(newState); }
        


        public static bool TrySetStateIsIdling()
        {
            if (!Jump.IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (PlayerController.GetPlayerController.GetRigidbody2D.linearVelocityX != 0) return false;
            SetState(AnimationsStates.IsIdlingRight);
            return true;
        }
        public static bool TrySetStateIsPushing()
        {
            if (!PushAndPull.PushOrPull) return false;
            if (!Jump.IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (true) return false;
            SetState(AnimationsStates.IsPushingRight);
            return true;
        }
        public static bool TrySetStateIsPulling()
        {
            if (!PushAndPull.PushOrPull) return false;
            if (!Jump.IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (true) return false;
            SetState(AnimationsStates.IsPullingRight);
            return true;
        }
        public static bool TrySetStateIsWalking()
        {
            Rigidbody2D rigidbody2D = PlayerController.GetPlayerController.GetRigidbody2D;
            if (!Jump.IsGrounded(rigidbody2D)) return false;
            if (rigidbody2D.linearVelocityX == 0) return false;
            SetState(AnimationsStates.IsWalkingRight);
            return true;
        }
        public static bool TrySetStateIsRunning()
        {
            if (!Jump.IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (true) return false;
            SetState(AnimationsStates.IsRunningRight);
            return true;
        }
        public static bool TrySetStateCrouchWalk()
        {
            if (!Move.GetIsCrouching) return false;
            if (!Jump.IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            SetState(AnimationsStates.IsCrouchingWalkRight);
            return true;
        }
        public static bool TrySetStateCrouchRun()
        {
            if (!Move.GetIsCrouching) return false;
            if (!Jump.IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (true) return false;
            SetState(AnimationsStates.IsCrouchingRunRight);
            return true;
        }
        public static bool TrySetStateIsJumping()
        {
            if (!Jump.IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (!Jump.GetIsJumping) return false;
            SetState(AnimationsStates.IsJumpingRight);
            return true;
        }
        public static bool TrySetStateIsFalling()
        {
            if (Jump.IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            SetState(AnimationsStates.IsFallingRight);
            return true;
        }
        public static bool TrySetStateIsLanding()
        {
            if (!Jump.IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            SetState(AnimationsStates.IsLandingRight);
            return true;
        }
    }
}