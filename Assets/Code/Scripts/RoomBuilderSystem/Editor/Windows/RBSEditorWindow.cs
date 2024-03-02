namespace RBS.Editor.Windows
{
    using UnityEditor;
    using UnityEngine;
    using RBS.Editor.Constants;
    using RBS.Editor.Utility;
    using RBS.Editor.Patterns.StateMachine;
    using RBS.Editor.Tools;

    public class RBSEditorWindow : EditorWindow
    {
        private static RBSEditorWindow s_window;

        private int _selectedTool;

        private RBSStateMachine _editorMachine;
        private RoomsRegisterTool _roomsRegisterTool;
        private PlacementTool _placement;
        private SnapTool _snapTool;

        [MenuItem(RBSConstants.MainWindowMenuItem, false, RBSConstants.RBSWindowMenuItemPriority)]
        public static void OpenWindow()
        {
            s_window = GetWindow<RBSEditorWindow>();
            s_window.titleContent = new GUIContent(RBSConstants.MainWindowName);
            s_window.Show();
        }

        private void Awake()
        {
            InitMachine();
        }

        private void InitMachine()
        {
            _editorMachine = new RBSStateMachine();
            _placement = new PlacementTool(this, RBSConstants.PlacementToolName);
            _roomsRegisterTool = new RoomsRegisterTool(this, RBSConstants.RoomToolName);
            _snapTool = new SnapTool(this, RBSConstants.SnapToolName);
            
            _editorMachine.States.Add(RBSConstants.PlacementToolName, _placement);
            _editorMachine.States.Add(RBSConstants.RoomToolName, _roomsRegisterTool);
            _editorMachine.States.Add(RBSConstants.SnapToolName, _snapTool);
        }

        private void OnEnable()
        {
            _editorMachine.Start();
        }

        private void OnDisable()
        {
            _editorMachine.Stop();
        }

        private void OnGUI()
        {
            RBSEditorUtility.DrawSelectionGrid(this, RBSConstants.ToolNames, ref _selectedTool);
            _editorMachine.Update();

            switch (_selectedTool)
            {
                case 0:
                    _editorMachine.ChangeState(RBSConstants.PlacementToolName);
                    break;

                case 1:
                    _editorMachine.ChangeState(RBSConstants.RoomToolName);
                    break;

                case 2:
                    _editorMachine.ChangeState(RBSConstants.SnapToolName);
                    break;

                case 3:
                    //_editorMachine.ChangeState(RBSConstants.PresetsToolName);
                    break;
            }

#if DEBUG
            if (GUILayout.Button("Delete Hide Previews"))
            {
                var allObjects = FindObjectsOfType<GameObject>();

                foreach (var obj in allObjects)
                {
                    if (obj.hideFlags == HideFlags.HideInHierarchy)
                    {
                        Debug.Log("Destroying: " + obj.name);
                        DestroyImmediate(obj);
                    }
                }
            }
#endif
        }
    }
}