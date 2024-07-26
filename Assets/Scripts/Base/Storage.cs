using System;
using UnityEngine;

public class Storage : MonoBehaviour
{
	private int _resources = 0;

	public int Resource => _resources;
	
	public event Action<int> ResourceChanged;

	public void Initialize(Base build)
	{
		build.ResourceAdded += AddResource;
		build.ResourceTaked += GetResource;
	}

	private void AddResource(int count)
	{
		_resources += count;
		ResourceChanged?.Invoke(_resources);
	}

	public bool TryGetResource(int amount)
	{
		if (amount > _resources)
			return false;

		_resources -= amount;
		ResourceChanged?.Invoke(_resources);
		
		return true;
	}

	private void GetResource(int amount)
	{
		TryGetResource(amount);
	}
}