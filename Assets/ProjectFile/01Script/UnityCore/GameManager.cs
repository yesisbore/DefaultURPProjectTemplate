using GlobalType;
using UnityEngine;
using UnityEngine.UI;

namespace UnityCore
{
    public class GameManager : Singleton<GameManager>
    {
        #region Variables
 
        public DebugModeType DebugMode = DebugModeType.Global;
 
        // Public Variables
    
        // Private Variables
    
        #endregion Variables

        #region Unity Methods

        //private void Start() { Initialize();} // End of Unity - Start

        //private void Update(){} // End of Unity - Update

        #endregion Unity Methods

        #region Public Methods

        
        // UI
        public static GameObject GetMainUI()
        {
            GameObject _mainUI = null;
            
            _mainUI = GameObject.FindWithTag("Main UI");

            if (_mainUI) return _mainUI;
            
            _mainUI = FindObjectOfType<Canvas>()?.gameObject;
            if (!_mainUI)
            {
                _mainUI = CreateNewMainUI();
            }
            _mainUI.tag = "Main UI";
            return _mainUI;
        } // End of GetMainUI
        public static GameObject CreateNewMainUI()
        {
            var go = new GameObject("Main UI");
            var canvas = go.AddComponent<Canvas>();
            var canvasScalar = go.AddComponent<CanvasScaler>();
            var raycaster = go.AddComponent<GraphicRaycaster>();

            canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            canvasScalar.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            canvasScalar.referenceResolution = GetResolution();
            CreateEventSystem();
            return go;
        } // End of CreateNewMainUI
        public static Vector2 GetResolution()
        {
            var rValue = Vector2.zero;

            switch (GameSetting.Instance.TargetResolution)
            {
                case TargetResolution.HD:
                    rValue = new Vector2(1280, 720);
                    break;
                case TargetResolution.FHD:
                    rValue = new Vector2(1920, 1080);
                    break;
                case TargetResolution.FourK:
                    rValue = new Vector2(3840, 2160);
                    break;
                case TargetResolution.EightK:
                    rValue = new Vector2(7680, 4320);
                    break;
            }

            return rValue;
        } // End of GetResolution
        private static void CreateEventSystem()
        {
            GameObject eventSystem = GameObject.Find("EventSystem");
            if (eventSystem != null)
                return;

            GameObject newEventSystem = new GameObject("EventSystem");
            newEventSystem.AddComponent<UnityEngine.EventSystems.EventSystem>();
            newEventSystem.AddComponent<UnityEngine.EventSystems.StandaloneInputModule>();
        } // End of CreateEventSystem
        
        #endregion Public Methods
    
        #region Private Methods


        
        private bool CheckDebugMode => DebugMode == DebugModeType.Global && !GameSetting.Instance.DebugMode;
        private void Log(string msg)
        {
            if(CheckDebugMode) return;
        
            Debug.Log("[Game Manager]: " + msg);
        }
        
        private void LogWarning(string msg)
        {
            if(CheckDebugMode) return;
                    
            Debug.Log("[Game Manager]: " + msg);
        }
    
        #endregion Private Methods
    }
}

