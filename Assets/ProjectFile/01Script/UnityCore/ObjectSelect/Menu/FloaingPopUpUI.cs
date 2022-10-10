using System.Collections.Generic;
using GlobalType;
using Unity.Mathematics;
using UnityCore.UI;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UI;

namespace UnityCore
{
    namespace ObjectSelect
    {
        [RequireComponent(typeof(RectTransform))]
        public class FloaingPopUpUI : MonoBehaviour
        {
            #region Variables

            public DebugModeType DebugMode = DebugModeType.Global;

            // Public Variables

            // Private Variables
            [Header("Settings")]
            [SerializeField] private AssetReference _indicatorPrefab;

            [Header("Children")] 
            [SerializeField] private RectTransform _editUI;
            [SerializeField] private RectTransform _closeUI;

            private FloatingPopUpUIPosition _floatingPopUpUIPosition ;
            private MovableScreen _movableScreen;
            private RectTransform _rootRect;
            private Button[] _editButtons;
            private Button _closeButton;
            private ObjectSelector _objectSelector;
            private Transform _editObject;
            private Transform _mainUI;
            private Indicator _indicator;
            private Camera _mainCamera;

            #endregion Variables

            #region Unity Methods

            private void Update()
            {
                FollowEditObject();
            } // End of Unity - Update

            #endregion Unity Methods
            
            #region Public Methods

            public void SetEditor(ObjectSelector selector) => _objectSelector = selector;
            public void UpdateEditObject(Transform editObject)
            {
                Log("Update Edit Object");

                _editObject = editObject;
                this.gameObject.name = _editObject.name + " - Edit UI";
                HideUI();
                SetButtonEvents();
                UpdateProcess();
            } // End of UpdateEditObject
            
            public void HideUI()
            {
                if (_indicator)
                {
                    _indicator.DeactivateIndicator();
                }
                
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

            private void UpdateProcess()
            {
                GetComponents();
                ActivateUI();
                ActivateIndicator();
            } // End of GetComponents
            private void GetComponents()
            {
                if (_mainCamera == null)
                {
                    _mainCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
                }

                if (_rootRect == null)
                {
                    _rootRect = GetComponent<RectTransform>();
                }
                GetMainUI();
            } // End of GetComponents
            private void GetMainUI()
            {
                if(_mainUI) return;
                
                _mainUI = GameManager.GetMainUI().transform;
                this.transform.SetParent(_mainUI);
                Log("Get Main UI : " + _mainUI.name);
            } // End of GetMainUI
            private void ActivateUI()
            {
                InitializeUI();
                ShowEditUI();
                ShowUI();
            } // End of ActivateUI

            private void InitializeUI()
            {
                _rootRect.pivot = Vector2.zero;
                _rootRect.anchoredPosition = Vector2.zero;
                if (_movableScreen == null)
                {
                    _movableScreen = new MovableScreen(_rootRect.rect.size);
                }
                // 변경되면 삭제 후 초기화 필요 업데이트 계속 돌아가서 - 그냥 오브젝트 변경만 해도될듯 
                if (_floatingPopUpUIPosition == null)
                {
                    _floatingPopUpUIPosition = new FloatingPopUpUIPosition(_editObject);
                    return;
                }
                _floatingPopUpUIPosition.UpdateTargetObject(_editObject);
            } // End of InitializeUI

            private void ShowUI()
            {
                gameObject.SetActive(true);
            } // End of ShowUI

            private void FollowEditObject()
            {
                if(!_editObject || !_mainCamera) return;
                
                _rootRect.anchoredPosition = _mainCamera.WorldToScreenPoint(_floatingPopUpUIPosition.GetPosition(UIPositionType.UpperRight));
            } // End of FollowEditObject
            
            #endregion Private Methods
            
            #region Menu Button
            
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
            
            #endregion Menu Button

            #region Indicator

            private void ActivateIndicator()
            {
                Log("Activate Indicator");

                if (!_indicator)
                {
                    SpawnIndicator();
                    return;
                }
                _indicator.UpdateIndicator(_editObject,_floatingPopUpUIPosition);
            } // End of ActivateIndicator
            
            private void SpawnIndicator()
            {
                _indicatorPrefab.InstantiateAsync().Completed += (op) =>
                {
                    _indicator = op.Result.GetComponent<Indicator>();
                    _indicator.UpdateIndicator(_editObject,_floatingPopUpUIPosition);
                };
            } // End of SpawnIndicator
            
            
            #endregion Indicator
            
            #region Debug

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

            #endregion Debug
        }
    }
}