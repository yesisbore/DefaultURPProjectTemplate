using System.Collections;
using System.Collections.Generic;
using GlobalType;
using UnityEngine;

namespace TestScript
{
    public class TestTempScript : MonoBehaviour
    {
        #region Variables
    
        // Public Variables
        
        public DebugModeType DebugMode = DebugModeType.Global;
        
        public GameObject Indicator;

        public GameObject TargetObject;
        
        // Private Variables
        
        #endregion Variables
    
        #region Unity Methods
    
        private void Start() { Initialize();} // End of Unity - Start
    
        //private void Update(){} // End of Unity - Update
    
        #endregion Unity Methods
    
        #region Public Methods
    
        #endregion Public Methods
        
        #region Private Methods

        private void Initialize()
        {
            GetInformation();
        } // End of Initialize

        private void GetInformation()
        {
            GetTargetInfo();
        } // End of GetInformation

        private void GetTargetInfo()
        {
            var renderer = TargetObject.GetComponent<MeshRenderer>();
            var bounds = renderer.bounds;
            
            Log("Target Bounds Info : " + bounds);
            Log("Target Bounds Center : " + bounds.center);
            Log("Target Bounds Min : " + bounds.min);
            Log("Target Bounds Max : " + bounds.max);
            Log("Target Bounds Size : " + bounds.size);
        } // End of GetTargetInfo
        
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

