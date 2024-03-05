namespace RBS.Editor.Tools.Bases
{
    using UnityEditor;
    using RBS.Editor.Patterns.StateMachine;

    public abstract class RBSToolBase : RBSState
    {
        protected EditorWindow ParentWindow;

        public RBSToolBase(EditorWindow parentWindow, string stateKey) : base(stateKey)
        {
            ParentWindow = parentWindow;
        }
    }
}