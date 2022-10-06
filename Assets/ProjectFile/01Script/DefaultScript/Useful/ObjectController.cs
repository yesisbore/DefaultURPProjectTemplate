using System.Collections;
using System.Collections.Generic;
using GlobalType;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UnityCore
{
    namespace ObjectController
    {
        public enum ObjectEditState { FindControlTarget, WaitForMenuSelect, Move, Rotate, Scale }

        [RequireComponent(typeof(Camera))]
        public class ObjectController : MonoBehaviour
        {
            #region Variables

            // Public Variables

            public DebugModeType DebugMode = DebugModeType.Global;

            // Private Variables

            [Header("Prefab")] [SerializeField] private AssetReference _indicatorPrefab;

            [Header("Settings")] [SerializeField] private InputType _useInput = InputType.Mouse;
            [SerializeField] private LayerMask _targetLayer;
            [SerializeField] private ObjectEditState _objectEditState = ObjectEditState.FindControlTarget;

            private Transform _selectedTarget;
            private Transform _indicator;
            private Camera _camera;
            private bool _ownIndicator = false;

            private float _maxDistance = float.MaxValue;

            // Input 
            private Vector2 InputPosition
            {
                get
                {
                    switch (_useInput)
                    {
                        case InputType.Mouse:
                            return Input.mousePosition;
                            break;
                        case InputType.Touch:
                            return Input.GetTouch(0).position;
                            break;     
                    }

                    return Vector2.zero;
                }
            } // End of InputPosition
            
            private bool ScreenTouch
            {
                get
                {
                    switch (_useInput)
                    {
                        case InputType.Mouse:
                            return Input.GetMouseButtonDown(0);
                            break;
                        case InputType.Touch:
                            return Input.touchCount > 0;
                            break;     
                    }

                    return false;
                }
            } // End of ScreenTouch
            
            #endregion Variables

            #region Unity Methods

            private void Start()
            {
                Initialize();
            } // End of Unity - Start

            private void Update()
            {
                DebugRay();
                ObjectControl();
            } // End of Unity - Update

            #endregion Unity Methods

            #region Public Methods

            #endregion Public Methods

            #region Private Methods

            private void Initialize()
            {
                GetComponents();
            } // End of Initialize
            private void GetComponents()
            {
                _camera = GetComponent<Camera>();
            } // End of GetComponents

            
            // Edit State
            private void ObjectControl()
            {
                switch (_objectEditState)
                {
                    case ObjectEditState.FindControlTarget:
                        FindControlTarget();
                        break;
                    case ObjectEditState.WaitForMenuSelect:
                        WaitForMenuSelect();
                        break;
                    case ObjectEditState.Move:
                        break;
                    case ObjectEditState.Rotate:
                        break;
                    case ObjectEditState.Scale:
                        break;
                }
            } // End of ObjectControl
            private void FindControlTarget()
            {
                if (_selectedTarget) return;

                if (!GetTargetFromInputPosition()) return;
                Log("Select object : " + _selectedTarget.name);

                ShowControlUI();
                _objectEditState = ObjectEditState.WaitForMenuSelect;
            } // End of GetControlTarget
            private void WaitForMenuSelect()
            {
                if (!ScreenTouch) return;

                // If Choose 
                if (GetTargetFromInputPosition())
                {
                    UpdateIndicator();
                    return;
                }
                
                UnSelectTarget();
            } // End of WaitForMenuSelect


            // Ray
            private bool GetTargetFromInputPosition()
            {
                if (!ScreenTouch) return false;

                if (!_camera)
                {
                    LogWarning("There is no Camera");
                    return false;
                }

                var ray = _camera.ScreenPointToRay(InputPosition);

                if (!Physics.Raycast(ray, out var hit, _maxDistance, _targetLayer)) return false;
                Log("Hit object : " + hit.transform.name);

                _selectedTarget = hit.transform;
                return true;
            } // End of GetTargetFromInputPosition
            private void UnSelectTarget()
            {
                _selectedTarget = null;
                DisableIndicator();
                _objectEditState = ObjectEditState.FindControlTarget;
            } // End of UnSelectTarget
            
            
            // Debug Process 
            private bool CheckDebugMode => DebugMode == DebugModeType.Global && !GameSetting.Instance.DebugMode;
            private void Log(string msg)
            {
                if(CheckDebugMode) return;

                Debug.Log("[Object Controller]: " + msg);
            }
            private void LogWarning(string msg)
            {
                if(CheckDebugMode) return;

                Debug.Log("[Object Controller]: " + msg);
            }
            private void DebugRay()
            {
                if (!GameSetting.Instance.DebugMode) return;
                if (!_camera) return;

                var playerPos = transform.position;
                var inputPos = (Vector3)InputPosition;
                inputPos.z = 10f;

                inputPos = _camera.ScreenToWorldPoint(inputPos);
                Debug.DrawRay(playerPos, inputPos - playerPos, Color.blue);
            }

            #endregion Private Methods

            #region UI Methods

            private void ShowControlUI()
            {
                UpdateIndicator();
            } // End of ShowControlUI

            // Indicator
            private void UpdateIndicator()
            {
                if (!_selectedTarget) return;

                if (!_ownIndicator)
                {
                    _ownIndicator = true;

                    _indicatorPrefab.InstantiateAsync().Completed += (op) =>
                    {
                        _indicator = op.Result.transform;
                        EnableIndicator();
                        return;
                    };
                }

                if (!_indicator) return;
                
                _indicator.gameObject.SetActive(false);
                _indicator.SetParent(null);
                EnableIndicator();
            } // End of UpdateIndicator
            private void EnableIndicator()
            {
                AdjustIndicatorProportion();
                _indicator.gameObject.SetActive(true);
            } // End of EnableIndicator
            private void DisableIndicator()
            {
                if (!_indicator) return;

                Log("Disable Indicator");
                _indicator.gameObject.SetActive(false);
                _indicator.SetParent(null);
            } // End of DisableIndicator
            private void AdjustIndicatorProportion()
            {
                var indicatorTransform = _indicator.transform;
                var targetPosition = _selectedTarget.position;
                var targetExtents = _selectedTarget.GetComponent<MeshFilter>().mesh.bounds.extents;

                // Calculate - Get world scale extents
                var extents = _selectedTarget.TransformVector(targetExtents);
                extents.x = Mathf.Abs(extents.x);
                extents.y = Mathf.Abs(extents.y);
                extents.z = Mathf.Abs(extents.z);
                Log("extents.x : " + extents.x + "extents.y : " + extents.y + "extents.z : " + extents.z );

                // Position
                var newPosition = new Vector3(targetPosition.x, targetPosition.y - extents.y, targetPosition.z) ;
                
                // Scale
                var newScale = new Vector3(extents.x * 2.0f, 1.0f, extents.z * 2.0f);
                    
                indicatorTransform.position = newPosition;
                indicatorTransform.localScale = newScale;
                indicatorTransform.parent = _selectedTarget.transform;

            } // End of AdjustIndicatorProportion

            #endregion
        }
    }
}