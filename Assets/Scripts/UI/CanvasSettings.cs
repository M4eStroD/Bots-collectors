using UnityEngine;
using UnityEngine.UI;

public class CanvasSettings : MonoBehaviour
{
	[SerializeField] private Button _buttonResources;
	[SerializeField] private ResourceGenerator _resourceGenerator;

	private readonly int _countSpawn = 1;

	private void Start()
	{
		_buttonResources.onClick.AddListener(() => _resourceGenerator.SpawnResource(_countSpawn));
	}
}