namespace RBS.Editor.Tools
{
    using UnityEditor;
    using UnityEngine;
    using RBS.Editor.Tools.Bases;
    using RBS.Editor.Utility;
    using RBS.Editor.Data;
    using RBS.Runtime.Room;
    using RBS.Runtime.Room.Data;

    public class PresetsTool : RBSToolBase
    {
        private RBSPresetData[] _presets;
        private string _presetName;
        private bool _isPresetsListShowed;

        public PresetsTool(EditorWindow parentWindow, string stateKey) : base(parentWindow, stateKey)
        {
        }

        public override void OnEnter()
        {
            UpdatePresets();
            GuessPresetName();
        }

        public override void OnExit()
        {
        }

        public override void OnProcess()
        {
            DrawFields();
        }

        #region GUI

        private void DrawFields()
        {
            RBSEditorUtility.DrawBox(Color.black, () =>
            {
                RBSPrefs.HasInstantiateSamePresetWarning = EditorGUILayout.Toggle("Enable Preset Warning", RBSPrefs.HasInstantiateSamePresetWarning);
                DrawPresetsList();
                DrawPresetCreator();
            });
        }

        private void DrawPresetsList()
        {
            RBSEditorUtility.DrawBox(Color.white, () =>
            {
                RBSEditorUtility.DrawFoldout(ref _isPresetsListShowed, "Presets List", () =>
                {
                    foreach (RBSPresetData preset in _presets)
                    {
                        RBSEditorUtility.DrawHorizontal(() =>
                        {
                            RBSEditorUtility.DrawDisabledGroup(() =>
                            {
                                EditorGUILayout.ObjectField(preset, typeof(RBSPresetData), false);
                            });

                            if (GUILayout.Button("\u2191", GUILayout.Width(20)))
                                LoadPreset(preset);

                            if (GUILayout.Button("X", GUILayout.Width(20)))
                                DeletePreset(preset);
                        });
                    }
                });
            });
        }

        private void DrawPresetCreator()
        {
            RBSEditorUtility.DrawBox(Color.white, () =>
            {
                _presetName = EditorGUILayout.TextField("Preset Name", _presetName);

                bool canCreatePreset = RBSIOUtility.IsStringValidForAssetName(_presetName);
                RBSEditorUtility.DrawDisabledGroup(() =>
                {
                    if (GUILayout.Button("Save Preset"))
                        CreatePreset();
                }, !canCreatePreset);

                if (!canCreatePreset)
                    EditorGUILayout.HelpBox("Preset name not valid.", MessageType.Warning);
            });
        }

        #endregion

        private void CreatePreset()
        {
            RBSRoom[] rooms = Object.FindObjectsOfType<RBSRoom>();
            if (rooms != null && rooms.Length > 0)
            {
                RBSIOUtility.CreatePresetAsset(rooms, _presetName);
                UpdatePresets();
            }
        }

        private void DeletePreset(RBSPresetData preset)
        {
            RBSIOUtility.DeleteAsset(preset);
            UpdatePresets();
        }

        private void LoadPreset(RBSPresetData preset)
        {
            if (RBSPrefs.HasInstantiateSamePresetWarning)
            {
                if (CheckIfPresetIsInScene(preset))
                {
                    if (!EditorUtility.DisplayDialog(
                            "Preset Detected in Scene",
                            "The selected preset is already present in the current scene. Do you wish to instantiate it again?",
                            "Yes", "No"))
                    {
                        return;
                    }
                }
            }

            foreach (RBSRoomInfo roomInfo in preset.RoomsList)
            {
                if (roomInfo.RoomData == null)
                {
                    Debug.LogWarning($"Room {roomInfo.RoomName} not found. Skipping room instantiation.");
                    continue;
                }

                RBSObjectsUtility.InstantiateRoom(roomInfo.RoomData, roomInfo.Position, roomInfo.Rotation);
            }
        }

        private bool CheckIfPresetIsInScene(RBSPresetData preset)
        {
            RBSRoom[] rooms = Object.FindObjectsOfType<RBSRoom>();

            foreach (RBSRoomInfo room in preset.RoomsList)
            {
                if (!IsRoomIn(rooms, room))
                    return false;
            }

            return true;
        }

        private bool IsRoomIn(RBSRoom[] rooms, RBSRoomInfo info)
        {
            foreach (RBSRoom room in rooms)
            {
                if (room.RoomData == info.RoomData && room.transform.position == info.Position && room.transform.rotation == info.Rotation)
                    return true;
            }

            return false;
        }

        private void UpdatePresets()
        {
            _presets = RBSIOUtility.GetPresetsData();
        }

        private bool HasPreset(string presetName)
        {
            foreach (RBSPresetData preset in _presets)
            {
                if (preset.name == presetName)
                    return true;
            }

            return false;
        }

        private void GuessPresetName()
        {
            RBSRoom room = Object.FindObjectOfType<RBSRoom>();
            string roomName = room != null ? $"[{room.name}]" : "";
            _presetName = $"Preset_{roomName}_{_presets.Length + 1}";
        }
    }
}