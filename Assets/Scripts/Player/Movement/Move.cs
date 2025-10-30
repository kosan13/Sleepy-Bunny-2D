using System;
using Animation;
using UnityEngine;

namespace Player.Movement
{
    public enum MoveStats
    {
        Walk,
        Run
    }
    public static class Move
    {
        private static Rigidbody2D _rigidbody2D;
        private static float _walkSpeed;
        private static float _runSpeed;
        
        private static float _moveDirection;
        public static float MoveDirection => _moveDirection;

        private static MoveStats _moveStats;
        public static MoveStats MoveStats => _moveStats;

        public static void OnMovementAwake(Rigidbody2D rigidbody2D, float walkSpeed, float runSpeed)
        {
            _rigidbody2D = rigidbody2D;
            _walkSpeed = walkSpeed;
            _runSpeed = runSpeed;
        }
        public static void OnMovementUpdate() {}

        public static void OnMovementFixedUpdate()
        {
            ApplyForces();
        }
        
        public static void OnMove(float moveDirection)
        {
            AnimationsStateMachine.SetState(AnimationsStates.IsWalking);
            _moveDirection = moveDirection;
            _moveStats = MoveStats.Walk;
        }
        public static void OnRun(float moveDirection)
        {
            if (!Jump.IsGrounded(_rigidbody2D)) OnMove(moveDirection);
            
            AnimationsStateMachine.SetState(AnimationsStates.IsRunning);
            _moveDirection = moveDirection;
            _moveStats = moveDirection == 0 ? MoveStats.Walk : MoveStats.Run;
        }

        private static void ApplyForces()
        {
            Debug.Log("Move stat is Value: " + _moveStats);
            // Move Force
            _rigidbody2D.linearVelocityX = _moveStats switch
            {
                MoveStats.Walk => _moveDirection * _walkSpeed,
                MoveStats.Run => _moveDirection * _runSpeed,
                _ => _rigidbody2D.linearVelocityX
            };
        }
    }
}