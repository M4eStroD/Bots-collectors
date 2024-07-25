using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[RequireComponent(typeof(ExtansionBase))]
public class BaseController : MonoBehaviour
{
	[SerializeField] private Transform _positionFirstBase;
	[SerializeField] private List<Base> _bases = new List<Base>();

	[Inject] private ResourceGenerator _resourceGenerator;
	
	private readonly Queue<Resource> _freeResource = new Queue<Resource>();
	private readonly int _countStartUnit = 5;
	
	private ExtansionBase _extansionBase;
	private FlagBase _flagBase;

	public int CountFreeResource => _freeResource.Count;
	
	public event Action<Base> BaseAdded;
	
 	private void Awake()
	{
		_extansionBase = GetComponent<ExtansionBase>();
	}

	private void OnEnable()
	{
		_extansionBase.FlagPlaced += ChangePriority;
		_extansionBase.BaseBuilded += AddNewBase;
		_resourceGenerator.ResourceAdded += AddResource;
	}

	private void OnDisable()
	{
		_extansionBase.FlagPlaced -= ChangePriority;
		_extansionBase.BaseBuilded -= AddNewBase;
		_resourceGenerator.ResourceAdded -= AddResource;
	}

	public void Initialize()
	{
		BuildFirstBase();
	}

	public Resource GetResource()
	{
		return _freeResource.Dequeue();
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
	
	private void AddResource(Resource resource)
	{
		_freeResource.Enqueue(resource);
	}
}