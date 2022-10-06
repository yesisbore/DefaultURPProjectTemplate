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
            var indicatorTransform = Indicator.transform;
            var target = TargetObject.transform;
            var targetPosition = target.position;
            
            var bounds = target.GetComponent<MeshFilter>().mesh.bounds;
 

            // transform the local extents' axes
            var extents = bounds.extents;
            Log("Bounds : " + bounds );

            var axisX = target.TransformVector(extents.x, 0, 0);
            var axisY = target.TransformVector(0, extents.y, 0);
            var axisZ = target.TransformVector(0, 0, extents.z);
 
            // sum their absolute value to get the world extents
            extents.x = Mathf.Abs(axisX.x) + Mathf.Abs(axisY.x) + Mathf.Abs(axisZ.x);
            extents.y = Mathf.Abs(axisX.y) + Mathf.Abs(axisY.y) + Mathf.Abs(axisZ.y);
            extents.z = Mathf.Abs(axisX.z) + Mathf.Abs(axisY.z) + Mathf.Abs(axisZ.z);

            var scaleX = extents.x *2 * Indicator.transform.localScale.x;
            var scaleZ = extents.z * 2*Indicator.transform.localScale.z;
            
            indicatorTransform.localScale = new Vector3(scaleX, 1.0f, scaleZ);
            indicatorTransform.position = new Vector3(targetPosition.x, targetPosition.y - extents.y, targetPosition.z) ;

            Log("axisX : " + axisX + "axisY : " + axisY + "axisZ : " + axisX );
            Log("extents.x : " + extents.x + "extents.y : " + extents.y + "extents.z : " + extents.z );
            
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

