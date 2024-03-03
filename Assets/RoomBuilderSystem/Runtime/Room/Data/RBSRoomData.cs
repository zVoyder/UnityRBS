#if UNITY_EDITOR
namespace RBS.Runtime.Room.Data
{
    using System.Collections.Generic;
    using UnityEngine;
    using RBS.Runtime.Room.Data;

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