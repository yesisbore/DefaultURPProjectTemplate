using GlobalType;
using UnityCore.ObjectSelect;
using UnityEngine;

namespace UnityCore
{
    namespace UI
    {
        public class Indicator : MonoBehaviour
        {
           
            #region Variables
         
            public DebugModeType DebugMode = DebugModeType.Global;
         
            // Public Variables
            
            // Private Variables
            private FloatingPopUpUIPosition _floatingPopUpUIPosition ;
            private Transform _targetTransform;
            
            #endregion Variables
        
            #region Unity Methods

            private void Update()
            {
                AdjustIndicatorProportion();
            } // End of Unity - Update
        
            #endregion Unity Methods
        
            #region Public Methods

            public void UpdateIndicator(Transform target,FloatingPopUpUIPosition fPosition)
            {
                _targetTransform = target;
                _floatingPopUpUIPosition = fPosition;
                
                DeactivateIndicator();
                AdjustIndicatorProportion();
                gameObject.SetActive(true);
            }
            public void DeactivateIndicator()
            {
                Log("Deactivate Indicator");
                gameObject.SetActive(false);
            } // End of DeactivateIndicator
            

            #endregion Public Methods
            
            #region Private Methods

            private void AdjustIndicatorProportion()
            {
                if (!_targetTransform) return;

                var multipleValue = Vector3.Distance(_floatingPopUpUIPosition.GetPosition(UIPositionType.LowerLeft)
                    , _floatingPopUpUIPosition.GetPosition(UIPositionType.LowerRight)) * 0.5f;
                var newScale = new Vector3(multipleValue * 2.0f, 1.0f, multipleValue * 2.0f);

                transform.position = _floatingPopUpUIPosition.GetPosition(UIPositionType.LowerCenter);
                transform.localScale = newScale;
            } // End of AdjustIndicatorProportion
        
            #endregion Private Methods
        
            #region Debug
        
            private bool CheckDebugMode => DebugMode == DebugModeType.Global && !GameSetting.Instance.DebugMode;
            private void Log(string msg)
            {
                if(CheckDebugMode) return;
            
                Debug.Log("[Indicator]: " + msg);
            }
            
            private void LogWarning(string msg)
            {
                if(CheckDebugMode) return;
                        
                Debug.Log("[Indicator]: " + msg);
            }
        
            #endregion Debug
        }
    }
}

