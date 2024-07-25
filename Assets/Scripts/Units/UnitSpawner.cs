using System;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;

public class UnitSpawner : MonoBehaviour
{
    [SerializeField] private Transform _spawnPosition;

    [SerializeField] private Transform _campFire;
    [SerializeField] private Transform _firewoodCollector; 

    private IUnitFactory _unitFactory;

    public event Action<Unit> UnitAdded;

    [Inject]
    private void Construct(IUnitFactory unitFactory)
    {
        _unitFactory = unitFactory;
    }

    public void SpawnUnits(float countSpawn)
    {
        for (int i = 0; i < countSpawn; i++)
        {
            Vector3 position = GetPoint();
            Unit unit = _unitFactory.Create(position, Quaternion.identity);

            unit.Initialize(position, _firewoodCollector.position, _campFire.position);
            UnitAdded?.Invoke(unit);
        }
    }

    private Vector3 GetPoint()
    {
        float distance = 8;
        float radius = 180;

        float randomAngle = Mathf.Lerp(-radius, radius, Random.value) * Mathf.Deg2Rad;

        float x = Mathf.Sin(randomAngle) * distance;
        float z = Mathf.Cos(randomAngle) * distance;

        Vector3 position = new Vector3(x, 0, z);

        return position + _spawnPosition.position;
    }
}
