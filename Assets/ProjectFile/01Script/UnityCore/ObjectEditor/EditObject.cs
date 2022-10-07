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
            [SerializeField] private string _editObjectUIName = "EditObject UI";
            private ObjectEditor _objectEditor;
            private Transform _mainUI;
            private EditObjectUI _editObjectUI;

            #endregion Variables

            #region Unity Methods

            // private void Start()
            // {
            //     Initialize();
            // } // End of Unity - Start

            // private void Update()
            // {
            //     //ObjectControl();
            // } // End of Unity - Update

            #endregion Unity Methods

            #region Public Methods

            public void SetEditState(ObjectEditState state) => _objectEditor.SetEditState(_objectEditState = state);
            
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
                HideEditButtons();
            } // End of MoveObject

            private void RotateObject()
            {
                HideEditButtons();
            } // End of RotateObject

            private void ScaleObject()
            {
                HideEditButtons();
            } // End of ScaleObject


            // Edit UI Button
            private void ShowEditButtons()
            {
            } // End of ShowEditButtons

            private void HideEditButtons()
            {
                ShowCloseButton();
            } // End of HideEditButtons

            private void ShowCloseButton()
            {
            } // End of ShowCloseButton

            private void HideCloseButton()
            {
                ShowEditButtons();
            } // End of HideCloseButton

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