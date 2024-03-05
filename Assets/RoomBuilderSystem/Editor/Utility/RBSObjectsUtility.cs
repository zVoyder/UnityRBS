namespace RBS.Editor.Utility
{
    using UnityEditor;
    using UnityEngine;
    using RBS.Runtime.Room;
    using Config;
    using RBS.Runtime.Room.Data;

    public static class RBSObjectsUtility
    {
        /// <summary>
        /// Gets the rooms parent object.
        /// </summary>
        /// <returns> The rooms parent object. </returns>
        public static Transform GetRoomsParent()
        {
            GameObject rooms = GameObject.Find(RBSConstants.RoomsParentName);

            if (!rooms)
            {
                rooms = new GameObject(RBSConstants.RoomsParentName)
                {
                    transform =
                    {
                        position = Vector3.zero
                    }
                };
            }

            return rooms.transform;
        }

        /// <summary>
        /// Instantiates a room from a room data, position and rotation.
        /// </summary>
        /// <param name="roomData"> The room data to instantiate the room from. </param>
        /// <param name="position"> The position to instantiate the room at. </param>
        /// <param name="rotation"> The rotation to instantiate the room with. </param>
        public static void InstantiateRoom(RBSRoomData roomData, Vector3 position, Quaternion rotation)
        {
            GameObject roomGameObject = Object.Instantiate(roomData.RoomPrefab);
            roomGameObject.transform.SetParent(RBSObjectsUtility.GetRoomsParent());
            roomGameObject.name = roomData.RoomPrefab.name;
            RBSRoom room = roomGameObject.AddComponent<RBSRoom>();
            room.hideFlags = HideFlags.HideInInspector | HideFlags.DontSaveInBuild;
            room.Init(roomData, position, rotation);
            Undo.RegisterCreatedObjectUndo(roomGameObject, "Placed Room");
        }

        /// <summary>
        /// Deletes all rooms from the scene.
        /// </summary>
        [MenuItem(RBSConstants.DeleteRoomsMenuItem, false, RBSConstants.RBSDeleteRoomsMenuItemPriority)]
        public static void DeleteRooms()
        {
            if (!EditorUtility.DisplayDialog("Delete All Rooms", "Are you sure you want to delete all rooms?", "Yes", "No")) return;

            GameObject rooms = GameObject.Find("Rooms");
            if (GameObject.Find("Rooms"))
            {
                UnityEngine.Object.DestroyImmediate(rooms);
                Undo.ClearUndo(rooms);
            }
        }
    }
}