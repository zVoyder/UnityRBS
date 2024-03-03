namespace RBS.Editor.Data
{
    using UnityEngine;
    using RBS.Runtime.Room.Data;

    [System.Serializable]
    public struct RBSRoomInfo
    {
        public string RoomName;
        public RBSRoomData RoomData;
        public Vector3 Position;
        public Quaternion Rotation;
    }
}