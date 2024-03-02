namespace RBS.Editor.Tools
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using RBS.Room.Data;
    using RBS.Editor.Tools.Bases;
    using RBS.Editor.Utility;

    public class RoomsRegisterTool : ToolBase
    {
        private RBSRoomData[] _rooms;
        private RBSRoomSnap _roomSnap;
        private GameObject _selectedRoomPrefab;
        private bool _isRoomListShowed;
        private bool _isRoomSnapShowed;
        private bool _isRoomEntrancesShowed;

        public RoomsRegisterTool(EditorWindow parentWindow, string stateKey) : base(parentWindow, stateKey)
        {
            UpdateRooms();
        }

        public override void OnEnter()
        {
        }

        public override void OnExit()
        {
        }

        public override void OnProcess()
        {
            DrawFoldoutRooms();
            DrawAddRoomBox();
        }

        #region GUI

        private void DrawFoldoutRooms()
        {
            RBSEditorUtility.DrawBox(Color.black, () =>
            {
                RBSEditorUtility.DrawFoldout(ref _isRoomListShowed, "Rooms List", () =>
                {
                    foreach (RBSRoomData room in _rooms)
                    {
                        RBSEditorUtility.DrawHorizontal(() =>
                        {
                            RBSEditorUtility.DrawDisabledGroup(() =>
                            {
                                EditorGUILayout.ObjectField(room.RoomPrefab, typeof(GameObject), false);
                            });

                            if (GUILayout.Button("X", GUILayout.Width(20)))
                                DeleteRoom(room);
                        });
                    }
                });
            });
        }

        private void DrawAddRoomBox()
        {
            RBSEditorUtility.DrawBox(Color.black, () =>
            {
                EditorGUILayout.LabelField("Room Creator");
                _selectedRoomPrefab = (GameObject)EditorGUILayout.ObjectField(_selectedRoomPrefab, typeof(GameObject), false);

                bool _hasRoom = HasRoom(_selectedRoomPrefab);

                RBSEditorUtility.DrawDisabledGroup(() =>
                {
                    RBSEditorUtility.DrawIndented(1, () =>
                    {
                        RBSEditorUtility.DrawBox(Color.white, () =>
                        {
                            RBSEditorUtility.DrawFoldout(ref _isRoomSnapShowed, "Room Snap", () =>
                            {
                                _roomSnap.HasEntranceNorth = EditorGUILayout.Toggle("Has Entrance North", _roomSnap.HasEntranceNorth);
                                _roomSnap.HasEntranceSouth = EditorGUILayout.Toggle("Has Entrance South", _roomSnap.HasEntranceSouth);
                                _roomSnap.HasEntranceEast = EditorGUILayout.Toggle("Has Entrance East", _roomSnap.HasEntranceEast);
                                _roomSnap.HasEntranceWest = EditorGUILayout.Toggle("Has Entrance West", _roomSnap.HasEntranceWest);
                            });
                        });
                    });

                    if (GUILayout.Button("Add Room") && _selectedRoomPrefab)
                        CreateRoom();
                }, _hasRoom);

                if (_hasRoom)
                    EditorGUILayout.HelpBox("This room is already registered.", MessageType.Warning);
            });
        }

        #endregion GUI

        private void CreateRoom()
        {
            RBSIOUtility.CreateRoomAsset(_selectedRoomPrefab, _roomSnap);
            _selectedRoomPrefab = null;
            UpdateRooms();
        }

        private void DeleteRoom(RBSRoomData room)
        {
            RBSIOUtility.DeleteRoomAsset(room);
            UpdateRooms();
        }

        private void UpdateRooms()
        {
            _rooms = RBSIOUtility.GetRoomsData();
        }

        private bool HasRoom(GameObject roomPrefab)
        {
            foreach (RBSRoomData room in _rooms)
            {
                if (room.RoomPrefab == roomPrefab)
                    return true;
            }

            return false;
        }
    }
}