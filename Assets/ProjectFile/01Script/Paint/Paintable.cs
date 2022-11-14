using UnityEngine;

namespace Paint
{
    public class Paintable : MonoBehaviour
    {
        #region Variables
 
        public bool DebugMode = false;

        public int TextureSize = 512;
        // Public Variables
    
        // Private Variables
        [SerializeField] private LayerMask _paintableLayerMask;

        private Renderer _renderer;
        private RenderTexture _renderTexture;
        
        #endregion Variables

        #region Unity Methods

        private void Start() { Initialize();} // End of Unity - Start

        #endregion Unity Methods

        #region Public Methods

        #endregion Public Methods
    
        #region Private Methods

        private void Initialize()
        {
          GetComponents();
          
          Log("Initialized");
         } // End of Initialize

        private void GetComponents()
        {
            gameObject.layer = gameObject.layer != _paintableLayerMask ? gameObject.layer :_paintableLayerMask;

            _renderer = GetComponent<Renderer>();
            _renderTexture = new RenderTexture(TextureSize,TextureSize,0)
            {
                filterMode = FilterMode.Bilinear
            };
        } // End of GetComponents
    

        #endregion Private Methods

        #region Debug

        private void Log(string msg)
        {
            if(!DebugMode) return;
        
            Logger.Log<Paintable>( msg);
        }
        
        private void LogWarning(string msg)
        {
            if(!DebugMode) return;
                    
            Logger.LogWarning<Paintable>(msg);
        }

        #endregion Debug
    }
}

