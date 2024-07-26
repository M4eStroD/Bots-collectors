using UnityEngine;
using Zenject;

public class GameplayEntryPoint : MonoBehaviour
{
	private BaseConstructor _baseConstructor;
	private ResourceGenerator _resourceGenerator;

	[Inject]
	public void Construct(BaseConstructor baseConstructor, ResourceGenerator resourceGenerator)
	{
		_baseConstructor = baseConstructor;
		_resourceGenerator = resourceGenerator;
	}
	
	private void Start()
	{
		_baseConstructor.Initialize();
		_resourceGenerator.Initialize();
	}
}
