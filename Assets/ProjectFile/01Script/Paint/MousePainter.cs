using UnityCore.PlayerControl;
using UnityEngine;

namespace Paint
{
    public class MousePainter : MonoBehaviour
    {
        #region Variables

        public bool DebugMode = false;

        // Public Variables
        [SerializeField] private Camera _mainCam;
        [SerializeField] private LayerMask _paintableLayerMask;
        
        // Private Variables

        #endregion Variables

        #region Unity Methods

        private void Start()
        {
            Initialize();
        } // End of Unity - Start

        //private void Update(){} // End of Unity - Update

        #endregion Unity Methods

        #region Public Methods

        #endregion Public Methods

        #region Private Methods

        private void Initialize()
        {
            GetComponents();
            SubscribeInputEvent();
            
            Log("Initialized");
        } // End of Initialize

        private void GetComponents()
        {
            if (!_mainCam) _mainCam = GameObject.FindWithTag("MainCam").GetComponent<Camera>();
        } // End of GetComponents

        private void SubscribeInputEvent()
        {
            ControllerInputs.Instance.OnScreenPressed.AddListener(Paint);
        } // End of SubscribeInputEvent

        private void Paint()
        {
            Log("Trying to paint");
            var inputPosition = ControllerInputs.Instance.InputPosition;
            var ray = _mainCam.ScreenPointToRay(inputPosition);

            if (!Physics.Raycast(ray, out var hitInfo,_paintableLayerMask)) return;

            var paintTarget = hitInfo.transform.GetComponent<Paintable>();
            if(!paintTarget) 
            {
                LogWarning("There is no paintable Target");
                return;
            }
            
            PaintManager.Instance.Paint(paintTarget,hitInfo.point);
        }  // End of Paint
        
        #endregion Private Methods

        #region Debug

        private void Log(string msg)
        {
            if (!DebugMode) return;

            Logger.Log<MousePainter>(msg);
        }

        private void LogWarning(string msg)
        {
            if (!DebugMode) return;

            Logger.LogWarning<MousePainter>(msg);
        }

        #endregion Debug
    }
}