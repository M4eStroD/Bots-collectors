using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(Storage))]
[RequireComponent(typeof(BaseScanner))]
public class Base : MonoBehaviour
{
	[SerializeField] private UnitSpawner _unitSpawner;
	
	[Inject] private IDataProvider _dataProvider;

	private List<Resource> _busyResources = new List<Resource>();
	private Queue<Unit> _freeUnits = new Queue<Unit>();

	private int _countUnits = 0;

	private Storage _storage;
	private FlagBase _newBase;
	private BaseScanner _baseScanner;

	public List<Resource> BusyResource => _busyResources;
	
	public event Action<int> ResourceAdded;
	public event Action<int> ResourceTaked;

	private void Awake()
	{
		_storage = GetComponent<Storage>();
		_baseScanner = GetComponent<BaseScanner>();
		_unitSpawner.UnitAdded += AddUnit;
	}

	private void Update()
	{
		BuyUnit();
		ExpandBase();
		CollectResource();
	}

	public void Initialize(int countUnit)
	{
		_unitSpawner.SpawnUnits(countUnit);
		_storage.Initialize(this);
		_baseScanner.Initialize(this);
	}

	public void AddBase(FlagBase newBase)
	{
		_newBase = newBase;
	}

	public void AddUnit(int countUnit = 1)
	{
		_countUnits += countUnit;
		_unitSpawner.SpawnUnits(countUnit);
	}
	
	private void BuyUnit()
	{
		if (_newBase != null && _countUnits > 1)
			return;
		
		var unitCost = _dataProvider.GetUnit(GameItemsConstant.IDUnit).Cost;

		if (_storage.Resource < unitCost)
			return;

		DecreaseResource(unitCost);
		AddUnit();
	}

	private void ExpandBase()
	{
		if (_newBase == null || _countUnits <= 1)
			return;

		var buildCost = _dataProvider.GetBase(GameItemsConstant.IDBase).Cost;
		
		if (_storage.Resource >= buildCost && _freeUnits.Count > 0)
		{
			Unit unit = _freeUnits.Dequeue();
			unit.SetTarget(_newBase, buildCost);
			_newBase = null;
		}
	}
	
	private void CollectResource()
	{
		if (_baseScanner.CountResources > 0 && _freeUnits.Count > 0)
		{
			Resource resource = _baseScanner.GetResource();
			Unit unit = _freeUnits.Dequeue();

			_busyResources.Add(resource);
			resource.Taked += ReleaseResource;
			
			unit.SetTarget(resource);
		}
	}

	private void AddUnit(Unit unit)
	{
		_countUnits++; 
		
		ReturnUnit(unit, 0);
		unit.ResourceConveyed += ReturnUnit;
		unit.ResourceConveyed += IncreaseResource;
		unit.ResourceTaked += DecreaseResource;
		unit.Transfered += TransferUnit;
	}

	private void TransferUnit(Unit unit)
	{
		unit.ResourceConveyed -= ReturnUnit;
		unit.ResourceConveyed -= IncreaseResource;
		unit.ResourceTaked -= DecreaseResource;
		unit.Transfered -= TransferUnit;

		_countUnits--;
	}

	private void IncreaseResource(Unit unit, int count)
	{
		ResourceAdded?.Invoke(count);
	}

	private void DecreaseResource(int count)
	{
		ResourceTaked?.Invoke(count);
	}

	private void ReturnUnit(Unit unit, int count)
	{
		_freeUnits.Enqueue(unit);
	}

	private void ReleaseResource(Resource resource)
	{
		if (_busyResources.Contains(resource))
			_busyResources.Remove(resource);

		resource.Taked -= ReleaseResource;
	}
}