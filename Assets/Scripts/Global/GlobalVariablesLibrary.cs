namespace Global
{
    public static class GlobalVariablesLibrary
    {
        #region Player
        
        public static int PlayerLayer { get; } = 7;
        public static int PlayerLayerMask { get; } = 1<<PlayerLayer; // PlayerLayer in bits

        #endregion

        #region PushAndPull

        public static int PushAndPullLayer { get; } = 6;
        public static int PushAndPullLayerMask { get; } = 1<<PushAndPullLayer; // PushAndPullLayer in bits

        #endregion

        #region Ground

        public static int GroundLayer { get; } = 7;
        public static int GroundLayerMask { get; } = 1<<GroundLayer; // GroundLayer in bits

        #endregion
        
    }
}