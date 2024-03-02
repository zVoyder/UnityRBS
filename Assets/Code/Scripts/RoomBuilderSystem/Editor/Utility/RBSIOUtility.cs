namespace RBS.Editor.Utility
{
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;
    using RBS.Editor.Constants;
    using RBS.Room.Data;

    public static class RBSIOUtility
    {
        public static void CreateRoomAsset(GameObject roomPrefab, RBSRoomSnap roomSnap)
        {
            RBSRoomData asset = ScriptableObject.CreateInstance<RBSRoomData>();
            asset.Init(roomPrefab, roomSnap);
            SaveAsset(asset, RBSConstants.RoomDataAssetPath, roomPrefab.name);
        }

        public static void SaveAsset(ScriptableObject asset, string path, string assetName)
        {
            string fullPath = AssetDatabase.GenerateUniqueAssetPath(path + assetName + ".asset");
            AssetDatabase.CreateAsset(asset, fullPath);
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void DeleteRoomAsset(RBSRoomData roomData)
        {
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(roomData));
            AssetDatabase.Refresh();
        }

        public static RBSRoomData[] GetRoomsData()
        {
            return Resources.LoadAll<RBSRoomData>("");
        }
    }
}