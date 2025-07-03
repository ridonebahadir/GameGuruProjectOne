using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GridManager : MonoBehaviour
{
    public int score = 0;
    [SerializeField] private int gridWidth = 10;
    [SerializeField] private int gridHeight = 10;
    [SerializeField] private float spacing = 1.1f;

    private GridCell[,] _grid;
    private CellFactory _cellFactory;
    private DiContainer _diContainer;

    [Inject]
    private void Construct(CellFactory cellFactory,DiContainer diContainer)
    {
        _cellFactory = cellFactory;
        _diContainer = diContainer;
    
    } 
    private void Start()
    {
        CreateGrid();
    }

    private void CreateGrid()
    {
        _grid = new GridCell[gridWidth, gridHeight];

        for (int x = 0; x < gridWidth; x++)
        {
            for (var y = 0; y < gridHeight; y++)
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

            score++;
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

            if (nx >= 0 && nx < gridWidth && ny >= 0 && ny < gridHeight)
            {
                neighbors.Add(_grid[nx, ny]);
            }
        }

        return neighbors;
    }

    public int GetGridWidth() => gridWidth;
    public int GetGridHeight() => gridHeight;
    public float GetSpacing() => spacing;
}
