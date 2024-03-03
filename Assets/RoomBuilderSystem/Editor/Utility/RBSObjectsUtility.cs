namespace RBS.Editor.Utility
{
    using UnityEditor;
    using UnityEngine;
    using RBS.Runtime.Room;
    using RBS.Editor.Constants;
    using RBS.Runtime.Room.Data;

    public static class RBSObjectsUtility
    {
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

        public static void InstantiateRoom(RBSRoomData roomData, Vector3 position, Quaternion rotation)
        {
            GameObject roomGameObject = Object.Instantiate(roomData.RoomPrefab, position, rotation);
            roomGameObject.transform.SetParent(RBSObjectsUtility.GetRoomsParent());
            roomGameObject.name = roomData.RoomPrefab.name;
            RBSRoom room = roomGameObject.AddComponent<RBSRoom>();
            room.hideFlags = HideFlags.HideInInspector | HideFlags.DontSaveInBuild;
            room.Init(roomData);
            Undo.RegisterCreatedObjectUndo(roomGameObject, "Placed Room");
        }

        [MenuItem(RBSConstants.DeleteRoomsMenuItem, false, RBSConstants.RBSDeleteRoomsMenuItemPriority)]
        public static void DeleteRooms()
        {
            if (!EditorUtility.DisplayDialog("Delete All Rooms", "Are you sure you want to delete all rooms?", "Yes", "No")) return;

            GameObject rooms = GameObject.Find("Rooms");
            if (GameObject.Find("Rooms"))
                UnityEngine.Object.DestroyImmediate(rooms);
        }
    }
}