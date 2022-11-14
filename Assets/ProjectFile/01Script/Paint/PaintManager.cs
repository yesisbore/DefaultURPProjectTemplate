using UnityEngine;

namespace Paint
{
    public class PaintManager : Singleton<PaintManager>
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

        public void Paint(Paintable paintTarget, Vector3 paintPos)
        {
            
            Log("Paint");
        } // End of Paint
        
        #endregion Public Methods
    
        #region Private Methods

        //private void Initialize()
        //{
        //  GetComponents();
        //  SubscribeInputEvent();
        // } // End of Initialize
    
        //private void GetComponents(){} // End of GetComponents

        #endregion Private Methods

        #region Debug

        private void Log(string msg)
        {
            if(!DebugMode) return;
        
            Logger.Log<PaintManager>( msg);
        }
        
        private void LogWarning(string msg)
        {
            if(!DebugMode) return;
                    
            Logger.LogWarning<PaintManager>(msg);
        }

        #endregion Debug
    }
    
}