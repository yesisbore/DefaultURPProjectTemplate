using GlobalType;
using UnityEngine;

namespace UnityCore
{
    namespace PlayerControl
    {
        public class JoyStick : MonoBehaviour
        {
               
            #region Variables

            public bool DebugMode = false;
 
            // Public Variables
    
            // Private Variables
    
            #endregion Variables

            #region Unity Methods

            //private void Start() { Initialize();} // End of Unity - Start

            //private void Update(){} // End of Unity - Update

            #endregion Unity Methods

            #region Public Methods

            #endregion Public Methods
    
            #region Private Methods

            //private void Initialize(){GetComponents();} // End of Initialize
    
            //private void GetComponents(){} // End of GetComponents

            // private IEnumerator CO_Lerp()
            // {
            // var initialValue = 1.0f;
            // var targetValue =  0.0f;
            // var duration = 1.0f;
            // var timer = 0.0f;
            //
            // while (timer < duration)
            // {
            //      = Mathf.Lerp(initialValue, targetValue, timer / duration);
            // timer += Time.deltaTime;
            // yield return null;
            // }
            //     Destroy(gameObject);
            // } // End of CO_Move

            #endregion Private Methods

            #region Debug

            private void Log(string msg)
            {
                if(!DebugMode) return;
            
                Logger.Log<JoyStick>( msg);
            }
            
            private void LogWarning(string msg)
            {
                if(!DebugMode) return;
                        
                Logger.LogWarning<JoyStick>(msg);
            }

            #endregion Debug
        }
    }
}

