using System.Collections;
using System.Collections.Generic;
using GlobalType;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.Rendering;

namespace UnityCore
{
    namespace ObjectSelect
    {
        public class EditObject : MonoBehaviour
        {
            #region Variables

            public bool DebugMode = false;

            // Public Variables

            // Private Variables
            [SerializeField] private ObjectEditState _objectEditState = ObjectEditState.None;

            private Transform _mainCamTransform;
            private Transform _mainUI;
            private FloatingPopUpUI _floatingPopUpUI;
            private bool _isInitializedTouchDistance = false;
            private float _beforeTouchDistance = 0f;

            private float _moveSensitivity = 0.02f;
            private float _scaleSensitivity = 0.03f;
            private float _scaleRange = 1.0f;

            // Lambda
            private bool IsMainCamTransformNull => _mainCamTransform == null;

            private Vector2 Touch01 => Input.GetTouch(0).position;
            private Vector2 Touch02
            {
                get
                {
                    if ( Input.touchCount == 1)
                    {
                        return Touch01;
                    }
                    return Input.GetTouch(1).position;
                }
            }

            private float CurTouchDistance => Vector2.Distance(Touch01, Touch02);
            private bool NotTouched => Input.touchCount == 0;
            
            #endregion Variables

            #region Unity Methods

            private void Start()
            {
                Initialize();
            } // End of Unity - Start
            
            private void Update()
            {
                ObjectControl();
            } // End of Unity - Update

            #endregion Unity Methods

            #region Public Methods

            public void Move() => _objectEditState = ObjectEditState.Move;
            
            public void Rotate() => _objectEditState = ObjectEditState.Rotate;
            
            public void Scale() => _objectEditState = ObjectEditState.Scale;
            
            public void StopEditing()
            {
                _objectEditState = ObjectEditState.None;
            }
            
            #endregion Public Methods

            #region Private Methods

            private void Initialize()
            {
                GetComponents();
            } // End of Initialize

            private void GetComponents()
            {
                if (Camera.main != null) _mainCamTransform = Camera.main.transform;
            } // End of GetComponents
            
            // Edit State
            private void ObjectControl()
            {
                switch (_objectEditState)
                {
                    case ObjectEditState.None:
                        break;
                    case ObjectEditState.Move:
                        MoveObject();
                        break;
                    case ObjectEditState.Rotate:
                        RotateObject();
                        break;
                    case ObjectEditState.Scale:
                        ScaleObject();
                        break;
                }
            } // End of ObjectControl

            private void MoveObject()
            {
                if(IsMainCamTransformNull) return;
                if(NotTouched) return;
                
                var delta = Input.GetTouch(0).deltaPosition;

                var newX = _mainCamTransform.right * delta.x;
                var newZ = _mainCamTransform.forward * delta.y;
                var addPos = newX + newZ;

                transform.position += addPos * _moveSensitivity;
            } // End of MoveObject

            private void RotateObject()
            {
                if(IsMainCamTransformNull) return;
                if(NotTouched) return;
                
                var delta = Input.GetTouch(0).deltaPosition;

                var newY = Vector3.up * delta.y;
                var newRotation = transform.eulerAngles.y - delta.x;

                transform.position += newY * _moveSensitivity;
                transform.rotation = Quaternion.Euler(new Vector3(0.0f,newRotation,0.0f)); 
            } // End of RotateObject

            private void ScaleObject()
            {
                if(IsMainCamTransformNull) return;
                if(NotTouched) return;
                
                if (!_isInitializedTouchDistance)
                {
                    SaveBeforeTouchDistance();
                    _isInitializedTouchDistance = true;
                }

                var differenceValue = CurTouchDistance - _beforeTouchDistance;
                SaveBeforeTouchDistance();

                if (differenceValue >= _scaleRange)
                {
                    // Scale Up
                    transform.localScale *= 1f + _scaleSensitivity;
                    return;
                }
                if (differenceValue <= -_scaleRange)
                {
                    // Scale Down
                    transform.localScale *= 1f - _scaleSensitivity;
                }
            } // End of ScaleObject

            private void SaveBeforeTouchDistance() => _beforeTouchDistance = CurTouchDistance;
            
            // Debug
            private void Log(string msg)
            {
                if(!DebugMode) return;
            
                Logger.Log<EditObject>( msg);
            }
            
            private void LogWarning(string msg)
            {
                if(!DebugMode) return;
                        
                Logger.LogWarning<EditObject>(msg);
            }

            #endregion Private Methods
        }
    }
}