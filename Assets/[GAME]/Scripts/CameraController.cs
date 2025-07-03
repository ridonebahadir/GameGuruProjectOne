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

    private void Start()
    {
        _mainCamera = GetComponent<Camera>();
        SetCamera();
    }

    private void SetCamera()
    {
       
        var gridWorldWidth = (_gridManager.GetGridWidth() - 1) * _gridManager.GetSpacing();
        var gridWorldHeight = (_gridManager.GetGridHeight() - 1) * _gridManager.GetSpacing();

        Vector3 center = new Vector3(gridWorldWidth / 2f, 0f, gridWorldHeight / 2f);

        
        _mainCamera.transform.position = center + new Vector3(0, 10f, 0);
        
        var halfWidth = (_gridManager.GetGridWidth() * _gridManager.GetSpacing()) / 2f;
        var halfHeight = (_gridManager.GetGridHeight() * _gridManager.GetSpacing()) / 2f;
        _mainCamera.orthographicSize = Mathf.Max(halfHeight, halfWidth / _mainCamera.aspect) + 1f;
        
    }
}