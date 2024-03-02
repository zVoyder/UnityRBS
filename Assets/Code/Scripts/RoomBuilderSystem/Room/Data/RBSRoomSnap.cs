#if UNITY_EDITOR
namespace RBS.Room.Data
{
    using UnityEngine;
    using System.Collections.Generic;

    [System.Serializable]
    public struct RBSRoomSnap
    {
        public bool HasEntranceNorth;
        public bool HasEntranceEast;
        public bool HasEntranceSouth;
        public bool HasEntranceWest;
    }
}
#endif