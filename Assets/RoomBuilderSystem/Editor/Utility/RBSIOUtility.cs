namespace RBS.Editor.Utility
{
    using System.Collections.Generic;
    using System.IO;
    using Data;
    using UnityEditor;
    using UnityEngine;
    using Config;
    using RBS.Runtime.Room.Data;
    using RBS.Runtime.Room;

    public static class RBSIOUtility
    {
        /// <summary>
        /// Creates a room asset from a room prefab and a room snap.
        /// </summary>
        /// <param name="roomPrefab"> The room prefab to create the asset from. </param>
        /// <param name="roomSnap"> The room snap to create the asset from. </param>
        public static void CreateRoomAsset(GameObject roomPrefab, RBSRoomSnap roomSnap)
        {
            RBSRoomData asset = ScriptableObject.CreateInstance<RBSRoomData>();
            asset.Init(roomPrefab, roomSnap);
            SaveAsset(asset, RBSConstants.RoomDataAssetPath, roomPrefab.name);
        }

        /// <summary>
        /// Creates a preset asset from a list of rooms and a preset name.
        /// </summary>
        /// <param name="rooms"> The list of rooms to create the asset from. </param>
        /// <param name="presetName"> The preset name to create the asset from. </param>
        public static void CreatePresetAsset(RBSRoom[] rooms, string presetName)
        {
            RBSPresetData asset = ScriptableObject.CreateInstance<RBSPresetData>();
            asset.Init(rooms);
            SaveAsset(asset, RBSConstants.PresetsDataAssetPath, presetName);
        }
        
        /// <summary>
        /// Saves an asset to a specific path with a specific name.
        /// </summary>
        /// <param name="asset"> The asset to save. </param>
        /// <param name="path"> The path to save the asset to. </param>
        /// <param name="assetName"> The name of the asset to save. </param>
        public static void SaveAsset(ScriptableObject asset, string path, string assetName)
        {
            if(!Directory.Exists(path))
                Directory.CreateDirectory(path);
            
            string fullPath = AssetDatabase.GenerateUniqueAssetPath(path + assetName + ".asset");
            AssetDatabase.CreateAsset(asset, fullPath);
            EditorUtility.SetDirty(asset);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// Deletes an asset from the project.
        /// </summary>
        /// <param name="asset"> The asset to delete. </param>
        public static void DeleteAsset(ScriptableObject asset)
        {
            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(asset));
            AssetDatabase.Refresh();
        }
        
        /// <summary>
        /// Gets all the room data assets in the project.
        /// </summary>
        /// <returns></returns>
        public static RBSRoomData[] GetRoomsData()
        {
            return Resources.LoadAll<RBSRoomData>("");
        }
        
        /// <summary>
        /// Gets all the preset data assets in the project.
        /// </summary>
        /// <returns></returns>
        public static RBSPresetData[] GetPresetsData()
        {
            return Resources.LoadAll<RBSPresetData>("");
        }
        
        /// <summary>
        /// Checks if a string is valid for an asset name.
        /// </summary>
        /// <param name="name"> The name to check. </param>
        /// <returns> True if the name is valid, false otherwise. </returns>
        public static bool IsStringValidForAssetName(string name)
        {
            return !string.IsNullOrEmpty(name) && !string.IsNullOrWhiteSpace(name) && !name.Contains(" ") && !name.Contains("/");
        }
    }
}