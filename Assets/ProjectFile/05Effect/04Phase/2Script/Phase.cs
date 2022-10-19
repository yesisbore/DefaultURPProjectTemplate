using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Phase : MonoBehaviour
{
    [SerializeField] private float _endValue = 1.0f;
    [SerializeField] private float _fadeTime = 5.0f;
    [SerializeField] private Renderer _renderer;
    private static readonly int SplitValue = Shader.PropertyToID("_Split_Value");

    private void Start()
    {
        _renderer = GetComponent<Renderer>();
        DoFade(_fadeTime);
    }

    private void DoFade(float time = 1.0f)
    {
        _renderer.material.SetFloat(SplitValue, 0.0f);
        _renderer.material.DOFloat( _endValue, SplitValue, time).OnComplete(Restart);
    }

    private void Restart()
    {
        DoFade(_fadeTime);
    }
}
