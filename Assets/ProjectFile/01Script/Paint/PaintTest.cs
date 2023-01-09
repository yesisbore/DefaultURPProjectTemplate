using System.Collections;
using System.Collections.Generic;
using UnityCore.PlayerControl;
using UnityEngine;
using UnityEngine.Rendering;

public class PaintTest : MonoBehaviour
{
    public MaskTest MaskTest;

    private Renderer _renderer;
    private Material _material;
    
    private int ShaderID_MaskRenderTexture = Shader.PropertyToID("_MaskRenderTexture");
    void Start()
    {
        _renderer = GetComponent<Renderer>();
        _material = _renderer.material;
        ControllerInputs.Instance.OnScreenPressed.AddListener(OnMouseClick);
    }

    private void OnMouseClick()
    {
        var random = Random.Range(0.0f, 1.0f);
        var offset = new Vector2(random, random);
        var radius = random;
        
        //_material.SetTexture(ShaderID_MaskRenderTexture,MaskTest.SetMask(offset,radius));

    }
}
