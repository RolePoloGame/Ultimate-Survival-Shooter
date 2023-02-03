using NaughtyAttributes;
using System;
using UnityEditor;
using UnityEngine;

namespace Core.World
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class MinimapIcon : MonoBehaviour
    {
        #region Fields

        [SerializeField]
        [Required]
        private Sprite Sprite;
        [SerializeField]
        private Color IconColor = Color.white;
        [HorizontalLine]

        [SerializeField]
        private bool FreezeRotation = true;
        [SerializeField]
        private bool ApplyRotationOffset = true;
        [SerializeField]
        [ShowIf(EConditionOperator.Or, nameof(FreezeRotation), nameof(ApplyRotationOffset))]
        [Range(0f, 1f)]
        private float RotationOffset = 0.0f;

        [HorizontalLine]

        [SerializeField]
        private bool KeepOnMinimapEdge = false;
        [SerializeField]
        [ShowIf(nameof(KeepOnMinimapEdge))]
        private bool HasToBeRemembered = true;
        [SerializeField]
        [ShowIf(nameof(KeepOnMinimapEdge))]
        private Transform MinimapCam;
        [ShowIf(nameof(KeepOnMinimapEdge))]
        [SerializeField]
        private float MinimapSize = 9.0f;
        [SerializeField]
        [ShowIf(nameof(KeepOnMinimapEdge))]
        private bool ScaleOnDistance = true;
        [SerializeField]
        [ShowIf(EConditionOperator.And, nameof(ScaleOnDistance), nameof(KeepOnMinimapEdge))]
        private bool DesaturizeOnDistance = true;

        private Vector3 parentPosition;
        private Vector3 Rotation = new(90.0f, 0.0f, 0.0f);

        [SerializeField]
        [ShowIf(EConditionOperator.And, nameof(ScaleOnDistance), nameof(KeepOnMinimapEdge))]
        private float DefaultScale = 0.5f;
        [SerializeField]
        [ShowIf(EConditionOperator.And, nameof(ScaleOnDistance), nameof(KeepOnMinimapEdge))]
        private float SmallestScale = 0.1f;

        #endregion

        #region Components
        private SpriteRenderer spriteRenderer;
        private SpriteRenderer GetSpriteRenderer()
        {
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
            return spriteRenderer;
        }
        private Transform GetMinimapCam()
        {
            if (MinimapCam == null)
            {
                //if (!EditorApplication.isPlaying)
                //    return transform;
                MinimapCam = CameraFollow.Camera;
                //Debug.LogError("MinimapCam not set, disabling locking to camera edge");
                //KeepOnMinimapEdge = false;
                //return transform;
            }
            return MinimapCam;
        }
        #endregion

        #region Tags
        [SerializeField]
        [ShowNativeProperty]
        private bool isRemembered
        {
            get
            {
                return tempVal;
            }
            set
            {
                if (!Application.isPlaying) return;
                tempVal = value;
            }
        }

        private bool tempVal = false;
        #endregion

        #region Unity Methods
        private void OnEnable()
        {
            UpdateSprite();
        }
        void Update()
        {
            UpdateValues();
        }

        private void UpdateValues()
        {
            if (!KeepOnMinimapEdge)
                return;
            UpdatePositionVector();
        }

        void LateUpdate()
        {
            UpdatePositionAndRotation();
        }

        private void UpdatePositionAndRotation()
        {
            if (FreezeRotation)
                SetIconRotation();
            if (ApplyRotationOffset)
                AddRotationOffset();
            if (KeepOnMinimapEdge)
                SetIconPosition();
        }

        private void OnValidate()
        {
            if (EditorApplication.isPlaying) return;
            UpdateValues();
            UpdatePositionAndRotation();
            UpdateSprite();
        }
        #endregion

        #region Public methods

        public void UpdateMinimapCamera(Transform camera)
        {
            MinimapCam = camera;
        }
        #endregion

        #region Private methods
        private void SetIconPosition()
        {
            // Center of Minimap
            Vector3 centerPosition = GetMinimapCam().localPosition;

            centerPosition.y = transform.position.y;

            // Nullyfing y coordinate
            // Distance from the gameObject to Minimap
            float Distance = GetDistance(centerPosition);

            // If the Distance is less than MinimapSize, it is within the Minimap view and we don't need to do anything
            float mapsizeOverDistance = MinimapSize / Distance;
            if (ScaleOnDistance)
            {
                ScaleOverDistance(mapsizeOverDistance);
                if (DesaturizeOnDistance)
                    DesaturizeOverDistance(Distance);
            }


            if (Distance <= MinimapSize)
            {
                if (HasToBeRemembered)
                    isRemembered = true;
                return;
            }

            if (HasToBeRemembered)
            {
                if (!isRemembered)
                    return;
            }

            KeepOnedge(centerPosition, mapsizeOverDistance);
        }

        private void KeepOnedge(Vector3 centerPosition, float mapsizeOverDistance)
        {
            Vector3 fromOriginToObject = transform.position - centerPosition;
            fromOriginToObject *= mapsizeOverDistance;
            transform.position = centerPosition + fromOriginToObject;
        }

        private void DesaturizeOverDistance(float Distance)
        {
            Distance = Mathf.Max(Distance - MinimapSize, 0.0f);
            Distance = Mathf.Min(Distance, MinimapSize);

            float val = (Distance / MinimapSize);

            UpdateSpriteColor(Color.Lerp(IconColor, Color.gray, val));
        }

        private void UpdateSpriteColor(Color color)
        {
            GetSpriteRenderer().color = color;
        }

        private void ScaleOverDistance(float sizeOverDistance)
        {
            float factor = Mathf.Max(sizeOverDistance, SmallestScale);
            factor = Mathf.Min(factor, DefaultScale);
            transform.localScale = new Vector3(factor, factor, factor);
        }

        private float GetDistance(Vector3 centerPosition)
        {
            Vector3 _thisPos = transform.position;
            _thisPos.y = 0.0f;
            Vector3 _centerPos = centerPosition;
            _centerPos.y = 0.0f;
            return Vector3.Distance(_thisPos, _centerPos);
        }

        private void UpdateSprite()
        {
            GetSpriteRenderer().sprite = Sprite;
            UpdateSpriteColor(IconColor);
        }
        private void UpdatePositionVector()
        {
            if (transform.parent is null)
                return;
            parentPosition = transform.parent.transform.position;
            parentPosition.y = transform.position.y;
            transform.position = parentPosition;
        }

        private Vector3 GetLockedRotation()
        {
            if (Rotation.x == RotationOffset * 360.0f)
                return Rotation;
            return Rotation = new Vector3(90.0f, RotationOffset * 360.0f, 0.0f);
        }
        private void SetIconRotation()
        {
            transform.rotation = Quaternion.Euler(GetLockedRotation());
        }
        private void AddRotationOffset()
        {
            Vector3 rot = transform.rotation.eulerAngles;
            rot = new Vector3(rot.x, rot.y + RotationOffset * 360.0f, rot.z);
            transform.rotation = Quaternion.Euler(rot);
        }
        #endregion
    }
}