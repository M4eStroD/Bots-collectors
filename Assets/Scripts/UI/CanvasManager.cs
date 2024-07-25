using UnityEngine;
using UnityEngine.UI;
using Zenject;

public class CanvasManager : MonoBehaviour
{
	[SerializeField] private Button _buttonResources;
	[SerializeField] private Button _buttonUnits;

	[SerializeField] private ResourceGenerator _resourceGenerator;

	private readonly int _countSpawn = 1;

	private Storage _storage;
	private IDataProvider _dataProvider;

	private PlacerFlag _placerFlag;
	private PlacerUnit _placerUnit;
	
	private bool _isCursoreBusy = false;
	
	public bool IsCursoreBusy => _isCursoreBusy;

	[Inject]
	public void Construct(IDataProvider dataProvider, Storage storage, PlacerFlag placerFlag, PlacerUnit placerUnit)
	{
		_dataProvider = dataProvider;
		_storage = storage;

		_placerFlag = placerFlag;
		_placerUnit = placerUnit;
	}

	private void Start()
	{
		Subscribe();
		
		_buttonUnits.onClick.AddListener(() =>
		{
			if (_storage.Resource >= _dataProvider.GetUnit(GameItemsConstant.IDUnit).Cost)
				_placerUnit.AddNewUnit();
		});
		
		_buttonResources.onClick.AddListener(() => _resourceGenerator.SpawnResource(_countSpawn));
	}
	
	private void Subscribe()
	{
		_placerUnit.ObjectPlaced += BuyUnit;
		
		_placerFlag.ObjectTaked += (() => _isCursoreBusy = true);
		_placerUnit.ObjectTaked += (() => _isCursoreBusy = true);

		_placerFlag.ObjectRemoved += (() => _isCursoreBusy = false);
		_placerUnit.ObjectRemoved += (() => _isCursoreBusy = false);
	}

	private void BuyUnit(Vector3 position, Base baseUnit)
	{
		if (_storage.TryGetResource(_dataProvider.GetUnit(GameItemsConstant.IDUnit).Cost) == false)
			return;
			
		baseUnit.AddUnit();
	}
}