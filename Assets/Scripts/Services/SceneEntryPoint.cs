using UnityEngine;
using Zenject;

public class SceneEntryPoint : MonoInstaller
{
    [SerializeField] private Transform _containerUnit;
    [SerializeField] private Transform _containerBase;
    
    [SerializeField] private ResourceGenerator _resourceGenerator;
    [SerializeField] private BaseConstructor _baseConstructor;
    [SerializeField] private CanvasSettings _canvasSettings;

    public override void InstallBindings()
    {
	    IDataProvider provider = new DataProvider();
	    provider.Load();

	    Container.BindInstance(provider).AsSingle();

	    Container.Bind<IUnitFactory>().To<UnitFactory>().AsSingle().WithArguments(_containerUnit);
	    Container.Bind<IBaseFactory>().To<BaseFactory>().AsSingle().WithArguments(_containerBase);
	    Container.Bind<IResourceFactory>().To<ResourceFactory>().AsSingle();

	    Container.Bind<CanvasSettings>().FromInstance(_canvasSettings).AsSingle().NonLazy();
	    
	    Container.Bind<ResourceGenerator>().FromInstance(_resourceGenerator).AsSingle().NonLazy();
	    Container.Bind<BaseConstructor>().FromInstance(_baseConstructor).AsSingle().NonLazy();
    }
}
