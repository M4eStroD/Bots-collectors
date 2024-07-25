using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private Transform _container;
    [SerializeField] private BoxCollider _spawnPlane;

    private readonly int _resourcesOnStart = 25;
    private IResourceFactory _resourceFactory;

    public event Action<Resource> ResourceAdded;

    [Inject]
    private void Construct(IResourceFactory resourceFactory)
    {
        _resourceFactory = resourceFactory;
    }
    
    public void Initialize()
    {
        SpawnResource(_resourcesOnStart);
    }

    public void SpawnResource(float countSpawn)
    {
        for (int i = 0; i < countSpawn; i++)
        {
            Vector3 position = GetRandomPoint();
            Quaternion rotation = GetRandomRotation();

            Resource resource = _resourceFactory.Create(position, rotation, _container);

            ResourceAdded?.Invoke(resource);
        }
    }

    private Quaternion GetRandomRotation()
    {
        float rotateX = -90;
        float rotateY = Random.Range(0, 360);
        float rotateZ = 0;

        Quaternion rotation = Quaternion.Euler(rotateX, rotateY, rotateZ); 

        return rotation;
    }

    private Vector3 GetRandomPoint()
    {
        Vector3 position = new Vector3();
        Collider[] colliders;

        bool isActive;
        float radius = 2;

        do
        {
            isActive = false;

            position.x = Random.Range(_spawnPlane.bounds.min.x, _spawnPlane.bounds.max.x);
            position.y = 0;
            position.z = Random.Range(_spawnPlane.bounds.min.z, _spawnPlane.bounds.max.z);

            colliders = Physics.OverlapSphere(position, radius);

            foreach (Collider collider in colliders)
                if (collider.TryGetComponent(out Base _))
                    isActive = true;
        }
        while (isActive);

        return position;
    }
}
