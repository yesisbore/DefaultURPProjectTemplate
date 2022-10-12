using System.Collections;
using GlobalType;
using UnityCore.PlayerControl;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using DeviceType = GlobalType.DeviceType;

namespace UnityCore
{
    namespace ObjectSelect
    {
        [RequireComponent(typeof(PlayerInput))]
        [RequireComponent(typeof(ControllerInputs))]
        public class ObjectSelector : MonoBehaviour
        {
            #region Variables

            public bool DebugMode = false;

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

            private bool _initialized = false;
            
            // Input
            private ControllerInputs _controllerInputs;

            private Vector2 InputPosition => _controllerInputs.inputPosition;
            private bool ScreenPressed
            {
                get
                {
                    if (!_controllerInputs.screenPressed) return false;
                    
                    _controllerInputs.screenPressed = false;
                    return true;

                }
            }

            #endregion Variables

            #region Unity Methods

            private void Start()
            {
                Initialize();
            } // End of Unity - Start

            private void Update()
            {
                if(!_initialized) return;
                    
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
                
                _initialized = true;
            } // End of Initialize

            private void GetComponents()
            {
                _mainCamera = Camera.main;
                _controllerInputs = GetComponent<ControllerInputs>();
                
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
                    if (_controllerInputs.CurrentDevice == DeviceType.PC)
                    {
                        return EventSystem.current.IsPointerOverGameObject();
                    }
                    else if (_controllerInputs.CurrentDevice == DeviceType.Touch)
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
                if (!ScreenPressed) return;

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
                if (!ScreenPressed) return false;

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
            private void Log(string msg)
            {
                if(!DebugMode) return;
            
                Logger.Log<ObjectSelector>( msg);
            }
            
            private void LogWarning(string msg)
            {
                if(!DebugMode) return;
                        
                Logger.LogWarning<ObjectSelector>(msg);
            }

            private void DebugRay()
            {
                if (!DebugMode) return;
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