using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnifyingObject : MonoBehaviour
{
    private Camera _mainCam;
    private Renderer _renderer;
    private Vector3 _screenPoint;
    private static readonly int ObjScreenPos = Shader.PropertyToID("_ObjScreenPos");

    void Start()
    {
        _mainCam = Camera.main;
        _renderer = GetComponent<Renderer>();
    }

    private void Update()
    {
        _screenPoint = _mainCam.WorldToScreenPoint(transform.position);
        _screenPoint.x /= Screen.width;
        _screenPoint.y /= Screen.height;
        _renderer.material.SetVector(ObjScreenPos,_screenPoint);
    }
}
