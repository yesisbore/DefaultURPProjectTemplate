using System.Collections;
using System.Collections.Generic;
using UnityCore.PlayerControl;
using UnityEngine;
using UnityEngine.Rendering;

public class MaskTest : MonoBehaviour
{
    public int TextureSize = 512;
    
    private Renderer _renderer;
    private Material _material;

    [SerializeField] private Shader _maskShader;
    private Material _maskMaterial;
    
    
    [SerializeField] private Material _initMaterial;

    public RenderTexture _preTexture;
    public RenderTexture _renderTexture02;
    private CommandBuffer _commandBuffer;
    
    // Shader ID - Mask
    private int ShaderID_Radius = Shader.PropertyToID("_Radius");
    private int ShaderID_PreTexture = Shader.PropertyToID("_PreTexture");
    private int ShaderID_HitPos = Shader.PropertyToID("_HitPos");
    
    // Start is called before the first frame update
    void Start()
    {
        Initialize();
        ControllerInputs.Instance.OnScreenPressed.AddListener(SetMask);
    }

    private void Initialize()
    {
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material;

        _maskMaterial = new Material(_maskShader);
        _preTexture = new RenderTexture(TextureSize, TextureSize, 0)
        {
            filterMode = FilterMode.Bilinear
        };
        _renderTexture02 = new RenderTexture(TextureSize, TextureSize, 0)
        {
            filterMode = FilterMode.Bilinear
        };
        _commandBuffer = new CommandBuffer();
        _commandBuffer.name = "CommandBuffer - " + gameObject.name;
    }

    public void SetMask()
    {
        var ray = Camera.main.ScreenPointToRay(ControllerInputs.Instance.InputPosition);

        Physics.Raycast(ray, out var hitInfo);
        
        _maskMaterial.SetTexture(ShaderID_PreTexture,_preTexture);
        _maskMaterial.SetVector(ShaderID_HitPos,hitInfo.point);
        CopyMaskToRenderTexture();
    }

    private void CopyMaskToRenderTexture()
    {
        _commandBuffer.SetRenderTarget(_preTexture);
        _commandBuffer.DrawRenderer(_renderer,_maskMaterial,0);
        
        _commandBuffer.Blit(_renderTexture02,_preTexture,_maskMaterial);
        
        Graphics.ExecuteCommandBuffer(_commandBuffer);
        _commandBuffer.Clear();
    }
}