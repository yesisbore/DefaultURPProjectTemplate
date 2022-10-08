using GlobalType;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace UnityCore
{
    namespace ObjectSelect
    {
        public class FloaingPopUpUI : MonoBehaviour
        {
            #region Variables

            public DebugModeType DebugMode = DebugModeType.Global;

            // Public Variables
            
            // Private Variables
            [Header("Prefab")]
            [SerializeField] private AssetReference _indicatorPrefab;

            [Header("Children")] 
            [SerializeField] private RectTransform _editUI;
            [SerializeField] private RectTransform _closeUI;
            
            private Button[] _editButtons;
            private Button _closeButton;
            private ObjectSelector _objectSelector;
            private EditObject _editObject;
            private Transform _mainUI;
            private Transform _indicator;

            #endregion Variables

            #region Unity Methods

            private void Update()
            {
                FollowEditObject();
            } // End of Unity - Update

            #endregion Unity Methods
            
            #region Public Methods

            public void SetEditor(ObjectSelector selector) => _objectSelector = selector;
            public void UpdateEditObject(EditObject editObject)
            {
                Log("Update Edit Object");

                _editObject = editObject;
                this.gameObject.name = _editObject.name + " - Edit UI";

                SetButtonEvents();
                UpdateProcess();
            } // End of UpdateEditObject

            public void ShowUI()
            {
                gameObject.SetActive(true);
            } // End of ShowUI

            public void HideUI()
            {
                DeactivateIndicator();
                gameObject.SetActive(false);
            } // End of HideUI

            // Edit Button Events
            private void OnClickMove()
            {
                HideEditUI();
                _objectSelector.MoveObject();
            } // End of OnClickMove
            private void OnClickRotate()
            {
                HideEditUI();
                _objectSelector.RotateObject();
            } // End of OnClickRotate
            private void OnClickScale()
            {
                HideEditUI();
                _objectSelector.ScaleObject();
            } // End of OnClickScale
            private void OnClickClose()
            {
                HideCloseUI();
                _objectSelector.StopEditing();
            } // End of OnClickClose

            #endregion Public Methods

            #region Private Methods

            private void SetButtonEvents()
            {
                GetButtons();
                
                _editButtons[0].onClick.AddListener(OnClickMove);
                _editButtons[1].onClick.AddListener(OnClickRotate);
                _editButtons[2].onClick.AddListener(OnClickScale);
                _closeButton.onClick.AddListener(OnClickClose);
            } // End of SetButtonEvents
            private void GetButtons()
            {
               _editButtons = _editUI.GetComponentsInChildren<Button>(true);
               _closeButton = _closeUI.GetComponentInChildren<Button>(true);
            } // End of GetButtons
            private void UpdateProcess()
            {
                GetMainUI();
                ActivateIndicator();
                ShowEditUI();
            } // End of GetComponents
            private void GetMainUI()
            {
                if(_mainUI) return;
                
                _mainUI = GameManager.GetMainUI().transform;
                this.transform.SetParent(_mainUI);
                Log("Get Main UI : " + _mainUI.name);
            } // End of GetMainUI

            private void FollowEditObject()
            {
                if(!_editObject) return;
                
            } // End of FollowEditObject
            
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
                _indicatorPrefab.InstantiateAsync().Completed += (op) =>
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

            // Edit UI Button
            private void ShowEditUI()
            {
                _editUI.gameObject.SetActive(true);
            } // End of ShowEditButtons
            
            private void HideEditUI()
            {
                _editUI.gameObject.SetActive(false);

                ShowCloseUI();
            } // End of HideEditButtons

            private void ShowCloseUI()
            {
                _closeUI.gameObject.SetActive(true);
            } // End of ShowCloseButton

            private void HideCloseUI()
            {
                _closeUI.gameObject.SetActive(false);

                ShowEditUI();
            } // End of HideCloseButton

            
            // Debug
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