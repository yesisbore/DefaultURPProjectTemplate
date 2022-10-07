using GlobalType;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace UnityCore
{
    namespace ObjectEditor
    {
        public class EditObjectUI : MonoBehaviour
        {
            #region Variables

            public DebugModeType DebugMode = DebugModeType.Global;

            // Public Variables
            
            // Private Variables
            [SerializeField] private ObjectEditState _objectEditState = ObjectEditState.WaitForMenuSelect;
            [SerializeField] private string _indicatorName = "Indicator";

            private Transform _indicator;
            private EditObject _editObject;
            private Transform _mainUI;
            private RectTransform _editUI;

            #endregion Variables

            #region Unity Methods

            private void Update()
            {
                //ObjectControl();
            } // End of Unity - Update

            #endregion Unity Methods

            #region Public Methods

            public void UpdateEditObject(EditObject editObject)
            {
                Log("Update Edit Object");

                _editObject = editObject;
                this.gameObject.name = _editObject.name + " - Edit UI";
                UpdateProcess();
            } // End of UpdateEditObject

            public void OnUI()
            {
                gameObject.SetActive(true);
            } // End of OnUI

            public void OffUI()
            {
                gameObject.SetActive(false);
            } // End of OffUI
            
            // Edit Button Events
            public void OnClickMove() => _editObject.SetEditState(_objectEditState = ObjectEditState.Move);
            public void OnClickRotate() => _editObject.SetEditState(_objectEditState = ObjectEditState.Rotate);
            public void OnClickScale() => _editObject.SetEditState(_objectEditState = ObjectEditState.Scale);
            public void OnClickClose()
            {
                HideCloseButton();
                _editObject.SetEditState(_objectEditState = ObjectEditState.WaitForMenuSelect);
            } // End of OnClickClose
            
            
            
            #endregion Public Methods

            #region Private Methods



            private void UpdateProcess()
            {
                GetMainUI();
                ActivateIndicator();
                OnUI();
            } // End of GetComponents

            private void GetMainUI()
            {
                if(_mainUI) return;
                
                _mainUI = GameManager.GetMainUI().transform;
                this.transform.SetParent(_mainUI);
                Log("Get Main UI : " + _mainUI.name);
            } // End of GetMainUI
            
            // Indicator
            private void ActivateIndicator()
            {
                Log("Activate Indicator");

                if (!_indicator)
                {
                    SpawnIndicator();
                    return;
                }
                UpdateIndicator();
            } // End of ActivateIndicator

            private void DeactivateIndicator()
            {
                if (!_indicator) return;
                
                Log("Deactivate Indicator");
                _indicator.gameObject.SetActive(false);
                _indicator.SetParent(null);
            } // End of DeactivateIndicator
            
            private void SpawnIndicator()
            {
                Addressables.InstantiateAsync(_indicatorName).Completed += (op) =>
                {
                    _indicator = op.Result.transform;
                    UpdateIndicator();
                };
            } // End of SpawnIndicator
            
            private void UpdateIndicator()
            {
                DeactivateIndicator();
                AdjustIndicatorProportion();
                _indicator.gameObject.SetActive(true);
            } // End of UpdateIndicator

            private void AdjustIndicatorProportion()
            {
                var indicatorTransform = _indicator.transform;
                var targetTransform = _editObject.transform;
                var targetPosition = targetTransform.position;
                var targetExtents = targetTransform.GetComponent<MeshFilter>().mesh.bounds.extents;
                
                // Calculate - Get world scale extents
                var extents = targetTransform.TransformVector(targetExtents);
                extents.x = Mathf.Abs(extents.x);
                extents.y = Mathf.Abs(extents.y);
                extents.z = Mathf.Abs(extents.z);
                Log("extents.x : " + extents.x + "extents.y : " + extents.y + "extents.z : " + extents.z );
                
                // Position
                var newPosition = new Vector3(targetPosition.x, targetPosition.y - extents.y, targetPosition.z) ;
                
                // Scale
                var multipleValue = extents.x >= extents.z ? extents.x : extents.z;
                var newScale = new Vector3(multipleValue * 2.0f, 1.0f, multipleValue * 2.0f);
                    
                indicatorTransform.position = newPosition;
                indicatorTransform.localScale = newScale;
                indicatorTransform.parent = _editObject.transform;
                
            } // End of AdjustIndicatorProportion
            


            
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

                Debug.Log("[EditObject UI]: " + msg);
            }

            private void LogWarning(string msg)
            {
                if (CheckDebugMode) return;

                Debug.Log("[EditObject UI]: " + msg);
            }

            #endregion Private Methods
        }
    }
}