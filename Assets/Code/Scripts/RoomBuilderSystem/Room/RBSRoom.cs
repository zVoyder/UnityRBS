#if UNITY_EDITOR
namespace RBS.Room
{
    using UnityEngine;
    using RBS.Room.Data;
    using Unity.VisualScripting;

    public class RBSRoom : MonoBehaviour
    {
        private Collider _collider;

        public RBSRoomSnap RoomSnap { get; private set; }

        public void Init(RBSRoomSnap rbsRoomSnap)
        {
            RoomSnap = rbsRoomSnap;
            TryGetComponent(out _collider);
            GenerateEntrances();
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
            entrGO.hideFlags = HideFlags.DontSaveInBuild | HideFlags.HideInHierarchy;
            RBSEntrance entrance = entrGO.AddComponent<RBSEntrance>();
            entrance.Init(transform, position, Vector3.one * 2f);
        }
    }
}
#endif