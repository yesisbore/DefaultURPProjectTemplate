using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookCamera : MonoBehaviour
{
    private Camera _mainCam;
    void Start()
    {
        _mainCam = Camera.main;
    }

    private void Update()
    {
        transform.forward = _mainCam.transform.position - transform.position;
    }
}
