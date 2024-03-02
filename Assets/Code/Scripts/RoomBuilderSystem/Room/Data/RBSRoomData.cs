#if UNITY_EDITOR
namespace RBS.Room.Data
{
    using System.Collections.Generic;
    using UnityEngine;
    using RBS.Room.Data;

    public class RBSRoomData : ScriptableObject
    {
        public RBSRoomSnap RoomSnap;
        public GameObject RoomPrefab;
        
        public void Init(GameObject roomPrefab, RBSRoomSnap roomSnap)
        {
            RoomPrefab = roomPrefab;
            RoomSnap = roomSnap;
        }
    }
}
#endif