using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class PaintTest : MonoBehaviour
{
    public int TextureSize = 1024;

    public Material _previousMaterial;
    public Material _paintMaterial;
    int paintColorID = Shader.PropertyToID("_PainterColor");

    public Renderer _renderer;
    public RenderTexture _renderTexture01;
    public RenderTexture _renderTexture02;
    private CommandBuffer _commandBuffer;
    
    int textureID = Shader.PropertyToID("_MainTexture");
    
    // Start is called before the first frame update
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _commandBuffer = new CommandBuffer();
        _commandBuffer.name = "CommandBuffer - " + gameObject.name;
        
        _renderTexture01 = new RenderTexture(TextureSize, TextureSize, 0)
        {
            filterMode = FilterMode.Bilinear
        };
        
        _renderTexture02 = new RenderTexture(TextureSize, TextureSize, 0)
        {
            filterMode = FilterMode.Bilinear
        };
        
        _commandBuffer.SetRenderTarget(_renderTexture01);
        _commandBuffer.DrawRenderer(_renderer,_paintMaterial,0);
        
        _renderer.material.SetTexture(textureID,_renderTexture01);
        
        
        Graphics.ExecuteCommandBuffer(_commandBuffer);
        _commandBuffer.Clear();
        
        Debug.Log("실행 완료 01");
        // 페인트 머티리얼 변경 
        //_paintMaterial.SetColor(paintColorID,Color.black);
        
        // 랜더 텍스쳐 변경해보기 
        
        // 랜더 텍스쳐 변경 후 다시 보내기 
        
        // 랜더 텍스쳐 합쳐보기 
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
