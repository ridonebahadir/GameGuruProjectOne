using System;
using UnityEngine;
using Zenject;

public class GridCell : MonoBehaviour
{
    [SerializeField] private Renderer rend;
    public bool IsMarked { get; private set; }
    public bool IsTempVisited { get; set; }
    public Vector2Int GridPosition { get; set; }
    
    private GridManager _gridManager;


    [Inject]
    private void Construct(GridManager gridManager)
    {
        _gridManager = gridManager;
    }
    
    
    
    private void Start()
    {
        ResetCell();
    }
    

    private void OnMouseDown()
    {
        if (IsMarked) return;
        Mark();
        _gridManager.OnCellClicked(this);
    }

    private void Mark()
    {
        IsMarked = true;
        rend.material.color = Color.green;
    }

    public void ResetCell()
    {
        IsMarked = false;
        rend.material.color = Color.white;
    }
}