using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class Base : MonoBehaviour
{
	[SerializeField] private UnitSpawner _unitSpawner;
	
	[Inject] private BaseController _baseController;
	[Inject] private IDataProvider _dataProvider;
	[Inject] private Storage _storage;
	
	private Queue<Unit> _freeUnits = new Queue<Unit>();

	private FlagBase _newBase;

	public event Action<int> ResourceAdded;
	public event Action<int> ResourceTaked;

	private void Awake()
	{
		_unitSpawner.UnitAdded += AddUnit;
	}

	private void Update()
	{
		ExpandBase();
		CollectResource();
	}

	public void Initialize(int countUnit)
	{
		_unitSpawner.SpawnUnits(countUnit);
	}

	public void AddBase(FlagBase newBase)
	{
		_newBase = newBase;
	}

	public void AddUnit(int countUnit = 1)
	{
		_unitSpawner.SpawnUnits(countUnit);
	}

	private void ExpandBase()
	{
		if (_newBase == null)
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
		if (_baseController.CountFreeResource > 0 && _freeUnits.Count > 0)
		{
			Resource resource = _baseController.GetResource();
			Unit unit = _freeUnits.Dequeue();

			unit.SetTarget(resource);
		}
	}

	private void AddUnit(Unit unit)
	{
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
}