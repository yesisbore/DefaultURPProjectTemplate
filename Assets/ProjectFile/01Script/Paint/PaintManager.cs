using UnityEngine;
using UnityEngine.Rendering;

namespace Paint
{
    public class PaintManager : Singleton<PaintManager>
    {
        #region Variables
 
        public bool DebugMode = false;
 
        // Public Variables
    
        // Private Variables
        [SerializeField] private int _textureSize = 512;
        [SerializeField] private Material _initMaterial;
        [SerializeField] private Shader _maskShader;
        [SerializeField] private Shader _tempMaskShader;

        private RenderTexture _tempTexture;
        
        private CommandBuffer _commandBuffer;
        private Material _tempMaskMaterial;
        
        // Shader ID - Mask
        private int ShaderID_Offset = Shader.PropertyToID("_Offset");
        private int ShaderID_Radius = Shader.PropertyToID("_Radius");
        private int ShaderID_PreTexture = Shader.PropertyToID("_PreTexture");
        private int ShaderID_HitPos = Shader.PropertyToID("_HitPos");
        
        #endregion Variables

        #region Unity Methods

        private void Start() { Initialize();} // End of Unity - Start

        //private void Update(){} // End of Unity - Update

        #endregion Unity Methods

        #region Public Methods

        public void Paint(Paintable paintTarget, Vector3 paintPos,float radius)
        {
            SetCommandBuffer();
            
            // Set TempMask
            _tempMaskMaterial.SetVector(ShaderID_HitPos,paintPos);
            
            var maskRenderTexture = paintTarget.GetMaskTexture();
            
            // _paintMaterial.SetTexture(ShaderID_PreTexture,mask);
            // _paintMaterial.SetVector(ShaderID_Offset,paintPos);
            // _paintMaterial.SetFloat(ShaderID_Radius,radius);
            //
            // _commandBuffer.Blit(_tempTexture,mask,_paintMaterial);
            Graphics.ExecuteCommandBuffer(_commandBuffer);
            _commandBuffer.Clear();
            Log("Paint");
        } // End of Paint
        
        #endregion Public Methods

        public void InitTexture(Paintable target)
        {
            SetCommandBuffer();
            var mask = target.GetMaskTexture();
            
            _commandBuffer.Blit(_tempTexture,mask,_initMaterial);
            Graphics.ExecuteCommandBuffer(_commandBuffer);
            _commandBuffer.Clear();
            
            Log("Initialized Paintable Texture");
        } // End of InitTexture 
        
        #region Private Methods

        private void Initialize()
        {
          GetComponents();
         } // End of Initialize
    
        private void GetComponents()
        {
            SetCommandBuffer();
            //_tempMaskMaterial = new Material(_tempMaskMaterial);
            _tempTexture = new RenderTexture(_textureSize, _textureSize, 0)
            {
                filterMode = FilterMode.Bilinear
            };

        } // End of GetComponents

        private void Mask()
        {
            
        }
        private void SetCommandBuffer()
        {
            if(_commandBuffer != null)  return;
            
            _commandBuffer = new CommandBuffer();
            _commandBuffer.name = "CommandBuffer - " + gameObject.name;
            
            Log("Set Command Buffer");
        } // End of SetCommandBuffer
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
