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
        private Material _material;

        private RenderTexture _maskTexture;
        
        // Shader ID
        private int ShaderID_MaskRenderTexture = Shader.PropertyToID("_MaskRenderTexture");
        
        #endregion Variables

        #region Unity Methods

        private void Start()
        {
            Initialize();
        } // End of Unity - Start

        #endregion Unity Methods

        #region Public Methods

        public RenderTexture GetMaskTexture() => _maskTexture;
        
        public void SetMask()
        {
            
        } // End of SetMask
        
        #endregion Public Methods

        #region Private Methods

        private void Initialize()
        {
            GetComponents();
            PaintManager.Instance.InitTexture(this);
            
            Log("Initialized");
        } // End of Initialize

        private void GetComponents()
        {
            gameObject.layer = gameObject.layer != _paintableLayerMask ? gameObject.layer : _paintableLayerMask;

            _renderer = GetComponent<Renderer>();
            _material = _renderer.material;
            
            _maskTexture = new RenderTexture(TextureSize, TextureSize, 0)
            {
                filterMode = FilterMode.Bilinear
            };
            
            _material.SetTexture(ShaderID_MaskRenderTexture, _maskTexture);
        } // End of GetComponents

        #endregion Private Methods

        #region Debug

        private void Log(string msg)
        {
            if (!DebugMode) return;

            Logger.Log<Paintable>(msg);
        }

        private void LogWarning(string msg)
        {
            if (!DebugMode) return;

            Logger.LogWarning<Paintable>(msg);
        }

        #endregion Debug
    }
}

