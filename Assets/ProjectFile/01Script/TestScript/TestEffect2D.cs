using GlobalType;
using UnityEngine;

namespace TestScript
{
    public class TestEffect2D : MonoBehaviour
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
    
    
        private void Log(string msg)
        {
            if(!DebugMode) return;
            
            Logger.Log<TestEffect2D>( msg);
        }
            
        private void LogWarning(string msg)
        {
            if(!DebugMode) return;
                        
            Logger.LogWarning<TestEffect2D>(msg);
        }
        
        #endregion Private Methods
    }
}

