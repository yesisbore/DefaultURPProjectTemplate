using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;

[RequireComponent(typeof(Camera))]
public class ObjectController : MonoBehaviour
{
    #region Variables

    // Public Variables


    // Private Variables

    [Header("Prefab")] 
    [SerializeField] private AssetReference _indicatorPrefab;
    
    [Header("Settings")]
    [SerializeField] private LayerMask _targetLayer;

    private Transform _selectedTarget;
    private Transform _indicator;
    private Camera _camera;
    private bool _ownIndicator = false;
    
    private float _maxDistance = float.MaxValue;

    private bool SelectObject => Input.GetMouseButtonDown(0);
    #endregion Variables

    #region Unity Methods

    private void Start()
    {
        Initialize();
    } // End of Unity - Start

    private void Update()
    {
        DebugRay();
        ObjectControl();
        ShowControlUI();
    } // End of Unity - Update

    #endregion Unity Methods

    #region Public Methods

    #endregion Public Methods

    #region Private Methods

    private void Initialize()
    {
        GetComponents();
    } // End of Initialize

    private void GetComponents()
    {
        _camera = GetComponent<Camera>();
    } // End of GetComponents

    private void ObjectControl()
    {
        if(_selectedTarget) return;
        if (!_camera)
        {
            LogWarning("There is no Camera");
            return;
        }

        var ray = _camera.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out var hit, _maxDistance, _targetLayer)) return;
        Log("Hit object : " + hit.transform.name);

        if (!SelectObject) return;
        _selectedTarget = hit.transform;
        Log("Select object : " + _selectedTarget.name);
    } // End of ObjectControl

    private void ShowControlUI()
    {
        if(!_selectedTarget) return;
        
        if (!_ownIndicator)
        {
            _ownIndicator = true;

            _indicatorPrefab.InstantiateAsync().Completed += (op) =>
            {
                _indicator = op.Result.transform;
                AdjustIndicatorProportionToSelectedTarget();
            };
        }
        
        if(!_indicator) return;
        
        _indicator.transform.position = _selectedTarget.position;

    } // End of ShowControlUI

    private void AdjustIndicatorProportionToSelectedTarget()
    {
        // Set Position
        _indicator.transform.position = _selectedTarget.position;
        
        // Set Scale
        var bounds = _selectedTarget.GetComponent<MeshRenderer>().bounds;
        Log("Bounds Size : " + bounds.size);
        
        _indicator.gameObject.SetActive(true);
    } // End of AdjustIndicatorProportionToSelectedTarget
    private void Log(string msg)
    {
        if (!GameSetting.Instance.DebugMode) return;

        Debug.Log("[Object Controller]: " + msg);
    }

    private void LogWarning(string msg)
    {
        if (!GameSetting.Instance.DebugMode) return;

        Debug.Log("[Object Controller]: " + msg);
    }

    private void DebugRay()
    {
        if (!GameSetting.Instance.DebugMode) return;
        if (!_camera) return;

        var playerPos = transform.position;
        var mousePos = Input.mousePosition;
        mousePos.z = 10f;

        mousePos = _camera.ScreenToWorldPoint(mousePos);
        Debug.DrawRay(playerPos, mousePos - playerPos, Color.blue);
    }

    #endregion Private Methods
}