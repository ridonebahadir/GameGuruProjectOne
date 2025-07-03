using UnityEngine;
using Zenject;

public class LevelInstaller : MonoInstaller
{
    [SerializeField] private GameObject cellPrefab;

    [SerializeField] private GridManager gridManager;
    
    public override void InstallBindings()
    {
        Container.Bind<CellFactory>().AsSingle();
        Container.BindInstance(cellPrefab).WhenInjectedInto<CellFactory>();
        Container.Bind<GridManager>().FromInstance(gridManager).AsSingle();
    }
}
