using GlobalType;
using UnityEngine;

namespace UnityCore
{
    namespace Effect
    {
        public class EffectController : MonoBehaviour
        {
            #region Variables

            // Public Variables
            public static EffectController Instance;

            public bool DebugMode = false;
            // Private Variables

            #endregion Variables

            #region Unity Methods

            private void Awake()
            {
                if (!Instance)
                {
                    Configure();
                }
            } // End of Unity - Awake

            //private void Update(){} // End of Unity - Update

            #endregion Unity Methods

            #region Public Methods

            #endregion Public Methods

            #region Private Methods

            private void Configure()
            {
                Instance = this;
            } // End of Configure

            private void Log(string msg)
            {
                if(!DebugMode) return;
            
                Logger.Log<EffectController>( msg);
            }
            
            private void LogWarning(string msg)
            {
                if(!DebugMode) return;
                        
                Logger.LogWarning<EffectController>(msg);
            }

            #endregion Private Methods
        }
    }
}