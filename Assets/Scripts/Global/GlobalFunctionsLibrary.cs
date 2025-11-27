using System.Runtime.CompilerServices;
using Player;
using UnityEngine;
using static Global.GlobalVariablesLibrary;

namespace Global
{
    public static class GlobalFunctionsLibrary
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsGrounded(Rigidbody2D rigidbody2D, float groundedDistance = 2f)
        {
            int[] groundLayerMasks = { GroundLayerMask, PushAndPullLayerMask };
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


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void UpdatePlayerAnimationsDirection()
        {
            switch (PlayerMoveDirection)
            {
                case 1: PlayerAnimationsDirection = PlayerAnimationsDirectionTypes.Right; break;
                case -1: PlayerAnimationsDirection = PlayerAnimationsDirectionTypes.Left; break;
                default: return;
            }
        }
    }
}