using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ExtansionBase))]
public class BaseConstructor : MonoBehaviour
{
	[SerializeField] private Transform _positionFirstBase;
	[SerializeField] private List<Base> _bases = new List<Base>();

	private readonly int _countStartUnit = 3;
	
	private ExtansionBase _extansionBase;
	private FlagBase _flagBase;

	public event Action<Base> BaseAdded;
	
 	private void Awake()
	{
		_extansionBase = GetComponent<ExtansionBase>();
	}

	private void OnEnable()
	{
		_extansionBase.FlagPlaced += ChangePriority;
		_extansionBase.BaseBuilded += AddNewBase;
	}

	private void OnDisable()
	{
		_extansionBase.FlagPlaced -= ChangePriority;
		_extansionBase.BaseBuilded -= AddNewBase;
	}

	public void Initialize()
	{
		BuildFirstBase();
	}

	private void BuildFirstBase()
	{
		Base tempBase = _extansionBase.BuildFirstBase(_positionFirstBase.position, _countStartUnit);
		_bases.Add(tempBase);
		BaseAdded?.Invoke(tempBase);
	}
	
	private void ChangePriority(FlagBase flagBase)
	{
		_flagBase = flagBase;
		
		if (_bases.Contains(_flagBase.Base))
		{
			var index = _bases.IndexOf(_flagBase.Base);
			_bases[index].AddBase(_flagBase);
		}
	}

	private void AddNewBase(Base newBase)
	{
		_bases.Add(newBase);
		BaseAdded?.Invoke(newBase);
	}
}
