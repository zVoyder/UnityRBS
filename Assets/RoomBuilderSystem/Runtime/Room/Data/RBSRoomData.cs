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
        
        /// <summary>
        /// Initializes the room data.
        /// </summary>
        /// <param name="roomPrefab"> The room prefab to initialize the room data with. </param>
        /// <param name="roomSnap"> The room snap to initialize the room data with. </param>
        public void Init(GameObject roomPrefab, RBSRoomSnap roomSnap)
        {
            RoomPrefab = roomPrefab;
            RoomSnap = roomSnap;
        }
    }
}
#endif