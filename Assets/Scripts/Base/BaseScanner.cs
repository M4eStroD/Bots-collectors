using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class BaseScanner : MonoBehaviour
{
	private ResourceGenerator _resourceGenerator;
	private Base _base;

	private int _raduisScanning = 50;

	private List<Resource> _resourcesInArea = new List<Resource>();

	public int CountResources => _resourcesInArea.Count;

	[Inject]
	public void Constructor(ResourceGenerator resourceGenerator)
	{
		_resourceGenerator = resourceGenerator;
		_resourceGenerator.ResourceAdded += SearchResourceInArea;
	}

	public void Initialize(Base build)
	{
		_base = build;
	}
	
	public void Start()
	{
		SearchResourceInArea(null);
	}

	public Resource GetResource()
	{
		Resource tempResource;

		tempResource = _resourcesInArea.First();
		RemoveResource(tempResource);

		return tempResource;
	}
	
	private void RemoveResource(Resource resource)
	{
		if (_resourcesInArea.Contains(resource) == false)
			return;

		_resourcesInArea.Remove(resource);
		resource.Taked -= RemoveResource;
	}

	private void SearchResourceInArea(Resource resource)
	{
		List<Collider> colliders = new List<Collider>();
		List<Resource> resources = new List<Resource>();
		
		colliders = Physics.OverlapSphere(transform.position, _raduisScanning).ToList();

		foreach (Collider collider in colliders)
			if (collider.TryGetComponent(out Resource tempResource))
				resources.Add(tempResource);

		foreach (Resource tempResource in resources)
		{
			if (Vector3.Magnitude(tempResource.gameObject.transform.position - transform.position) > _raduisScanning)
				continue;

			if (_base.BusyResource.Contains(tempResource))
				continue;

			if (_resourcesInArea.Contains(tempResource))
				continue;

			_resourcesInArea.Add(tempResource);
			tempResource.Taked += RemoveResource;

			_resourcesInArea = _resourcesInArea.OrderBy(
				tempResource => Vector3.SqrMagnitude(transform.position - tempResource.transform.position)).ToList();
		}
	}
}