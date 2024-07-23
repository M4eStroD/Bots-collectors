using System.Collections.Generic;
using UnityEngine;

public class ResourceFactory : IResourceFactory
{
    private const string IDResource = "wood";

    private readonly IDataProvider _dataProvider;

    private readonly ObjectPool<Resource> _objectPoolResources;
    private readonly HashSet<Resource> _resources;

    public ResourceFactory(IDataProvider dataProvider)
    {
        _objectPoolResources = new ObjectPool<Resource>();
        _resources = new HashSet<Resource>();

        _dataProvider = dataProvider;
    }

    public Resource Create(Vector3 position, Quaternion rotation, Transform container = null)
    {
        Resource tempResource = _objectPoolResources.GetObject();

        if (tempResource == null)
        {
            tempResource = Object.Instantiate(_dataProvider.GetResource(IDResource).prefab);
            _resources.Add(tempResource);

            tempResource.Taked += (() => _objectPoolResources.PutObject(tempResource));
        }
        else
        {
            tempResource.gameObject.SetActive(true);
        }

        tempResource.transform.position = position;
        tempResource.transform.rotation = rotation;
        tempResource.transform.SetParent(container);

        return tempResource;
    }

    public void Clear()
    {
        foreach (var resource in _resources)
            Object.Destroy(resource.gameObject);

        _objectPoolResources.Clear();
        _resources.Clear();
    }
}
