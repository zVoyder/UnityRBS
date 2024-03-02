#if UNITY_EDITOR
namespace RBS.Room
{
    using Data;
    using Unity.VisualScripting;
    using UnityEngine;
    using UnityEditor;
    
    public class RBSEntrance : MonoBehaviour
    {
        private BoxCollider _snapCollider;
        
        public void Init(Transform parent, Vector3 position, Vector3 size)
        {
            GenerateSnapCollider(size);
            transform.position = position;
            transform.parent = parent;
            transform.rotation = Quaternion.LookRotation(transform.position - parent.position);
        }

        private void GenerateSnapCollider(Vector3 size)
        {
            _snapCollider = gameObject.AddComponent<BoxCollider>();
            _snapCollider.isTrigger = true;
            transform.localScale = size;
        }

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(transform.position,_snapCollider.size);
        }
    }
}
#endif