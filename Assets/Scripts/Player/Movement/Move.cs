using Animation;
using UnityEngine;

namespace Player.Movement
{
    public enum MoveState
    {
        Walk,
        Run
    }
    public static class Move
    {
        private static Rigidbody2D _rigidbody2D;
        private static float _walkSpeed;
        private static float _runSpeed;

        public static float MoveDirection { get; private set; }
        public static MoveState MoveState { get; private set; }

        public static void OnMovementAwake(Rigidbody2D rigidbody2D, float walkSpeed, float runSpeed)
        {
            _rigidbody2D = rigidbody2D;
            _walkSpeed = walkSpeed;
            _runSpeed = runSpeed;
        }
        public static void OnMovementUpdate() {}
        public static void OnMovementFixedUpdate() => ApplyForces();
        public static void OnMove(float moveDirection)
        {
            MoveDirection = moveDirection;
            MoveState = MoveState.Walk;
            AnimationsStateMachine.SetState(AnimationsStates.IsWalking);
        }
        public static void OnRun(float moveDirection)
        {
            if (!Jump.IsGrounded(_rigidbody2D)) OnMove(moveDirection);
            MoveDirection = moveDirection;
            MoveState = moveDirection == 0 ? MoveState.Walk : MoveState.Run;
            AnimationsStateMachine.SetState(AnimationsStates.IsRunning);
        }

        private static void ApplyForces()
        {
            Debug.Log("MoveState is Value: " + MoveState);
            // Move Force
            _rigidbody2D.linearVelocityX = MoveState switch
            {
                MoveState.Walk => MoveDirection * _walkSpeed,
                MoveState.Run => MoveDirection * _runSpeed,
                _ => _rigidbody2D.linearVelocityX
            };
        }
    }
}