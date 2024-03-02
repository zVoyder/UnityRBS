namespace RBS.Editor.Utility
{
    using UnityEngine;
    using UnityEditor;

    public static class RBSPrefs
    {
        public const string SnapSettingsPref = "RBS_SnapSettings";
        public const string PlaceHeightPref = "RBS_PlaceHeight";
        public const string GridSnapPref = "RBS_GridSnap";
        public const string GridSnapSizePref = "RBS_GridSnapSize";
        
        public static float RotationStep
        {
            get => EditorPrefs.GetFloat(SnapSettingsPref, 90f);
            set => EditorPrefs.SetFloat(SnapSettingsPref, value);
        }
        
        public static float PlaceHeight
        {
            get => EditorPrefs.GetFloat(PlaceHeightPref, 0f);
            set => EditorPrefs.SetFloat(PlaceHeightPref, value);
        }
        
        public static bool GridSnap
        {
            get => EditorPrefs.GetBool(GridSnapPref, false);
            set => EditorPrefs.SetBool(GridSnapPref, value);
        }
        
        public static float GridSnapSize
        {
            get => EditorPrefs.GetFloat(GridSnapSizePref, 10f);
            set => EditorPrefs.SetFloat(GridSnapSizePref, value);
        }
    }
}