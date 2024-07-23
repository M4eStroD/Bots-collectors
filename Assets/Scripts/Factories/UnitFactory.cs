using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UnitFactory : IUnitFactory
{
    private const string IDUnit = "unit";

    private readonly IDataProvider _dataProvider;
    private readonly DiContainer _diContainer;

    private readonly Transform _container;

    private readonly ObjectPool<Unit> _objectPoolUnits;
    private readonly HashSet<Unit> _units;

    public UnitFactory(IDataProvider dataProvider, DiContainer diContainer, Transform container)
    {
        _objectPoolUnits = new ObjectPool<Unit>();
        _units = new HashSet<Unit>();

        _container = container;

        _dataProvider = dataProvider;
        _diContainer = diContainer;
    }

    public Unit Create(Vector3 position, Quaternion rotation)
    {
        Unit tempUnit = _objectPoolUnits.GetObject();

        if (tempUnit == null)
        {
            tempUnit = _diContainer.InstantiatePrefabForComponent<Unit>(_dataProvider.GetUnit(IDUnit).prefab);
            _units.Add(tempUnit);
        }

        tempUnit.transform.position = position;
        tempUnit.transform.rotation = rotation;
        tempUnit.transform.SetParent(_container);

        return tempUnit;
    }

    public void Clear()
    {
        foreach (Unit unit in _units)
            Object.Destroy(unit.gameObject);

        _objectPoolUnits.Clear();
        _units.Clear();
    }
}
