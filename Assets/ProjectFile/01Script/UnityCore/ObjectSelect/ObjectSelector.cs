using GlobalType;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.EventSystems;

namespace UnityCore
{
    namespace ObjectSelect
    {
        [RequireComponent(typeof(Camera))]
        public class ObjectSelector : MonoBehaviour
        {
            #region Variables

            public DebugModeType DebugMode = DebugModeType.Global;

            // Public Variables

            // Private Variables
            [Header("Prefab")] [SerializeField] private AssetReference _editObjectUIPrefab;

            [Header("Settings")] [SerializeField] private InputType _useInput = InputType.Mouse;
            [SerializeField] private LayerMask _targetLayer;
            [SerializeField] private ObjectSelectorState _objectEditState = ObjectSelectorState.FindControlTarget;

            private FloaingPopUpUI _floaingPopUpUI;
            private EditObject _selectedEditObject;
            private Camera _camera;

            // Input 
            private Vector2 InputPosition
            {
                get
                {
                    switch (_useInput)
                    {
                        case InputType.Mouse:
                            return Input.mousePosition;
                        case InputType.Touch:
                            return Input.GetTouch(0).position;
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
                        case InputType.Touch:
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
                _camera = GetComponent<Camera>();
            } // End of GetComponents

            // Edit State
            private void ObjectControl()
            {
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

            private bool IsInputOnUI => EventSystem.current.IsPointerOverGameObject();

            private void WaitForMenuSelect()
            {
                if (!ScreenTouch) return;

                // If Choose another object
                if (GetTargetFromInputPosition())
                {
                    _floaingPopUpUI.UpdateEditObject(_selectedEditObject);
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

                if (!_camera)
                {
                    LogWarning("There is no Camera");
                    return false;
                }

                var ray = _camera.ScreenPointToRay(InputPosition);

                if (!Physics.Raycast(ray, out var hit, float.MaxValue, _targetLayer))
                {
                    return false;
                }

                Log("Hit object : " + hit.transform.name);

                if (_selectedEditObject)
                {
                    Destroy(_selectedEditObject);
                }

                _selectedEditObject = hit.transform.gameObject.AddComponent<EditObject>();
                return true;
            } // End of GetTargetFromInputPosition

            private void UnSelectTarget()
            {
                Log("UnSelect Target");
                Destroy(_selectedEditObject);
                _selectedEditObject = null;
                _floaingPopUpUI.HideUI();
                _objectEditState = ObjectSelectorState.FindControlTarget;
            } // End of UnSelectTarget

            // UI
            private void ShowEditUI()
            {
                if (!_floaingPopUpUI)
                {
                    SpawnEditUI();
                    return;
                }

                _floaingPopUpUI.ShowUI();
                _floaingPopUpUI.UpdateEditObject(_selectedEditObject);
            } // End of ShowEditUI

            private void SpawnEditUI()
            {
                _editObjectUIPrefab.InstantiateAsync().Completed += (op) =>
                {
                    _floaingPopUpUI = op.Result.GetComponent<FloaingPopUpUI>();
                    _floaingPopUpUI.SetEditor(this);
                    _floaingPopUpUI.UpdateEditObject(_selectedEditObject);
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
                if (!_camera) return;

                var playerPos = transform.position;
                var inputPos = (Vector3) InputPosition;
                inputPos.z = 10f;

                inputPos = _camera.ScreenToWorldPoint(inputPos);
                Debug.DrawRay(playerPos, inputPos - playerPos, Color.blue);
            }

            #endregion Private Methods
        }
    }
}