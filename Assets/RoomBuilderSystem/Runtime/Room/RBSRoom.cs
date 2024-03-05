#if UNITY_EDITOR
namespace RBS.Runtime.Room
{
    using System;
    using UnityEngine;
    using RBS.Runtime.Room.Data;

    public class RBSRoom : MonoBehaviour
    {
        private Bounds _bounds;
        
        public RBSRoomData RoomData { get; private set; }
        public RBSRoomSnap RoomSnap => RoomData.RoomSnap;

        private void OnValidate()
        {
            if (TryGetComponent(out Collider collider))
                _bounds = collider.bounds;
        }

        /// <summary>
        /// Initializes the room.
        /// </summary>
        /// <param name="roomData"> The room data to initialize the room with. </param>
        /// <param name="position"> The position to initialize the room with. </param>
        /// <param name="rotation"> The rotation to initialize the room with. </param>
        public void Init(RBSRoomData roomData, Vector3 position, Quaternion rotation)
        {
            RoomData = roomData;
            transform.SetPositionAndRotation(position, rotation);
            
            if(_bounds != null)
                GenerateEntrances();
        }

        /// <summary>
        /// Generates the entrances.
        /// </summary>
        private void GenerateEntrances()
        {
            Vector3 northPosition = transform.position + _bounds.extents.z * transform.forward;
            Vector3 southPosition = transform.position - _bounds.extents.z * transform.forward;
            Vector3 eastPosition = transform.position + _bounds.extents.x * transform.right;
            Vector3 westPosition = transform.position - _bounds.extents.x * transform.right;

            if (RoomSnap.HasEntranceNorth)
                GenerateEntrance(northPosition, RBSEntranceType.North);

            if (RoomSnap.HasEntranceSouth)
                GenerateEntrance(southPosition, RBSEntranceType.South);

            if (RoomSnap.HasEntranceEast)
                GenerateEntrance(eastPosition, RBSEntranceType.East);

            if (RoomSnap.HasEntranceWest)
                GenerateEntrance(westPosition, RBSEntranceType.West);
        }

        /// <summary>
        /// Generates an entrance.
        /// </summary>
        /// <param name="position"> The position to generate the entrance at. </param>
        /// <param name="entranceType"> The entrance type to generate the entrance with. </param>
        private void GenerateEntrance(Vector3 position, RBSEntranceType entranceType)
        {
            GameObject entrGO = new GameObject("Entrance");
            // entrGO.hideFlags = HideFlags.DontSaveInBuild | HideFlags.HideInHierarchy;
            RBSEntrance entrance = entrGO.AddComponent<RBSEntrance>();
            entrance.Init(transform, position, _bounds.size.normalized);
        }
    }
}
#endif