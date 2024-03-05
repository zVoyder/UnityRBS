namespace RBS.Editor.Config
{
    using UnityEngine;
    using UnityEditor;

    public static class RBSPrefs
    {
        public const string SnapSettingsPref = "RBS_SnapSettings";
        public const string PlaceHeightPref = "RBS_PlaceHeight";
        public const string GridSnapSizePref = "RBS_GridSnapSize";
        public const string HasGridSnapPref = "RBS_GridSnap";
        public const string HasInstantiateSamePresetWarningPref = "RBS_HasInstantiateSamePresetWarningPref";
        
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
        
        public static float GridSnapSize
        {
            get => EditorPrefs.GetFloat(GridSnapSizePref, 10f);
            set => EditorPrefs.SetFloat(GridSnapSizePref, value);
        }
        
        public static bool HasGridSnap
        {
            get => EditorPrefs.GetBool(HasGridSnapPref, false);
            set => EditorPrefs.SetBool(HasGridSnapPref, value);
        }
        
        public static bool HasInstantiateSamePresetWarning
        {
            get => EditorPrefs.GetBool(HasInstantiateSamePresetWarningPref, false);
            set => EditorPrefs.SetBool(HasInstantiateSamePresetWarningPref, value);
        }
    }
}