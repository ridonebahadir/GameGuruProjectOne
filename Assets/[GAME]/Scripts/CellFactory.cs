using UnityEngine;
using Zenject;

public class CellFactory
{
    private readonly GameObject _prefab;

    [Inject]
    public CellFactory(GameObject prefab)
    {
        _prefab = prefab;
    }

    public GridCell CreateCell(Vector3 position, Transform parent)
    {
        var obj = Object.Instantiate(_prefab, position, Quaternion.identity, parent);
        return obj.GetComponent<GridCell>();
    }
}