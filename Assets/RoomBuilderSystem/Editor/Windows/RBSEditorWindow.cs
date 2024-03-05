namespace RBS.Editor.Windows
{
    using UnityEditor;
    using UnityEditor.SceneManagement;
    using UnityEngine;
    using Config;
    using RBS.Editor.Utility;
    using RBS.Editor.Patterns.StateMachine;
    using RBS.Editor.Tools;
    using UnityEngine.SceneManagement;

    public class RBSEditorWindow : EditorWindow
    {
        private static RBSEditorWindow s_window;

        private int _selectedTool;
        private RBSStateMachine _editorMachine;
        private RoomsRegisterTool _roomsRegisterTool;
        private PlacementTool _placementTool;
        private SnapTool _snapTool;
        private PresetsTool _presetsTool;

        /// <summary>
        /// Opens the editor window.
        /// </summary>
        [MenuItem(RBSConstants.MainWindowMenuItem, false, RBSConstants.RBSWindowMenuItemPriority)]
        public static void OpenWindow()
        {
            s_window = GetWindow<RBSEditorWindow>();
            s_window.titleContent = new GUIContent(RBSConstants.MainWindowName);
            s_window.Show();
        }

        /// <summary>
        /// Closes the editor window.
        /// </summary>
        public static void CloseWindow()
        {
            if (s_window)
                s_window.Close();
        }
        
        /// <summary>
        /// Restarts the editor window.
        /// </summary>
        public static void RestartWindow()
        {
            CloseWindow();
            OpenWindow();
        }

        private void Awake()
        {
            InitMachine();
        }

        /// <summary>
        /// Initializes the state machine with its states.
        /// </summary>
        private void InitMachine()
        {
            _editorMachine = new RBSStateMachine();
            _placementTool = new PlacementTool(this, RBSConstants.PlacementToolName);
            _roomsRegisterTool = new RoomsRegisterTool(this, RBSConstants.RoomToolName);
            _snapTool = new SnapTool(this, RBSConstants.SnapToolName);
            _presetsTool = new PresetsTool(this, RBSConstants.PresetsToolName);

            _editorMachine.States.Add(RBSConstants.PlacementToolName, _placementTool);
            _editorMachine.States.Add(RBSConstants.RoomToolName, _roomsRegisterTool);
            _editorMachine.States.Add(RBSConstants.SnapToolName, _snapTool);
            _editorMachine.States.Add(RBSConstants.PresetsToolName, _presetsTool);
        }

        private void OnEnable()
        {
            EditorSceneManager.sceneOpened += OnSceneOpened;
            _editorMachine.Start();
        }

        private void OnDisable()
        {
            EditorSceneManager.sceneOpened -= OnSceneOpened;
            _editorMachine.Stop();
        }

        private void OnSceneOpened(Scene scene, OpenSceneMode mode)
        {
            RestartWindow();
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
                    _editorMachine.ChangeState(RBSConstants.PresetsToolName);
                    break;
            }

// #if DEBUG
//             if (GUILayout.Button("Delete Hide Previews"))
//             {
//                 var allObjects = FindObjectsOfType<GameObject>();
//
//                 foreach (var obj in allObjects)
//                 {
//                     if (obj.hideFlags == HideFlags.HideInHierarchy)
//                     {
//                         Debug.Log("Destroying: " + obj.name);
//                         DestroyImmediate(obj);
//                     }
//                 }
//             }
// #endif
        }
    }
}