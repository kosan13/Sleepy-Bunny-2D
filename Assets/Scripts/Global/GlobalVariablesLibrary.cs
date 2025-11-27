using Player;
using UnityEngine;
using static Global.GlobalEnumLibrary;

namespace Global
{
    public static class GlobalVariablesLibrary
    {
        #region Player
        
        public static int PlayerLayer { get; } = 7;
        public static int PlayerLayerMask { get; } = 1<<PlayerLayer; // PlayerLayer in bits
        
        
        public static MoveState PlayerMoveState { get; set; }
        public static float PlayerMoveDirection { get; set; }
        public static PlayerAnimationsDirectionTypes PlayerAnimationsDirection { get; set; }
        
        public static Rigidbody2D PlayerRigidbody => PlayerController.GetPlayerController.GetRigidbody2D;

        #endregion

        #region PushAndPull

        public static int PushAndPullLayer { get; } = 6;
        public static int PushAndPullLayerMask { get; } = 1<<PushAndPullLayer; // PushAndPullLayer in bits

        #endregion

        #region Ground

        public static int GroundLayer { get; } = 3;
        public static int GroundLayerMask { get; } = 1<<GroundLayer; // GroundLayer in bits

        #endregion
        
    }
}