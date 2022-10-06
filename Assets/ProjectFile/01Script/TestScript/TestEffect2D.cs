using GlobalType;
using UnityEngine;

namespace TestScript
{
    public class TestEffect2D : MonoBehaviour
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
    
        #endregion Public Methods
        
        #region Private Methods
    
        //private void Initialize(){GetComponents();} // End of Initialize
        
        //private void GetComponents(){} // End of GetComponents
    
    
        private bool CheckDebugMode => DebugMode == DebugModeType.Global && !GameSetting.Instance.DebugMode;
        private void Log(string msg)
        {
            if(CheckDebugMode) return;
        
            Debug.Log("[]: " + msg);
        }
        
        private void LogWarning(string msg)
        {
            if(CheckDebugMode) return;
                    
            Debug.Log("[]: " + msg);
        }
        
        #endregion Private Methods
    }
}

