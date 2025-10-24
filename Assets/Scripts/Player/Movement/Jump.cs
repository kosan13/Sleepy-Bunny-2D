using Animation;
using UnityEngine;

namespace Player.Movement
{
    public static class Jump
    {
        private static Rigidbody2D _rigidbody2D;
        private static float _jumpPower;
        private static bool _isJump;
        
        /// <summary>
        /// GroundLayerMask is the layer the Ground tag is on in the UnityEditor
        /// </summary>
        private const int GroundLayerMask = 3;
        private const int RayCastLength = 1000;
        
        public static void OnJumpAwake(Rigidbody2D rigidbody2D, float jumpPower)
        {
            _rigidbody2D = rigidbody2D;
            _jumpPower = jumpPower;
            _isJump = false;
        }
        public static void OnJumpUpdate()
        {
            if (_isJump == false) { _rigidbody2D.linearVelocity += Vector2.zero; return; }

            // _rigidbody2D.linearVelocity += Vector2.up * (_jumpPower * Time.deltaTime);
            _rigidbody2D.linearVelocity += Vector2.up * _jumpPower;
            
            _isJump = false;

        }
        public static void OnJumpFixedUpdate() {}
        
        public static void OnJump(bool jump)
        {
            if (jump == false)
            {
                _isJump = false;
                return;
            }
            
            if (!IsGrounded(_rigidbody2D))
            {
                Debug.LogError("\"IsGrounded is False"); 
                _isJump = false;
                return;
            }
            _isJump = true;
            AnimationsStateMachine.SetState(AnimationsStates.IsJumping);
        }
        
        public static bool IsGrounded(Rigidbody2D rigidbody2D)
        {
            const float groundedDistance = 3f;
            // if (rigidbody2D.linearVelocityY <= 0)
            // {
            //     Debug.Log("IsGrounded failed because of linearVelocityY <= 0 and the value was Value: " + _rigidbody2D.linearVelocityY);
            //     return false;
            // }

            Vector2 position = rigidbody2D.transform.position;

            Debug.Log(LayerMask.NameToLayer("Ground"));
            Debug.DrawLine(position, new Vector2(position.x ,position.y + Vector2.down.y * groundedDistance), Color.black, 1000);
            RaycastHit2D hit = Physics2D.Raycast(position, Vector2.down, groundedDistance, GroundLayerMask);
            return hit.transform is not null;
        }
    }
}