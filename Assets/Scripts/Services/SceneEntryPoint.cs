using UnityEngine;
using Zenject;

public class SceneEntryPoint : MonoInstaller
{
    [SerializeField] private Transform _containerUnit; 

    public override void InstallBindings()
    {
        IDataProvider provider = new DataProvider();
        provider.Load();

        Container.BindInstance(provider).AsSingle();

        Container.Bind<IUnitFactory>().To<UnitFactory>().AsSingle().WithArguments(_containerUnit);
        Container.Bind<IResourceFactory>().To<ResourceFactory>().AsSingle();
    }
}
