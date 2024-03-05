namespace RBS.Editor.Tools
{
    using System;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using RBS.Runtime.Room.Data;
    using RBS.Editor.Tools.Bases;
    using RBS.Editor.Utility;

    [Flags]
    public enum RoomCheckFlags
    {
        Valid = 0,
        NoPrefab = 1 << 0,
        RoomExists = 1 << 1,
        NoFloor = 1 << 2
    }

    public class RoomsRegisterTool : RBSToolBase
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
            DrawRoomsList();
            DrawRoomCreator();
        }

        #region GUI

        /// <summary>
        /// Draws the rooms list.
        /// </summary>
        private void DrawRoomsList()
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

        /// <summary>
        /// Draws the room creator.
        /// </summary>
        private void DrawRoomCreator()
        {
            RBSEditorUtility.DrawBox(Color.black, () =>
            {
                EditorGUILayout.LabelField("Room Creator");
                _selectedRoomPrefab = (GameObject)EditorGUILayout.ObjectField(_selectedRoomPrefab, typeof(GameObject), false);

                RoomCheckFlags roomCheckFlags = RoomCheckFlags.Valid;
                roomCheckFlags = CheckRoom(roomCheckFlags);

                bool isInvalid = roomCheckFlags != RoomCheckFlags.Valid;

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

                    if (GUILayout.Button("Add Room"))
                        CreateRoom();
                }, isInvalid);

                DrawErrors(roomCheckFlags);
            });
        }
        
        /// <summary>
        /// Draws the room flagged errors.
        /// </summary>
        /// <param name="roomCheckFlags"> The room check flags. </param>
        private void DrawErrors(RoomCheckFlags roomCheckFlags)
        {
            if ((roomCheckFlags & RoomCheckFlags.NoPrefab) != 0)
                EditorGUILayout.HelpBox("Select a room prefab to register.", MessageType.Warning);
            
            if ((roomCheckFlags & RoomCheckFlags.RoomExists) != 0)
                EditorGUILayout.HelpBox("This room is already registered.", MessageType.Error);
            
            if ((roomCheckFlags & RoomCheckFlags.NoFloor) != 0)
                EditorGUILayout.HelpBox("This room does not have a collider set as the floor. Ensure that the floor's collider is set on the root of the room's game object.", MessageType.Error);
        }

        #endregion GUI

        /// <summary>
        /// Creates a room asset.
        /// </summary>
        private void CreateRoom()
        {
            RBSIOUtility.CreateRoomAsset(_selectedRoomPrefab, _roomSnap);
            _selectedRoomPrefab = null;
            UpdateRooms();
        }

        /// <summary>
        /// Deletes a room asset.
        /// </summary>
        /// <param name="room"> The room to delete. </param>
        private void DeleteRoom(RBSRoomData room)
        {
            RBSIOUtility.DeleteAsset(room);
            UpdateRooms();
        }

        /// <summary>
        /// Updates the rooms list.
        /// </summary>
        private void UpdateRooms()
        {
            _rooms = RBSIOUtility.GetRoomsData();
        }

        /// <summary>
        /// Checks the room for errors.
        /// </summary>
        /// <param name="roomCheckFlags"> The room check flags. </param>
        /// <returns> The room check flags. </returns>
        private RoomCheckFlags CheckRoom(RoomCheckFlags roomCheckFlags)
        {
            if (_selectedRoomPrefab == null)
            {
                roomCheckFlags |= RoomCheckFlags.NoPrefab;
                return roomCheckFlags;
            }

            if (HasRoom(_selectedRoomPrefab))
                roomCheckFlags |= RoomCheckFlags.RoomExists;

            if (!HasFloor(_selectedRoomPrefab))
                roomCheckFlags |= RoomCheckFlags.NoFloor;

            return roomCheckFlags;
        }

        /// <summary>
        /// Checks if the room is already registered by the room prefab name.
        /// </summary>
        /// <param name="roomPrefab"> The room prefab to check. </param>
        /// <returns> True if the room is already registered, false otherwise. </returns>
        private bool HasRoom(GameObject roomPrefab)
        {
            foreach (RBSRoomData room in _rooms)
            {
                if (room.RoomPrefab == roomPrefab)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if the room prefab has a floor collider. (The floor collider is the collider set on the root of the room's game object.)
        /// </summary>
        /// <param name="roomPrefab"> The room prefab to check. </param>
        /// <returns> True if the room prefab has a floor, false otherwise. </returns>
        private bool HasFloor(GameObject roomPrefab)
        {
            return roomPrefab.TryGetComponent(out Collider collider);
        }
    }
}