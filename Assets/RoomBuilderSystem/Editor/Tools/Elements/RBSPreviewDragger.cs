namespace RBS.Editor.Tools.Elements
{
    using UnityEditor;
    using UnityEngine;
    using RBS.Runtime.Room.Data;
    using RBS.Runtime.Room;
    using RBS.Editor.Utility;
    using RBS.Editor.Config;

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

        /// <summary>
        /// Sets the room as a preview to be dragged.
        /// </summary>
        /// <param name="room"> The room to be set as a preview. </param>
        public void SetAsPreview(RBSRoomData room)
        {
            _currentRoomData = room;
            _prefabAsPreview = room.RoomPrefab;
            DisableDrag();
            EnableDrag();
        }

        /// <summary>
        /// Enables the dragging of the preview.
        /// </summary>
        public void EnableDrag()
        {
            if (_isDragging) return;
            if (!_prefabAsPreview) return;

            _isDragging = true;
            InstantiatePreview(_prefabAsPreview);
        }

        /// <summary>
        /// Disables the dragging of the preview.
        /// </summary>
        public void DisableDrag()
        {
            if (!_isDragging) return;
            if (!_draggedPreview) return;

            _isDragging = false;
            DestroyPreview();
        }

        /// <summary>
        /// Checks if the preview can be placed and returns the position and rotation of the preview.
        /// </summary>
        /// <param name="previewPosition"> The position of the preview. </param>
        /// <param name="previewRotation"> The rotation of the preview. </param>
        /// <returns> True if the preview can be placed, false otherwise. </returns>
        public bool CanPlacePrefab(out Vector3 previewPosition, out Quaternion previewRotation)
        {
            previewPosition = Vector3.zero;
            previewRotation = Quaternion.identity;

            if (!_isDragging) return false;
            if (!_draggedPreview) return false;

            previewPosition = _draggedPreview.transform.position;
            previewRotation = _draggedPreview.transform.rotation;
            return true;
        }

        /// <summary>
        /// Drags the preview in the scene.
        /// </summary>
        /// <param name="sceneCamera"> The camera of the scene. </param>
        /// <param name="height"> The height of the preview. </param>
        // Do not use Graphics.DrawMeshNow to avoid performance issues
        public void Drag(Camera sceneCamera, float height = 0f)
        {
            if (!_isDragging) return;

            Vector3 pos;
            Vector2 mousePosition = new Vector2(Event.current.mousePosition.x, Event.current.mousePosition.y);
            Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit) && SnapTool.EnableSnapOnObjects)
            {
                if (!TryGetSnapPosition(hit, out pos))
                    pos = hit.point;
            }
            else
            {
                pos = GetPositionOnImaginaryPlane(sceneCamera, height, ray);

                if (RBSPrefs.HasGridSnap)
                    pos = GetPositionOnGrid(pos, RBSPrefs.GridSnapSize);
            }

            DrawHandles(pos);
            _draggedPreview.transform.position = pos;
            DisableSelection(); // Disable selection to avoid the placed room to be selected when clicking on it
        }

        #region PREVIEW

        /// <summary>
        /// Sets the preview rotation.
        /// </summary>
        /// <param name="rotation"> The rotation to be set. </param>
        public void SetPreviewRotation(Quaternion rotation)
        {
            if (!_draggedPreview) return;
            _draggedPreview.transform.rotation = rotation;
        }

        /// <summary>
        /// Rotates the preview by the given angle on the Y axis.
        /// </summary>
        /// <param name="angle"> The angle to rotate the preview. </param>
        public void RotatePreview(float angle)
        {
            if (!_draggedPreview) return;
            _draggedPreview.transform.Rotate(Vector3.up, angle);
        }

        /// <summary>
        /// Instantiates the preview of the room.
        /// </summary>
        /// <param name="prefab"> The prefab to be instantiated as a preview. </param>
        private void InstantiatePreview(GameObject prefab)
        {
            _draggedPreview = Object.Instantiate(prefab);

            SetPreviewBounds();
            SetPreviewMaterial();
            SetPreviewCollision(false);
            SetHideFlagsPreview(HideFlags.HideInHierarchy);
        }
        
        /// <summary>
        /// Sets the bounds of the preview.
        /// </summary>
        private void SetPreviewBounds()
        {
            if (_draggedPreview.TryGetComponent(out Collider collider))
                _previewBounds = collider.bounds;
        }

        /// <summary>
        /// Destroys the preview of the room.
        /// </summary>
        private void DestroyPreview()
        {
            Object.DestroyImmediate(_draggedPreview);
        }

        /// <summary>
        /// Sets the material of the preview to the preview material.
        /// </summary>
        private void SetPreviewMaterial()
        {
            MeshRenderer[] renderers = _draggedPreview.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer renderer in renderers)
                renderer.sharedMaterial = _previewMaterial;
        }

        /// <summary>
        /// Sets the collision of the preview.
        /// </summary>
        /// <param name="isEnabled"> True to enable the collision, false to disable it. </param>
        private void SetPreviewCollision(bool isEnabled)
        {
            Collider[] colliders = _draggedPreview.GetComponentsInChildren<Collider>();
            foreach (Collider collider in colliders)
                collider.enabled = isEnabled;
        }

        /// <summary>
        /// Sets the hide flags of the preview.
        /// </summary>
        /// <param name="flags"> The hide flags to be set. </param>
        private void SetHideFlagsPreview(HideFlags flags)
        {
            _draggedPreview.hideFlags = flags;
            foreach (Transform child in _draggedPreview.transform)
                child.gameObject.hideFlags = flags;
        }

        #endregion PREVIEW

        #region SNAP

        /// <summary>
        /// Tries to get the snap position of the preview.
        /// </summary>
        /// <param name="hit"> The hit of the raycast. </param>
        /// <param name="position"> The position of the snap. </param>
        /// <returns> True if the snap position was found, false otherwise. </returns>
        private bool TryGetSnapPosition(RaycastHit hit, out Vector3 position)
        {
            position = Vector3.zero;
            if (!hit.transform.TryGetComponent(out RBSEntrance entrance) || !SnapTool.EnableSnap) return false;
            
            if (TryCalculateEntranceSnapPosition(entrance, out Vector3 snapPosition))
            {
                position = snapPosition;
                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries to calculate the snap position of the preview.
        /// </summary>
        /// <param name="entrance"> The entrance to snap to. </param>
        /// <param name="snapPosition"> The snap position of the entrance. </param>
        /// <returns> True if the snap position was found, false otherwise. </returns>
        private bool TryCalculateEntranceSnapPosition(RBSEntrance entrance, out Vector3 snapPosition)
        {
            snapPosition = Vector3.zero;
            int angle = Mathf.RoundToInt(
                Vector3.SignedAngle(entrance.transform.forward, _draggedPreview.transform.forward, Vector3.up)
            );
            bool hasEntrance = !SnapTool.SnapOnlyOnFacingDoors;
            
            switch (angle)
            {
                case 0:
                    snapPosition = entrance.transform.position + (_previewBounds.size.z / 2.0f) * _draggedPreview.transform.forward;
                    hasEntrance |= _currentRoomData.RoomSnap.HasEntranceSouth;
                    break;
                case 90:
                    snapPosition = entrance.transform.position + -(_previewBounds.size.x / 2.0f) * _draggedPreview.transform.right;
                    hasEntrance |= _currentRoomData.RoomSnap.HasEntranceEast;
                    break;
                case -90:
                    snapPosition = entrance.transform.position + (_previewBounds.size.x / 2.0f) * _draggedPreview.transform.right;
                    hasEntrance |= _currentRoomData.RoomSnap.HasEntranceWest;
                    break;
                case 180:
                case -180:
                    snapPosition = entrance.transform.position + -(_previewBounds.size.z / 2.0f) * _draggedPreview.transform.forward;
                    hasEntrance |= _currentRoomData.RoomSnap.HasEntranceNorth;
                    break;
                default:
                    return false;
            }

            return hasEntrance;
        }

        /// <summary>
        /// Converts a position to a position on a grid for snapping.
        /// </summary>
        /// <param name="pos"> The position to be converted. </param>
        /// <param name="gridSize"> The size of the grid. </param>
        /// <returns> The position on the grid. </returns>
        private Vector3 GetPositionOnGrid(Vector3 pos, float gridSize)
        {
            return new Vector3(
                Mathf.Round(pos.x / gridSize) * gridSize,
                pos.y,
                Mathf.Round(pos.z / gridSize) * gridSize
            );
        }

        /// <summary>
        /// Gets the position of the preview on an imaginary plane.
        /// </summary>
        /// <param name="sceneCamera"> The camera of the scene. </param>
        /// <param name="height"> The height of the preview. </param>
        /// <param name="ray"> The ray to get the direction from. </param>
        /// <returns> The position on the imaginary plane. </returns>
        private Vector3 GetPositionOnImaginaryPlane(Camera sceneCamera, float height, Ray ray)
        {
            Vector3 pos;
            Vector3 rayDirection = ray.direction;
            Vector3 cameraPos = sceneCamera.transform.position;

            float t = (height - cameraPos.y) / rayDirection.y;
            Vector3 pointOnPlane = cameraPos + t * rayDirection;
            pos = new Vector3(pointOnPlane.x, height, pointOnPlane.z);

            return pos;
        }

        /// <summary>
        /// Draws the handles of the preview.
        /// </summary>
        /// <param name="pos"> The position where to draw the handles. </param>
        private void DrawHandles(Vector3 pos)
        {
            Vector3 gridPos = new Vector3(pos.x, 0, pos.z);
            Color color = pos.y < 0 ? Color.red : Color.green;
            Handles.color = color;
            Handles.DrawLine(gridPos, pos, 1);
            Handles.DrawWireDisc(gridPos, Vector3.up, _previewBounds.size.x / 2.0f);
            Handles.color = Color.white;
            Handles.DrawWireDisc(pos, Vector3.up, _previewBounds.size.x / 2.0f);
        }

        #endregion

        /// <summary>
        /// Disables the unity scene selection.
        /// </summary>
        private void DisableSelection()
        {
            Selection.activeGameObject = null;
        }
    }
}