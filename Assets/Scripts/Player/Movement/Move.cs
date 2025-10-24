using Animation;
using UnityEngine;

namespace Player.Movement
{
    public static class Move
    {
        private static Rigidbody2D _rigidbody2D;
        private static float _speed;
        private static float _moveDirection;

        public static void OnMovementAwake(Rigidbody2D rigidbody2D, float speed)
        {
            _rigidbody2D = rigidbody2D;
            _speed = speed;
        }
        public static void OnMovementUpdate() => ApplyForces();
        public static void OnMovementFixedUpdate() {}
        
        public static void OnMove(float moveDirection)
        {
            AnimationsStateMachine.SetState(AnimationsStates.IsWalking);
            _moveDirection = moveDirection;
        }

        private static void ApplyForces()
        {
            // Move Force
            _rigidbody2D.linearVelocityX = _moveDirection * (_speed * Time.deltaTime);;
        }
    }
}