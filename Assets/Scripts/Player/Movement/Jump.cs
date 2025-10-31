using System;
using Animation;
using UnityEngine;

namespace Player.Movement
{
    public static class Jump
    {
        private static Rigidbody2D _rigidbody2D;
        private static float _jumpPower;
        private static float _runJumpPenalty;
        private static bool _isJump;
        
        /// <summary>
        /// GroundLayerMask is the layer the Ground tag is on in the UnityEditor
        /// </summary>
        private const int GroundLayerMask = 3;
        
        public static void OnJumpAwake(Rigidbody2D rigidbody2D, float jumpPower, float runJumpPenalty)
        {
            _rigidbody2D = rigidbody2D;
            _jumpPower = jumpPower;
            _runJumpPenalty = runJumpPenalty;
            _isJump = false;
        }
        public static void OnJumpUpdate() {}
        public static void OnJumpFixedUpdate()
        {
            if (_isJump == false) { _rigidbody2D.linearVelocityY += 0; return; }

            _rigidbody2D.linearVelocityY += Move.MoveState switch
            {
                MoveState.Walk => Vector2.up.y * _jumpPower,
                MoveState.Run => Vector2.up.y * (_jumpPower / _runJumpPenalty),
                _ => _rigidbody2D.linearVelocityY
            };
            _isJump = false;
        }
        
        public static void OnJump(bool jump)
        {
            if (jump == false) { _isJump = false; return; }
            
            if (!IsGrounded(_rigidbody2D))
            {
                Debug.LogError("IsGrounded is False"); 
                _isJump = false;
                return;
            }
            _isJump = true;
            AnimationsStateMachine.SetState(AnimationsStates.IsJumping);
        }
        
        public static bool IsGrounded(Rigidbody2D rigidbody2D)
        {
            const float groundedDistance = 3f;
            Vector2 position = rigidbody2D.position;

            Debug.Log(LayerMask.NameToLayer("Ground"));
            Debug.DrawLine(position, new Vector2(position.x ,position.y + Vector2.down.y * groundedDistance), Color.black, 1000);
            RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, groundedDistance);
            return hit.transform is not null;
        }
    }
}