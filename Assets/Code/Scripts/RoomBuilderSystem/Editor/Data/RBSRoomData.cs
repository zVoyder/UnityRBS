namespace RBS.Editor.Data
{
    using System.Collections.Generic;
    using UnityEngine;
    using RBS.Room.Data;

    public class RBSRoomData : ScriptableObject
    {
        public RBSRoomSnap RoomSnap;
        public GameObject RoomPrefab;
        public List<Vector3> EntrancePositions;
        
        public void Init(GameObject roomPrefab, RBSRoomSnap roomSnap, List<Vector3> entrancePositions)
        {
            RoomPrefab = roomPrefab;
            RoomSnap = roomSnap;
            EntrancePositions = entrancePositions;
        }
    }
}