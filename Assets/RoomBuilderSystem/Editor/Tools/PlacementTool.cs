namespace RBS.Editor.Tools
{
    using UnityEditor;
    using UnityEngine;
    using RBS.Runtime.Room;
    using RBS.Runtime.Room.Data;
    using RBS.Editor.Tools.Bases;
    using RBS.Editor.Tools.Elements;
    using RBS.Editor.Utility;
    using static RBS.Editor.Utility.RBSPrefs;

    public class PlacementTool : RBSToolBase
    {
        private static bool s_canPlace = true;

        private int _selectedRoomIndex;
        private RBSPreviewDragger _previewDragger;
        private RBSRoomData[] _rooms;
        private Vector2 _scrollPosition;
        private Rect _selectorRect = new Rect(20, 20, 200, 400);

        private RBSRoomData CurrentRoomData => _rooms[_selectedRoomIndex];

        public PlacementTool(EditorWindow parentWindow, string stateKey) : base(parentWindow, stateKey)
        {
            _previewDragger = new RBSPreviewDragger();
        }

        public override void OnEnter()
        {
            UpdateRoomsData();
            SceneView.duringSceneGui += OnSceneGUI;

            RBSInputs.OnLeftMouseButtonDown += PlacePrefab;
            RBSInputs.OnShiftScrollWheel += RotatePreview;
            RBSInputs.OnShiftAltScrollWheel += AddHeight;
            RBSInputs.OnShiftRKeyDown += ResetPreviewRotation;
            RBSInputs.OnShiftFKeyDow += ResetHeightPreview;
            RBSInputs.OnSpaceKeyDown += ToggleCanPlace;
        }

        public override void OnExit()
        {
            _previewDragger.DisableDrag();
            SceneView.duringSceneGui -= OnSceneGUI;

            RBSInputs.OnLeftMouseButtonDown -= PlacePrefab;
            RBSInputs.OnShiftScrollWheel -= RotatePreview;
            RBSInputs.OnShiftAltScrollWheel -= AddHeight;
            RBSInputs.OnShiftRKeyDown -= ResetPreviewRotation;
            RBSInputs.OnShiftFKeyDow -= ResetHeightPreview;
            RBSInputs.OnSpaceKeyDown -= ToggleCanPlace;
        }

        public override void OnProcess()
        {
            RBSEditorUtility.DrawBox(Color.black, () =>
            {
                DrawFields();
                RBSEditorUtility.DrawSpace(10);
                DrawButtons();
            });

            DrawHelpBox();
        }

        private void OnSceneGUI(SceneView view)
        {
            RBSInputs.Process();

            if (s_canPlace && _selectedRoomIndex >= 0)
            {
                DrawSelector();
                _previewDragger.EnableDrag();
                _previewDragger.Drag(view.camera, PlaceHeight);
            }
            else
            {
                _previewDragger.DisableDrag();
            }
        }

        #region GUI

        private void DrawFields()
        {
            s_canPlace = EditorGUILayout.Toggle("Enable Place", s_canPlace);
            RotationStep = EditorGUILayout.FloatField("Rotation Step", RotationStep);
            PlaceHeight = EditorGUILayout.FloatField("Place Height", PlaceHeight);
        }

        private void DrawButtons()
        {
            if (GUILayout.Button("Delete All Rooms"))
                DeleteRooms();
        }

        private void DrawHelpBox()
        {
            RBSEditorUtility.DrawSpace(10);
            EditorGUILayout.HelpBox(
                "Controls:\n" +
                "    LMB -> Place Room.\n" +
                "    SHIFT + MOUSE WHEEL -> Rotate Room.\n" +
                "    SHIFT + ALT + MOUSE WHEEL -> Change RBSRoom Height.\n" +
                "    SHIFT + R -> Reset Rotation.\n" +
                "    SHIFT + F -> Reset Room Height.", MessageType.Info);
        }

        private void DrawSelector()
        {
            Handles.BeginGUI();
            GUILayout.Window(0, _selectorRect, (id) =>
            {
                _scrollPosition = GUILayout.BeginScrollView(_scrollPosition);
                for (int i = 0; i < _rooms.Length; i++)
                {
                    Texture icon = AssetPreview.GetAssetPreview(_rooms[i].RoomPrefab);
                    GUI.backgroundColor = i == _selectedRoomIndex ? Color.gray : Color.white;

                    if (GUILayout.Button(icon))
                        SelectPrefabPreview(i);
                }

                GUILayout.EndScrollView();
            }, "Room Selector", GUILayout.MinWidth(100), GUILayout.MinHeight(100), GUILayout.MaxWidth(400), GUILayout.MaxHeight(600));
            Handles.EndGUI();
        }

        #endregion GUI

        private void AddHeight(float value)
        {
            SetHeight(RBSPrefs.PlaceHeight + value);
        }

        public void SetHeight(float value)
        {
            PlaceHeight = value;
            ParentWindow.Repaint(); // Draw the window again to update the value
        }

        private void ToggleCanPlace()
        {
            s_canPlace = !s_canPlace;
            ParentWindow.Repaint();
        }

        private void DeleteRooms()
        {
            RBSObjectsUtility.DeleteRooms();
        }

        private void PlacePrefab(Vector2 screenMousePosition)
        {
            if (_previewDragger.CanPlacePrefab(out Vector3 position, out Quaternion rotation))
                RBSObjectsUtility.InstantiateRoom(CurrentRoomData, position, rotation);
        }

        private void RotatePreview(float scrollDirection)
        {
            _previewDragger.RotatePreview(scrollDirection * RotationStep);
        }

        private void ResetHeightPreview()
        {
            SetHeight(0f);
        }

        private void ResetPreviewRotation()
        {
            _previewDragger.SetPreviewRotation(Quaternion.identity);
        }

        private void SelectPrefabPreview(int index)
        {
            if (index < 0 || index >= _rooms.Length)
            {
                _selectedRoomIndex = -1;
                _previewDragger.DisableDrag();
                return;
            }

            _selectedRoomIndex = index;
            _previewDragger.SetAsPreview(CurrentRoomData);
        }

        private void UpdateRoomsData()
        {
            _rooms = RBSIOUtility.GetRoomsData();
            SelectPrefabPreview(0);
        }
    }
}