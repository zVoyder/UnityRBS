namespace RBS.Editor.Utility
{
    using UnityEditor;
    using UnityEngine;
    using RBS.Editor.Constants;

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

        [MenuItem(RBSConstants.DeleteRoomsMenuItem, false, RBSConstants.RBSDeleteRoomsMenuItemPriority)]
        public static void DeleteRooms()
        {
            if (!EditorUtility.DisplayDialog("Delete All Rooms", "Are you sure you want to delete all rooms?", "Yes", "No")) return;

            GameObject rooms = GameObject.Find("Rooms");
            if(GameObject.Find("Rooms"))
                UnityEngine.Object.DestroyImmediate(rooms);
        }
    }
}
