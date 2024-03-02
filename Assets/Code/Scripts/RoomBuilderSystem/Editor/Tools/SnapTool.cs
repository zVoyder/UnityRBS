namespace RBS.Editor.Tools
{
    using UnityEngine;
    using UnityEditor;
    using RBS.Editor.Tools.Bases;
    using Utility;

    public class SnapTool : ToolBase
    {
        public static bool EnableSnap = true;
        public static bool EnableSnapOnObjects = true;
        public static bool SnapOnlyOnFacingDoors = true;
        
        public SnapTool(EditorWindow parentWindow, string stateKey) : base(parentWindow, stateKey)
        {
        }
        
        public override void OnEnter()
        {
        }
        
        public override void OnExit()
        {
        }
        
        public override void OnProcess()
        {
            RBSEditorUtility.DrawBox(Color.black, () =>
            {
                EnableSnap = EditorGUILayout.Toggle("Enable Snap", EnableSnap);
                EnableSnapOnObjects = EditorGUILayout.Toggle("Enable Snap On Objects", EnableSnapOnObjects);
                SnapOnlyOnFacingDoors = EditorGUILayout.Toggle("Only Snap Doors", SnapOnlyOnFacingDoors);
            });
        }
    }
}