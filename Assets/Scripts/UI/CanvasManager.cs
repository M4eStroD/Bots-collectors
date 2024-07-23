using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    [SerializeField] private Button _buttonResources;
    [SerializeField] private Button _buttonUnits;

    [SerializeField] private UnitSpawner _unitSpawner;
    [SerializeField] private ResourceGenerator _resourceGenerator; 

    private readonly int _countSpawn = 1;

    private void Start()
    {
        _buttonUnits.onClick.AddListener(() => _unitSpawner.SpawnUnits(_countSpawn));
        _buttonResources.onClick.AddListener(() => _resourceGenerator.SpawnResource(_countSpawn));
    }
}
