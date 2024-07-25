using UnityEngine;
using Zenject;

public class GameplayEntryPoint : MonoBehaviour
{
	private BaseController _baseController;
	private ResourceGenerator _resourceGenerator;

	[Inject]
	public void Construct(BaseController baseController, ResourceGenerator resourceGenerator)
	{
		_baseController = baseController;
		_resourceGenerator = resourceGenerator;
	}
	
	private void Start()
	{
		_baseController.Initialize();
		_resourceGenerator.Initialize();
	}
}
