namespace RBS.Editor.Data
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using RBS.Runtime.Room.Data;
    using RBS.Runtime.Room;

    public class RBSPresetData : ScriptableObject
    {
        public List<RBSRoomInfo> RoomsList;

        public void Init(RBSRoom[] rooms)
        {
            RoomsList = new List<RBSRoomInfo>();
            foreach (RBSRoom room in rooms)
            {
                RoomsList.Add(new RBSRoomInfo
                {
                    RoomName = room.transform.name,
                    RoomData = room.RoomData,
                    Position = room.transform.position,
                    Rotation = room.transform.rotation
                });
            }
        }
    }
}