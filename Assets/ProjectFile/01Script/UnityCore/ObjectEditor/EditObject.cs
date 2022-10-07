using System.Collections;
using System.Collections.Generic;
using GlobalType;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UnityCore
{
    namespace ObjectEditor
    {
        public class EditObject : MonoBehaviour
        {
            #region Variables

            public DebugModeType DebugMode = DebugModeType.Global;

            // Public Variables

            // Private Variables
            [SerializeField] private ObjectEditState _objectEditState = ObjectEditState.WaitForMenuSelect;
            private Transform _mainUI;
            private EditObjectUI _editObjectUI;

            #endregion Variables

            #region Unity Methods

            private void Update()
            {
                ObjectControl();
            } // End of Unity - Update

            #endregion Unity Methods

            #region Public Methods

            public void Move() => _objectEditState = ObjectEditState.Move;
            
            public void Rotate() => _objectEditState = ObjectEditState.Move;
            
            public void Scale() => _objectEditState = ObjectEditState.Move;
            
            public void StopEditing()
            {
                _objectEditState = ObjectEditState.WaitForMenuSelect;
            }
            
            #endregion Public Methods

            #region Private Methods

            private void Initialize()
            {
                GetComponents();
            } // End of Initialize

            private void GetComponents()
            {
                
            } // End of GetComponents

            
            // Edit State
            private void ObjectControl()
            {
                switch (_objectEditState)
                {
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
            } // End of MoveObject

            private void RotateObject()
            {
            } // End of RotateObject

            private void ScaleObject()
            {
            } // End of ScaleObject

            private bool CheckDebugMode => DebugMode == DebugModeType.Global && !GameSetting.Instance.DebugMode;

            private void Log(string msg)
            {
                if (CheckDebugMode) return;

                Debug.Log("[Edit Object]: " + msg);
            }

            private void LogWarning(string msg)
            {
                if (CheckDebugMode) return;

                Debug.Log("[Edit Object]: " + msg);
            }

            #endregion Private Methods
        }
    }
}