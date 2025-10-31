using Animation;
using UnityEngine;

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
        private static float _crouchWalkPenalty;
        private static float _crouchRunPenalty;

        private static bool _crouching;
        
        public static float MoveDirection { get; private set; }
        public static MoveState MoveState { get; private set; }

        public static void OnMovementAwake(Rigidbody2D rigidbody2D, CapsuleCollider2D playerCollider, CapsuleCollider2D playerCrouchCollider, float walkSpeed, float runSpeed, float crouchWalkPenalty, float crouchRunPenalty)
        {
            _rigidbody2D = rigidbody2D;
            _playerCollider = playerCollider;
            _playerCrouchCollider = playerCrouchCollider;
            _walkSpeed = walkSpeed;
            _runSpeed = runSpeed;
            _crouchWalkPenalty = crouchWalkPenalty;
            _crouchRunPenalty = crouchRunPenalty;
        }
        public static void OnMovementUpdate() {}
        public static void OnMovementFixedUpdate() => ApplyForces();
        public static void OnWalk(float moveDirection)
        {
            MoveDirection = moveDirection;
            
            if (_crouching)
            {
                MoveState = MoveState.CrouchWalk;
                AnimationsStateMachine.SetState(AnimationsStates.CrouchWalk);
                return;
            }
            
            MoveState = MoveState.Walk;
            AnimationsStateMachine.SetState(AnimationsStates.IsWalking);
        }
        public static void OnRun(float moveDirection)
        {
            if (!Jump.IsGrounded(_rigidbody2D)) OnWalk(moveDirection);
            MoveDirection = moveDirection;
            
            if (_crouching)
            {
                MoveState = moveDirection == 0 ? MoveState.CrouchWalk : MoveState.CrouchRun;
                AnimationsStateMachine.SetState(AnimationsStates.CrouchRun);
                return;
            }
            
            MoveState = moveDirection == 0 ? MoveState.Walk : MoveState.Run;
            AnimationsStateMachine.SetState(AnimationsStates.IsRunning);
        }
        public static void OnCrouch(bool relistButton = false)
        {
            if (relistButton) return;
            _crouching = !_crouching;
            if (_crouching)
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
            Debug.Log("MoveState is Value: " + MoveState);
            // Move Force
            _rigidbody2D.linearVelocityX = MoveState switch
            {
                MoveState.Walk => MoveDirection * _walkSpeed,
                MoveState.Run => MoveDirection * _runSpeed,
                MoveState.CrouchWalk => MoveDirection * _walkSpeed / _crouchWalkPenalty,
                MoveState.CrouchRun => MoveDirection * _runSpeed / _crouchRunPenalty,
                _ => _rigidbody2D.linearVelocityX
            };
        }
    }
}