using System.Collections;
using GlobalType;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;

namespace UnityCore
{
    namespace ObjectSelect
    {
        public class ObjectSelector : MonoBehaviour
        {
            #region Variables

            public DebugModeType DebugMode = DebugModeType.Global;

            // Public Variables

            // Private Variables
            [Header("Prefab")] [SerializeField] private AssetReference _floatingPopUpUIPrefab;

            [Header("Settings")] 
            [SerializeField] private LayerMask _targetLayer;
            [SerializeField] private ObjectSelectorState _objectEditState = ObjectSelectorState.FindControlTarget;

            private FloatingPopUpUI _floatingPopUpUI;
            private EditObject _selectedEditObject;
            private Camera _mainCamera;
            private bool _canTouch = true;

            // Input 
            private Vector2 InputPosition
            {
                get
                {
                    switch (GameSetting.Instance.DeviceTarget)
                    {
                        case DeviceTarget.PC:
                            return Input.mousePosition;
                        case DeviceTarget.Mobile:
                            return Input.GetTouch(0).position;
                    }

                    return Vector2.zero;
                }
            } // End of InputPosition

            private bool ScreenTouch
            {
                get
                {
                    switch (GameSetting.Instance.DeviceTarget)
                    {
                        case DeviceTarget.PC:
                            return Input.GetMouseButtonDown(0);
                        case DeviceTarget.Mobile:
                            return Input.touchCount > 0;
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

            // Edit Object
            public void MoveObject()
            {
                Log("Move Object");

                _objectEditState = ObjectSelectorState.Editing;
                _selectedEditObject.Move();
            } // End of MoveObject

            public void RotateObject()
            {
                Log("Rotate Object");

                _objectEditState = ObjectSelectorState.Editing;
                _selectedEditObject.Rotate();
            } // End of RotateObject

            public void ScaleObject()
            {
                Log("Scale Object");

                _objectEditState = ObjectSelectorState.Editing;
                _selectedEditObject.Scale();
            } // End of ScaleObject

            public void StopEditing()
            {
                Log("Stop Editing");

                _objectEditState = ObjectSelectorState.WaitForMenuSelect;
                _selectedEditObject.StopEditing();
            } // End of StopEditing

            #endregion Public Methods

            #region Private Methods

            private void Initialize()
            {
                GetComponents();
            } // End of Initialize

            private void GetComponents()
            {
                // get a reference to our main camera
                if (_mainCamera == null)
                {
                    _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
                }
            } // End of GetComponents

            // Edit State
            private void ObjectControl()
            {
                if(!_canTouch) return;
                
                switch (_objectEditState)
                {
                    case ObjectSelectorState.FindControlTarget:
                        FindControlTarget();
                        break;
                    case ObjectSelectorState.WaitForMenuSelect:
                        WaitForMenuSelect();
                        break;
                }
            } // End of ObjectControl

            private void FindControlTarget()
            {         
                if (_selectedEditObject) return;

                if (!GetTargetFromInputPosition()) return;
                Log("Select object : " + _selectedEditObject.name);

                ShowEditUI();
                _objectEditState = ObjectSelectorState.WaitForMenuSelect;
            } // End of GetControlTarget

            private bool IsInputOnUI
            {
                get
                {
                    if (GameSetting.Instance.DeviceTarget == DeviceTarget.PC)
                    {
                        return EventSystem.current.IsPointerOverGameObject();
                    }
                    else if (GameSetting.Instance.DeviceTarget == DeviceTarget.Mobile)
                    {
                        foreach (Touch touch in Input.touches)
                        {
                            int id = touch.fingerId;
                            if (EventSystem.current.IsPointerOverGameObject(id))
                            {
                                Debug.Log("Input is On UI");
                                return true;
                            }
                        }
                    }

                    return false;
                }
            }

            private void WaitForMenuSelect()
            {
                if (!ScreenTouch) return;

                // If Choose another object
                if (GetTargetFromInputPosition())
                {
                    _floatingPopUpUI.UpdateEditObject(_selectedEditObject.transform);
                    return;
                }

                if (IsInputOnUI)
                {
                    Log("Input is On UI");
                    return;
                }

                UnSelectTarget();
            } // End of WaitForMenuSelect

            // Ray
            private bool GetTargetFromInputPosition()
            {
                if (!ScreenTouch) return false;

                StartCoroutine(Co_TouchDelay());
                if (!_mainCamera)
                {
                    LogWarning("There is no Camera");
                    return false;
                }
                var ray = _mainCamera.ScreenPointToRay(InputPosition);

                if (!Physics.Raycast(ray, out var hit, float.MaxValue, _targetLayer))
                {
                    return false;
                }
                Log("2");
                Log("Hit object : " + hit.transform.name);

                if (_selectedEditObject)
                {
                    Log("3");
                    Destroy(_selectedEditObject);
                }
                Log("4");
                _selectedEditObject = hit.transform.gameObject.AddComponent<EditObject>();
                return true;
            } // End of GetTargetFromInputPosition

            private void UnSelectTarget()
            {
                Log("UnSelect Target");
                Destroy(_selectedEditObject);
                _selectedEditObject = null;
                _floatingPopUpUI.HideUI();
                _objectEditState = ObjectSelectorState.FindControlTarget;
            } // End of UnSelectTarget

            private IEnumerator Co_TouchDelay()
            {
                _canTouch = false;
                yield return new WaitForSeconds(0.05f);
                _canTouch = true;
            }
            
            // UI
            private void ShowEditUI()
            {
                if (!_floatingPopUpUI)
                {
                    SpawnEditUI();
                    return;
                }

                _floatingPopUpUI.UpdateEditObject(_selectedEditObject.transform);
            } // End of ShowEditUI

            private void SpawnEditUI()
            {
                _floatingPopUpUIPrefab.InstantiateAsync().Completed += (op) =>
                {
                    _floatingPopUpUI = op.Result.GetComponent<FloatingPopUpUI>();
                    _floatingPopUpUI.SetEditor(this);
                    _floatingPopUpUI.UpdateEditObject(_selectedEditObject.transform);
                };
            } // End of SpawnEditUI

            // Debug 
            private bool CheckDebugMode => DebugMode == DebugModeType.Global && !GameSetting.Instance.DebugMode;

            private void Log(string msg)
            {
                if (CheckDebugMode) return;

                Debug.Log("[Object Controller]: " + msg);
            }

            private void LogWarning(string msg)
            {
                if (CheckDebugMode) return;

                Debug.Log("[Object Controller]: " + msg);
            }

            private void DebugRay()
            {
                if (!GameSetting.Instance.DebugMode) return;
                if (!_mainCamera) return;

                var playerPos = transform.position;
                var inputPos = (Vector3) InputPosition;
                inputPos.z = 10f;

                inputPos = _mainCamera.ScreenToWorldPoint(inputPos);
                Debug.DrawRay(playerPos, inputPos - playerPos, Color.blue);
            }

            #endregion Private Methods
        }
    }
}