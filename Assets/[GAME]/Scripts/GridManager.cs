using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using Zenject;

public class GridManager : MonoBehaviour
{
    public Action<int> OnScoreAdd;
    public Action OnChangeGridSize;
    
    [SerializeField] private int grid = 10;
    [SerializeField] private float spacing = 1.1f;

    private GridCell[,] _grid;
    private CellFactory _cellFactory;
    private DiContainer _diContainer;
    private int _score;

    [Inject]
    private void Construct(CellFactory cellFactory, DiContainer diContainer)
    {
        _cellFactory = cellFactory;
        _diContainer = diContainer;
    }

    private void Start()
    {
        CreateGrid();
        OnScoreAdd?.Invoke(_score);
    }

    private void CreateGrid()
    {
        _grid = new GridCell[grid, grid];

        for (int x = 0; x < grid; x++)
        {
            for (var y = 0; y < grid; y++)
            {
                var pos = new Vector3(x * spacing, 0, y * spacing);
                var cell = _cellFactory.CreateCell(pos, transform);
                _diContainer.Inject(cell);
                cell.GridPosition = new Vector2Int(x, y);
                _grid[x, y] = cell;
            }
        }
    }

    public void OnCellClicked(GridCell startCell)
    {
        List<GridCell> group = GetConnectedMarkedGroup(startCell);

        if (group.Count >= 3)
        {
            foreach (var cell in group)
                cell.ResetCell();
            _score++;
            OnScoreAdd?.Invoke(_score);
        }
    }

    private List<GridCell> GetConnectedMarkedGroup(GridCell start)
    {
        List<GridCell> result = new();
        CheckConnectedRecursive(start, result);
        foreach (var cell in result)
            cell.IsTempVisited = false;

        return result;
    }

    private void CheckConnectedRecursive(GridCell current, List<GridCell> group)
    {
        if (current.IsTempVisited || !current.IsMarked)
            return;

        current.IsTempVisited = true;
        group.Add(current);

        foreach (var neighbor in GetNeighbors(current))
        {
            CheckConnectedRecursive(neighbor, group);
        }
    }


    private List<GridCell> GetNeighbors(GridCell cell)
    {
        List<GridCell> neighbors = new();
        Vector2Int[] directions = new Vector2Int[]
        {
            new(-1, 0), new(1, 0), new(0, 1), new(0, -1)
        };

        foreach (var dir in directions)
        {
            int nx = cell.GridPosition.x + dir.x;
            int ny = cell.GridPosition.y + dir.y;

            if (nx >= 0 && nx < grid && ny >= 0 && ny < grid)
            {
                neighbors.Add(_grid[nx, ny]);
            }
        }

        return neighbors;
    }

    public void ResetAllGrid()
    {
        foreach (var item in _grid)
        {
            Destroy(item.gameObject);
        }
        CreateGrid();
    }

    public int GetGrid() => grid;
    public float GetSpacing() => spacing;

    public int GridSize
    {
        get => grid;
        set
        {
            grid = value;
            OnChangeGridSize?.Invoke();
        }
    }
}