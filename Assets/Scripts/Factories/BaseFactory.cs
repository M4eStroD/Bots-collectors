using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BaseFactory : IBaseFactory
{
	private readonly IDataProvider _dataProvider;
	private readonly DiContainer _diContainer;
	
	private readonly Transform _container;

	private readonly ObjectPool<Base> _objectPoolBase;
	private readonly HashSet<Base> _bases;

	public BaseFactory(IDataProvider dataProvider, DiContainer diContainer, Transform Container)
	{
		_objectPoolBase = new ObjectPool<Base>();
		_bases = new HashSet<Base>();

		_container = Container;
		
		_dataProvider = dataProvider;
		_diContainer = diContainer;
	}

	public Base Create(Vector3 position, Quaternion rotation)
	{
		Base tempBase = _objectPoolBase.GetObject();

		if (tempBase == null)
		{
			var basePrefab = _dataProvider.GetBase(GameItemsConstant.IDBase).Prefab;
			tempBase = _diContainer.InstantiatePrefabForComponent<Base>(basePrefab);
			_bases.Add(tempBase);
		}

		tempBase.transform.position = position;
		tempBase.transform.rotation = rotation;
		tempBase.transform.SetParent(_container);

		return tempBase;
	}

	public void Clear()
	{
		foreach (Base build in _bases) 
			Object.Destroy(build.gameObject);
		
		_objectPoolBase.Clear();
		_bases.Clear();
	}
}