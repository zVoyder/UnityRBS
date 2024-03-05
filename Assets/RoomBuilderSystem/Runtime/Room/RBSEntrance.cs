#if UNITY_EDITOR
namespace RBS.Runtime.Room
{
    using UnityEngine;
    using UnityEditor;
    using RBS.Runtime.Room.Data;
    
    public class RBSEntrance : MonoBehaviour
    {
        private BoxCollider _snapCollider;
        
        /// <summary>
        /// Initializes the entrance.
        /// </summary>
        /// <param name="parent"> The parent to initialize the entrance with. </param>
        /// <param name="position"> The position to initialize the entrance with. </param>
        /// <param name="size"> The size to initialize the entrance with. </param>
        public void Init(Transform parent, Vector3 position, Vector3 size)
        {
            GenerateSnapCollider(size);
            transform.position = position;
            transform.SetParent(parent, true);
            transform.rotation = Quaternion.LookRotation(transform.position - parent.position);
        }

        /// <summary>
        /// Generates the snap collider.
        /// </summary>
        /// <param name="size"> The size to generate the snap collider with. </param>
        private void GenerateSnapCollider(Vector3 size)
        {
            _snapCollider = gameObject.AddComponent<BoxCollider>();
            _snapCollider.isTrigger = true;
            float variation = Random.Range(size.y / 100, size.y / 80); // Random variation to avoid snapping issues
            transform.localScale = (size + new Vector3(0, variation, 0)) + transform.lossyScale;
        }
    }
}
#endif