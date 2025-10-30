using Animation;
using UnityEngine;

namespace Player.Movement
{
    public static class Move
    {
        private static Rigidbody2D _rigidbody2D;
        private static float _speed;
        private static float _moveDirection;
        public static float MoveDirection => _moveDirection;

        public static void OnMovementAwake(Rigidbody2D rigidbody2D, float speed)
        {
            _rigidbody2D = rigidbody2D;
            _speed = speed;
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
        }

        private static void ApplyForces()
        {
            // Move Force
            _rigidbody2D.linearVelocityX = _moveDirection * (_speed);
        }
    }
}