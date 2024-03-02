namespace RBS.Editor.Tools.Bases
{
    using UnityEditor;
    using RBS.Editor.Patterns.StateMachine;

    public abstract class ToolBase : RBSState
    {
        protected EditorWindow ParentWindow;

        public ToolBase(EditorWindow parentWindow, string stateKey) : base(stateKey)
        {
            ParentWindow = parentWindow;
        }
    }
}