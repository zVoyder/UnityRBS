#if UNITY_EDITOR
namespace RBS.Runtime.Room
{
    using UnityEngine;
    using RBS.Runtime.Room.Data;
    using Unity.VisualScripting;
    using UnityEngine.Serialization;

    public class RBSRoom : MonoBehaviour
    {
        private Collider _collider;
        
        public RBSRoomData RoomData { get; private set; }
        public RBSRoomSnap RoomSnap => RoomData.RoomSnap;

        public void Init(RBSRoomData roomData)
        {
            RoomData = roomData;
            if (TryGetComponent(out _collider))
            {
                GenerateEntrances();
            }
        }

        private void GenerateEntrances()
        {
            Vector3 center = transform.position;

            if (RoomSnap.HasEntranceNorth)
                GenerateEntrance(center + transform.lossyScale.z / 2f * transform.forward, RBSEntranceType.North);

            if (RoomSnap.HasEntranceSouth)
                GenerateEntrance(center - transform.lossyScale.z / 2f * transform.forward, RBSEntranceType.South);

            if (RoomSnap.HasEntranceEast)
                GenerateEntrance(center + transform.lossyScale.x / 2f * transform.right, RBSEntranceType.East);

            if (RoomSnap.HasEntranceWest)
                GenerateEntrance(center - transform.lossyScale.x / 2f * transform.right, RBSEntranceType.West);
        }

        private void GenerateEntrance(Vector3 position, RBSEntranceType entranceType)
        {
            GameObject entrGO = new GameObject("Entrance");
            entrGO.hideFlags = HideFlags.DontSaveInBuild /*| HideFlags.HideInHierarchy*/;
            RBSEntrance entrance = entrGO.AddComponent<RBSEntrance>();
            entrance.Init(transform, position);
        }
    }
}
#endif