using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BaseBuilder))]
[RequireComponent(typeof(PlacerFlag))]
public class ExtansionBase : MonoBehaviour
{
	[SerializeField] private FlagBase _flagBasePrefab;

	private Dictionary<Base, FlagBase> _basesFlags = new Dictionary<Base, FlagBase>();

	private PlacerFlag _placerFlag;
	private BaseBuilder _baseBuilder;

	private FlagBase _currentFlag;

	public event Action<FlagBase> FlagPlaced;
	public event Action<Base> BaseBuilded;

	private void Awake()
	{
		_baseBuilder = GetComponent<BaseBuilder>();
		_placerFlag = GetComponent<PlacerFlag>();
	}

	private void OnEnable()
	{
		_placerFlag.ObjectPlaced += Place;
	}

	private void OnDisable()
	{
		_placerFlag.ObjectPlaced -= Place;
	}

	public Base BuildFirstBase(Vector3 position, int countUnits)
	{
		return _baseBuilder.Build(position, countUnits);
	}

	private void Place(Vector3 position, Base extenstionBase)
	{
		if (_basesFlags.ContainsKey(extenstionBase))
		{
			_basesFlags[extenstionBase].Replace(position);
			return;
		}

		_currentFlag = Instantiate(_flagBasePrefab);
		_currentFlag.gameObject.transform.position = position;

		_currentFlag.Builded += BuildBase;
		_currentFlag.Initialize(extenstionBase);
		
		_basesFlags.Add(extenstionBase, _currentFlag);

		FlagPlaced?.Invoke(_currentFlag);
	}

	private void BuildBase(Base extensionBase)
	{
		Vector3 position = _basesFlags[extensionBase].transform.position;
		
		Destroy(_basesFlags[extensionBase].gameObject);
		_basesFlags.Remove(extensionBase);

		Base tempBase = _baseBuilder.Build(position);
		BaseBuilded?.Invoke(tempBase);
	}
}