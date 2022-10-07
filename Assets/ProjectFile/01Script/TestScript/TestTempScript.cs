using GlobalType;
using UnityEngine;

namespace TestScript
{
    public class TestTempScript : MonoBehaviour
    {
        #region Variables

        // Public Variables

        public DebugModeType DebugMode = DebugModeType.Global;

        public Canvas Canvas;
        public GameObject UITop;
        public GameObject UIBottom;

        public GameObject TargetObject;
        public GameObject Picker1;
        public GameObject Picker2;

        // Private Variables

        private Camera _camera;

        #endregion Variables

        #region Unity Methods

        private void Start()
        {
            Initialize();
        } // End of Unity - Start

        private void Update()
        {
            UpdateUIPosition();
        } // End of Unity - Update

        #endregion Unity Methods

        #region Public Methods

        #endregion Public Methods

        #region Private Methods

        private void Initialize()
        {
            GetComponents();
            CalcUIMovableBounds();
            ShowUi();
        } // End of Initialize

        private void GetComponents()
        {
            _camera = Camera.main;
        } // End of GetComponents

        public class MovableBounds
        {
            public float minX;
            public float minY;
            public float maxX;
            public float maxY;
        }

        // UI 
        private RectTransform UIRectTop;
        private RectTransform UIRectBottom;
        private Vector2 UISize;
        
        public MovableBounds movableBounds = new MovableBounds();

        private void CalcUIMovableBounds()
        {
            var screenWidth = Screen.width;
            var screenHeight = Screen.height;
            
            UIRectTop = UITop.GetComponent<RectTransform>();
            UIRectTop.pivot = Vector2.zero;
            UIRectTop.anchoredPosition = Vector2.zero;
            UISize = UIRectTop.rect.size;
            
            movableBounds.minX = UISize.x / 2f;
            movableBounds.maxX = screenWidth - UISize.x;
            movableBounds.minY = UISize.y / 2f;
            movableBounds.maxY = screenHeight - UISize.y;
        } // End of CalcUIMovableBounds
        
        
        
        
        private Transform _targetTransform ;
        private Bounds _localBounds;
        private Vector3   TargetPosition => _targetTransform.position;
        private float _relativeHeight;
        
        // 스케일 조정 시 변경되어야함 
        private Vector3 _targetTopPosition;
        private Vector3 _targetBottomPosition;

        private Vector2 ScreenPosTop
        {
            get
            {
                var rValue = _camera.WorldToScreenPoint(_targetTopPosition);
                rValue.x -= UISize.x / 2f;
                rValue.y -= UISize.y / 2f;
                return rValue;
            }
        }
        private Vector2 ScreenPosBottom
        {
            get
            {
                var rValue = _camera.WorldToScreenPoint(_targetBottomPosition);
                rValue.x -= UISize.x / 2f;
                rValue.y -= UISize.y / 2f;
                return rValue;
            }
        }
        private void ShowUi()
        {
            _targetTransform = TargetObject.transform;
            _localBounds = TargetObject.GetComponent<MeshFilter>().mesh.bounds;
            _relativeHeight = _localBounds.extents.y * _targetTransform.localScale.y;
            UIRectBottom = UIBottom.GetComponent<RectTransform>();
        } // End of Show Ui
        
        private void UpdateUIPosition()
        {
            _targetTopPosition = TargetPosition + new Vector3(0.0f, _relativeHeight, 0.0f);
            _targetBottomPosition = TargetPosition - new Vector3(0.0f, _relativeHeight, 0.0f);
            
            Picker1.transform.position = _targetTopPosition;
            Picker2.transform.position = _targetBottomPosition;
            
            UIRectTop.anchoredPosition = ScreenPosTop;
            UIRectBottom.anchoredPosition = ScreenPosBottom;
            // var clampX = Mathf.Clamp(TargetTopScreenPos.x, movableBounds.minX, movableBounds.maxX);
            // var clampY = Mathf.Clamp(TargetTopScreenPos.y, movableBounds.minY, movableBounds.maxY);
            // var newPos = new Vector2(clampX, clampY);
            //
            // UIPrefab.GetComponent<RectTransform>().anchoredPosition = newPos;
            // Log("Screen Pos : " + TargetTopScreenPos);
        } // ENd of UpdateUIPosition

        private bool CheckDebugMode => DebugMode == DebugModeType.Global && !GameSetting.Instance.DebugMode;

        private void Log(string msg)
        {
            if (CheckDebugMode) return;

            Debug.Log("[Test Temp Script]: " + msg);
        }

        private void LogWarning(string msg)
        {
            if (CheckDebugMode) return;

            Debug.Log("[Test Temp Script]: " + msg);
        }

        #endregion Private Methods
    }
}