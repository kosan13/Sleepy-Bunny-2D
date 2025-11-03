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
        
        private static AnimationsStateMachine _stateMachine;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnEnable() => _stateMachine = this;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnDisable() => _stateMachine = _stateMachine == this ? null : _stateMachine;
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void OnValidate() { if (animator is null) Debug.LogWarning("animator is null in the AnimationsStateMachine"); }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void Start() { foreach (AnimationMachineStates state in _stateMachine.machineStates) state.GenerateHash(); }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void SetState(AnimationsStates newState) { foreach (AnimationMachineStates state in _stateMachine.machineStates) state.Poll(newState); }
        


        public static bool TrySetStateIsIdling()
        {
            if (!Jump.IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (PlayerController.GetPlayerController.GetRigidbody2D.linearVelocityX != 0) return false;
            SetState(AnimationsStates.IsIdling);
            return true;
        }
        public static bool TrySetStateIsPushing()
        {
            if (!PushAndPull.PushOrPull) return false;
            if (!Jump.IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (true) return false;
            SetState(AnimationsStates.IsPushing);
            return true;
        }
        public static bool TrySetStateIsPulling()
        {
            if (!PushAndPull.PushOrPull) return false;
            if (!Jump.IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (true) return false;
            SetState(AnimationsStates.IsPulling);
            return true;
        }
        public static bool TrySetStateIsWalking()
        {
            Rigidbody2D rigidbody2D = PlayerController.GetPlayerController.GetRigidbody2D;
            if (!Jump.IsGrounded(rigidbody2D)) return false;
            if (rigidbody2D.linearVelocityX == 0) return false;
            SetState(AnimationsStates.IsWalking);
            return true;
        }
        public static bool TrySetStateIsRunning()
        {
            if (!Jump.IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (true) return false;
            SetState(AnimationsStates.IsRunning);
            return true;
        }
        public static bool TrySetStateCrouchWalk()
        {
            if (!Move.GetIsCrouching) return false;
            if (!Jump.IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            SetState(AnimationsStates.IsCrouchWalk);
            return true;
        }
        public static bool TrySetStateCrouchRun()
        {
            if (!Move.GetIsCrouching) return false;
            if (!Jump.IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (true) return false;
            SetState(AnimationsStates.IsCrouchRun);
            return true;
        }
        public static bool TrySetStateIsJumping()
        {
            if (!Jump.IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            if (!Jump.GetIsJumping) return false;
            SetState(AnimationsStates.IsJumping);
            return true;
        }
        public static bool TrySetStateIsFalling()
        {
            if (Jump.IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            SetState(AnimationsStates.IsFalling);
            return true;
        }
        public static bool TrySetStateIsLanding()
        {
            if (!Jump.IsGrounded(PlayerController.GetPlayerController.GetRigidbody2D)) return false;
            SetState(AnimationsStates.IsLanding);
            return true;
        }
    }
}