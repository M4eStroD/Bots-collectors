using UnityEngine;
using Zenject;

public class SceneEntryPoint : MonoInstaller
{
    [SerializeField] private Transform _containerUnit;
    [SerializeField] private Transform _containerBase;
    
    [SerializeField] private Storage _storage;
    [SerializeField] private ResourceGenerator _resourceGenerator;
    [SerializeField] private BaseController _baseController;
    [SerializeField] private CanvasManager _canvasManager;

    [SerializeField] private PlacerFlag _placerFlag;
    [SerializeField] private PlacerUnit _placerUnit;

    public override void InstallBindings()
    {
	    IDataProvider provider = new DataProvider();
	    provider.Load();

	    Container.BindInstance(provider).AsSingle();

	    Container.Bind<IUnitFactory>().To<UnitFactory>().AsSingle().WithArguments(_containerUnit);
	    Container.Bind<IBaseFactory>().To<BaseFactory>().AsSingle().WithArguments(_containerBase);
	    Container.Bind<IResourceFactory>().To<ResourceFactory>().AsSingle();

	    Container.Bind<PlacerFlag>().FromInstance(_placerFlag).AsSingle().NonLazy();
	    Container.Bind<PlacerUnit>().FromInstance(_placerUnit).AsSingle().NonLazy();
	    
	    Container.Bind<CanvasManager>().FromInstance(_canvasManager).AsSingle().NonLazy();
	    
	    Container.Bind<Storage>().FromInstance(_storage).AsSingle().NonLazy();
	    Container.Bind<ResourceGenerator>().FromInstance(_resourceGenerator).AsSingle().NonLazy();
	    Container.Bind<BaseController>().FromInstance(_baseController).AsSingle().NonLazy();
    }
}
