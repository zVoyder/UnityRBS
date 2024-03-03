#if UNITY_EDITOR
namespace RBS.Runtime.Room.Data
{
    using System.Collections.Generic;
    using UnityEngine;

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