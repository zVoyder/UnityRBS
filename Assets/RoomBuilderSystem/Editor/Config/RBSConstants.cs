namespace RBS.Editor.Config
{
    public static class RBSConstants
    {
        #region PATHS
        public const string RoomDataAssetPath = "Assets/Resources/Rooms/";
        public const string PresetsDataAssetPath = "Assets/Resources/Presets/";
        #endregion

        #region TOOLS NAMES
        public const string PlacementToolName = "Placement";
        public const string RoomToolName = "Rooms List";
        public const string SnapToolName = "Snap";
        public const string PresetsToolName = "Presets";
        public static readonly string[] ToolNames = new string[] { PlacementToolName, RoomToolName, SnapToolName, PresetsToolName };
        #endregion

        #region OBJECTS NAMES
        public const string RoomsParentName = "Rooms";
        #endregion

        #region WINDOWS NAMES
        public const string MainWindowName = "RoomBuilder";
        #endregion

        #region MenuItems
        public const string MainMenuItem = "Tools/" + MainWindowName + "/";
        public const string ActionsMenuItem = MainMenuItem + "Actions/";
        public const string MainWindowMenuItem = MainMenuItem + MainWindowName + " Window";
        public const string DeleteRoomsMenuItem = ActionsMenuItem + "Delete All Rooms";
        public const int RBSWindowMenuItemPriority = 0;
        public const int RBSDeleteRoomsMenuItemPriority = 11;
        #endregion
    }
}
