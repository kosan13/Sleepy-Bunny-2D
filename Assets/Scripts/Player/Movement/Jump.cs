using System.Runtime.CompilerServices;
using Animation;
using UnityEngine;

namespace Player.Movement
{
    public static class Jump
    {
        /// <summary>
        /// GroundLayerMask is the layer the Ground tag is on in the UnityEditor
        /// </summary>
        private const int GroundLayerMask = 0b00000000000000000000000000001000; // layer 3 in bits 
        
        private static Rigidbody2D _rigidbody2D;
        private static float _jumpPower;
        private static float _runJumpPower;
        private static float _runJumpMomentumBoost;

        public static bool GetIsJumping { get; private set; }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OnJumpAwake(Rigidbody2D rigidbody2D, float jumpPower, float runJumpPower, float runJumpMomentumBoost)
        {
            _rigidbody2D = rigidbody2D;
            _jumpPower = jumpPower;
            _runJumpPower = runJumpPower;
            GetIsJumping = false;
            _runJumpMomentumBoost = runJumpMomentumBoost;
        }
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OnJumpUpdate() {}
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void OnJumpFixedUpdate()
        {
            if (GetIsJumping == false) { _rigidbody2D.linearVelocityY += 0; return; }

            _rigidbody2D.linearVelocityY += Move.GetMoveState switch
            {
                MoveState.Walk => Vector2.up.y * _jumpPower,
                MoveState.Run => Vector2.up.y * (_runJumpPower),
                _ => _rigidbody2D.linearVelocityY
            };
            if (Move.GetMoveState == MoveState.Run) 
                _rigidbody2D.AddForceX(Move.GetMoveDirection * (_runJumpMomentumBoost * _rigidbody2D.mass), ForceMode2D.Impulse);
            
            GetIsJumping = false;
        }
        
        public static void OnJump(bool jump)
        {
            if (jump == false) { GetIsJumping = false; return; }
            
            if (!IsGrounded(_rigidbody2D))
            {
                Debug.LogError("IsGrounded is False"); 
                GetIsJumping = false;
                return;
            }
            GetIsJumping = true;
            AnimationsStateMachine.SetState(AnimationsStates.IsJumping);
        }
        
        public static bool IsGrounded(Rigidbody2D rigidbody2D)
        {
            const float groundedDistance = 2f;
            Vector2 position = rigidbody2D.position;
            
            RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, groundedDistance, GroundLayerMask);
            #if UNITY_EDITOR
            if (hit.transform is not null) 
                Debug.DrawLine(position, new Vector2(position.x ,position.y + Vector2.down.y * groundedDistance), Color.black, 1000);
            #endif
            return hit.transform is not null;
        }
    }
}