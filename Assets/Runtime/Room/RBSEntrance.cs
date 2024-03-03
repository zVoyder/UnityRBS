#if UNITY_EDITOR
namespace RBS.Runtime.Room
{
    using Data;
    using Unity.VisualScripting;
    using UnityEngine;
    using UnityEditor;
    
    public class RBSEntrance : MonoBehaviour
    {
        private BoxCollider _snapCollider;
        
        public void Init(Transform parent, Vector3 position)
        {
            transform.position = position;
            transform.parent = parent;
            transform.rotation = Quaternion.LookRotation(transform.position - parent.position);
            GenerateSnapCollider();
        }

        private void GenerateSnapCollider()
        {
            _snapCollider = gameObject.AddComponent<BoxCollider>();
            _snapCollider.isTrigger = true;
            _snapCollider.size = transform.parent.lossyScale / 2f;
        }
    }
}
#endif