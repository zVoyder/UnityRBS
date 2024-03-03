namespace RBS.Editor.Utility
{
    using System.Collections.Generic;
    using Data;
    using UnityEditor;
    using UnityEngine;
    using RBS.Editor.Constants;
    using RBS.Runtime.Room.Data;
    using RBS.Runtime.Room;

    public static class RBSIOUtility
    {
        public static void CreateRoomAsset(GameObject roomPrefab, RBSRoomSnap roomSnap)
        {
            RBSRoomData asset = ScriptableObject.CreateInstance<RBSRoomData>();
            asset.Init(roomPrefab, roomSnap);
            SaveAsset(asset, RBSConstants.RoomDataAssetPath, roomPrefab.name);
        }

        public static void CreatePresetAsset(RBSRoom[] rooms, string presetName)
        {
            RBSPresetData asset = ScriptableObject.CreateInstance<RBSPresetData>();
            asset.Init(rooms);
            SaveAsset(asset, RBSConstants.PresetsDataAssetPath, presetName);
        }
        
        public static void SaveAsset(ScriptableObject asset, string path, string assetName)
        {
            string fullPath = AssetDatabase.GenerateUniqueAssetPath(path + assetName + ".asset");
            AssetDatabase.CreateAsset(asset, fullPath);
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        public static void DeleteAsset(ScriptableObject asset)
        {
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(asset));
            AssetDatabase.Refresh();
        }
        
        public static RBSRoomData[] GetRoomsData()
        {
            return Resources.LoadAll<RBSRoomData>("");
        }
        
        public static RBSPresetData[] GetPresetsData()
        {
            return Resources.LoadAll<RBSPresetData>("");
        }
        
        public static bool IsStringValidForAssetName(string name)
        {
            return !string.IsNullOrEmpty(name) && !string.IsNullOrWhiteSpace(name) && !name.Contains(" ") && !name.Contains("/");
        }
    }
}