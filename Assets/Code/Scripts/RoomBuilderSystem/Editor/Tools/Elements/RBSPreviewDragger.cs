namespace RBS.Editor.Tools.Elements
{
    using UnityEditor;
    using UnityEngine;
    using RBS.Editor.Data;
    using RBS.Room;

    public class RBSPreviewDragger
    {
        private RBSRoomData _currentRoomData;
        private Bounds _previewBounds;
        private GameObject _prefabAsPreview;
        private GameObject _draggedPreview;

        private bool _isDragging;
        private Material _previewMaterial;

        public RBSPreviewDragger()
        {
            _draggedPreview = null;
            _isDragging = false;

            _previewMaterial = new Material(Shader.Find("Transparent/Diffuse"))
            {
                color = new Color(1f, 1f, 1f, 0.5f)
            };
        }

        public void SetAsPreview(RBSRoomData room)
        {
            _currentRoomData = room;
            _prefabAsPreview = room.RoomPrefab;
            if (_prefabAsPreview.TryGetComponent(out Renderer renderer))
                _previewBounds = renderer.bounds;
            DisableDrag();
            EnableDrag();
        }

        public void EnableDrag()
        {
            if (_isDragging) return;
            if (!_prefabAsPreview) return;

            _isDragging = true;
            InstantiatePreview(_prefabAsPreview);
        }

        public void DisableDrag()
        {
            if (!_isDragging) return;
            if (!_draggedPreview) return;

            _isDragging = false;
            DestroyPreview();
        }

        public bool TryPlacePrefab(out GameObject placedGameobject)
        {
            placedGameobject = null;
            if (!_isDragging) return false;
            if (!_draggedPreview) return false;

            placedGameobject = Object.Instantiate(_prefabAsPreview, _draggedPreview.transform.position, _draggedPreview.transform.rotation);
            return true;
        }

        // Do not use Graphics.DrawMeshNow to avoid performance issues
        public void Drag(Camera sceneCamera, float height = 0f)
        {
            if (!_isDragging) return;

            Vector3 pos;
            Event currentEvent = Event.current;
            Vector2 mousePosition = new Vector2(currentEvent.mousePosition.x, currentEvent.mousePosition.y);
            Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
            
            if (Physics.Raycast(ray, out RaycastHit hit) && SnapTool.EnableSnapOnObjects)
            {
                if (!TryGetSnapPosition(hit, out pos))
                    pos = hit.point;
            }
            else
            {
                pos = GetSnapPositionOnGrid(sceneCamera, height, ray);
            }

            _draggedPreview.transform.position = pos;
            DisableSelection(); // Disable selection to avoid the placed room to be selected when clicking on it
        }
        
        private Vector3 GetSnapPositionOnGrid(Camera sceneCamera, float height, Ray ray)
        {
            Vector3 pos;
            Vector3 rayDirection = ray.direction;
            Vector3 cameraPos = sceneCamera.transform.position;

            float t = (height - cameraPos.y) / rayDirection.y;
            Vector3 pointOnPlane = cameraPos + t * rayDirection;
            pos = new Vector3(pointOnPlane.x, height, pointOnPlane.z);

            Vector3 gridPos = new Vector3(pos.x, 0, pos.z);
            Color color = pos.y < 0 ? Color.red : Color.green;
            Handles.color = color;
            Handles.DrawLine(gridPos, pos, 1);
            Handles.DrawWireDisc(gridPos, Vector3.up, _previewBounds.size.x / 2.0f);
            Handles.color = Color.white;
            Handles.DrawWireDisc(pos, Vector3.up, _previewBounds.size.x / 2.0f);
            return pos;
        }

        #region PREVIEW

        public void SetPreviewRotation(Quaternion rotation)
        {
            if (!_draggedPreview) return;
            _draggedPreview.transform.rotation = rotation;
        }

        public void RotatePreview(float angle)
        {
            if (!_draggedPreview) return;
            _draggedPreview.transform.Rotate(Vector3.up, angle);
        }

        #region SNAP

        private bool TryGetSnapPosition(RaycastHit hit, out Vector3 position)
        {
            position = Vector3.zero;
            if (!hit.transform.TryGetComponent(out RBSEntrance entrance) || !SnapTool.EnableSnap) return false;

            int angle = Mathf.RoundToInt(
                Vector3.SignedAngle(entrance.transform.forward, _draggedPreview.transform.forward, Vector3.up)
            );

            if (CalculatePositionDistanceOnMatching(angle, out Vector3 posDistance))
            {
                position = entrance.transform.position + posDistance;
                return true;
            }

            return false;
        }

        private bool CalculatePositionDistanceOnMatching(int angle, out Vector3 posDistance)
        {
            posDistance = Vector3.zero;
            bool hasEntrance = !SnapTool.SnapOnlyOnFacingDoors;
            
            switch (angle)
            {
                case 0:
                    posDistance = (_previewBounds.size.z / 2.0f) * _draggedPreview.transform.forward;
                    hasEntrance |= _currentRoomData.RoomSnap.HasEntranceSouth;
                    break;
                case 90:
                    posDistance = -(_previewBounds.size.x / 2.0f) * _draggedPreview.transform.right;
                    hasEntrance |= _currentRoomData.RoomSnap.HasEntranceEast;
                    break;
                case -90:
                    posDistance = (_previewBounds.size.x / 2.0f) * _draggedPreview.transform.right;
                    hasEntrance |= _currentRoomData.RoomSnap.HasEntranceWest;
                    break;
                case 180:
                case -180:
                    posDistance = -(_previewBounds.size.z / 2.0f) * _draggedPreview.transform.forward;
                    hasEntrance |= _currentRoomData.RoomSnap.HasEntranceNorth;
                    break;
                default:
                    return false;
            }
            
            return hasEntrance;
        }

        #endregion

        private void InstantiatePreview(GameObject prefab)
        {
            _draggedPreview = Object.Instantiate(prefab);

            SetPreviewMaterial();
            SetPreviewCollision(false);
            SetHideFlagsPreview(HideFlags.HideInHierarchy);
        }

        private void DestroyPreview()
        {
            Object.DestroyImmediate(_draggedPreview);
        }

        private void SetPreviewMaterial()
        {
            MeshRenderer[] renderers = _draggedPreview.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in renderers)
            {
                Material mat = new Material(renderer.sharedMaterial);
                mat.shader = Shader.Find("Transparent/Diffuse");
                Color color = mat.color;
                color.a = 0.5f;
                mat.color = color;
                renderer.sharedMaterial = mat;
            }
        }

        private void SetPreviewCollision(bool isEnabled)
        {
            Collider[] colliders = _draggedPreview.GetComponentsInChildren<Collider>();
            foreach (Collider collider in colliders)
                collider.enabled = isEnabled;
        }

        private void SetHideFlagsPreview(HideFlags flags)
        {
            _draggedPreview.hideFlags = flags;
            foreach (Transform child in _draggedPreview.transform)
                child.gameObject.hideFlags = flags;
        }

        #endregion PREVIEW

        private void DisableSelection()
        {
            Selection.activeGameObject = null;
        }
    }
}