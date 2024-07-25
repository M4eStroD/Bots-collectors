using UnityEngine;
using Zenject;

public class BaseBuilder : MonoBehaviour
{
	private Transform _buildPosition;

	private IBaseFactory _baseFactory;

	[Inject]
	private void Construct(IBaseFactory baseFactory)
	{
		_baseFactory = baseFactory;
	}

	public Base Build(Vector3 position, int countUnits = 1)
	{
		Base build = _baseFactory.Create(position, Quaternion.identity);
		
		build.Initialize(countUnits);
		
		return build;
	}
}