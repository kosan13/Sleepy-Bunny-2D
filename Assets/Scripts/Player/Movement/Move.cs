using System;
using Animation;
using UnityEngine;

namespace Player.Movement
{
    public enum WalkOrRun
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

        private static WalkOrRun _walkOrRun;
        public static WalkOrRun WalkOrRun => _walkOrRun;

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
            _walkOrRun = WalkOrRun.Walk;
        }
        public static void OnRun(float moveDirection)
        {
            AnimationsStateMachine.SetState(AnimationsStates.IsRunning);
            _moveDirection = moveDirection;
            _walkOrRun = WalkOrRun.Run;
        }

        private static void ApplyForces()
        {
            // Move Force
            _rigidbody2D.linearVelocityX = _walkOrRun switch
            {
                WalkOrRun.Walk => _moveDirection * _walkSpeed,
                WalkOrRun.Run => _moveDirection * _runSpeed,
                _ => _rigidbody2D.linearVelocityX
            };
        }
    }
}