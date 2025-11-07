using System.Runtime.CompilerServices;
using Animation;
using UnityEngine;

namespace Player.Movement
{
    public static class Jump
    {
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
            AnimationsStateMachine.SetState(AnimationsStates.IsJumpingRight);
        }
        
        public static bool IsGrounded(Rigidbody2D rigidbody2D)
        {
            const float groundedDistance = 2f;
            int[] groundLayerMasks = new []
            {
                0b00000000000000000000000000001000, // layer 3 in bits
                1<<6 // layer 6 in bits
            };
            Vector2 position = rigidbody2D.position;
            RaycastHit2D returnValue = new();
            foreach (int groundLayerMask in groundLayerMasks)
            {
                RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, groundedDistance, groundLayerMask);
                if (hit.transform is null) continue;
                returnValue = hit;
            }
            #if UNITY_EDITOR
            if (returnValue.transform is not null) 
                Debug.DrawLine(position, new Vector2(position.x ,position.y + Vector2.down.y * groundedDistance), Color.black, 1000);
            #endif 
            return returnValue.transform is not null;
        }
    }
}