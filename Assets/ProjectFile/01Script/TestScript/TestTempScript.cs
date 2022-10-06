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
        public GameObject UIPrefab;

        public GameObject TargetObject;

        private float _relativeHeight;
        
        // Private Variables

        private Camera _camera;
        
        #endregion Variables
    
        #region Unity Methods
    
        private void Start() { Initialize();} // End of Unity - Start

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
            ShowUi();
            CalcUIMovableBounds();

        } // End of Initialize

        private void GetComponents()
        {
            _camera = Camera.main;
        } // End of GetComponents
        
        private Vector3 TargetTopPos => TargetObject.transform.position + new Vector3(0.0f,_relativeHeight,0.0f);
        private Vector2 TargetTopScreenPos => _camera.WorldToScreenPoint(TargetTopPos);

        private void ShowUi()
        {
            var targetTransform = TargetObject.transform;
            var targetExtents = TargetObject.GetComponent<MeshFilter>().mesh.bounds.extents;
            _relativeHeight = targetExtents.y * targetTransform.localScale.y;
        } // End of Show Ui
        
        public class MovableBounds
        {
            public float minX; 
            public float minY;  
            public float maxX; 
            public float maxY;
        }
        public MovableBounds movableBounds = new MovableBounds();

        private void CalcUIMovableBounds()
        {
            var screenWidth = Screen.width;
            var screenHeight = Screen.height;
            
            var uiRect = UIPrefab.GetComponent<RectTransform>();
            var uiSize = uiRect.rect.size;

            uiRect.pivot = Vector2.zero;
            uiRect.anchoredPosition = Vector2.zero;
            
            movableBounds.minX = uiSize.x / 2f ;
            movableBounds.maxX = screenWidth - uiSize.x ;
            movableBounds.minY = uiSize.y /2f ;
            movableBounds.maxY = screenHeight - uiSize.y ;
        } // End of CalcUIMovableBounds
        
        private void UpdateUIPosition()
        {
            return;
            var clampX = Mathf.Clamp(TargetTopScreenPos.x, movableBounds.minX, movableBounds.maxX);
            var clampY = Mathf.Clamp(TargetTopScreenPos.y, movableBounds.minY, movableBounds.maxY);
            var newPos = new Vector2(clampX, clampY);
            
            UIPrefab.GetComponent<RectTransform>().anchoredPosition = newPos;
            Log("Screen Pos : " + TargetTopScreenPos);
        } // ENd of UpdateUIPosition
        private bool CheckDebugMode => DebugMode == DebugModeType.Global && !GameSetting.Instance.DebugMode;
        private void Log(string msg)
        {
            if(CheckDebugMode) return;
                    
            Debug.Log("[Test Temp Script]: " + msg);
        }
        
        private void LogWarning(string msg)
        {
            if(CheckDebugMode) return;
                    
            Debug.Log("[Test Temp Script]: " + msg);
        }
        
        #endregion Private Methods
    }
}

