using System;
using UnityEngine;
using Zenject;

public class CameraController : MonoBehaviour
{
    private Camera _mainCamera;
    private GridManager _gridManager;
    
    
    [Inject]
    private void Construct(GridManager gridManager)
    {
        _gridManager = gridManager;
      
    }

    private void OnEnable()
    {
        _gridManager.OnChangeGridSize += SetCamera;
    }

    private void OnDisable()
    {
        _gridManager.OnChangeGridSize -= SetCamera;
    }

    private void Start()
    {
        _mainCamera = GetComponent<Camera>();
        SetCamera();
    }

    private void SetCamera()
    {
       
        var gridWorld = (_gridManager.GetGrid() - 1) * _gridManager.GetSpacing();

        var center = new Vector3(gridWorld / 2f, 0f, gridWorld / 2f);

        
        _mainCamera.transform.position = center + new Vector3(0, 10f, 0);
        
        var half = (_gridManager.GetGrid() * _gridManager.GetSpacing()) / 2f;
        _mainCamera.orthographicSize = Mathf.Max(half, half / _mainCamera.aspect) + 1f;
        
    }
}