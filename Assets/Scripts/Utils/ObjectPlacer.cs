using System;
using UnityEngine;
using Zenject;

public abstract class ObjectPlacer : MonoBehaviour
{
	[SerializeField] protected ObjectMouseFollower _objectMouseFollowerPrefab;

	protected bool IsObjectHand;
	protected Camera Camera;
	protected Base TargetBase;
	protected CanvasManager CanvasManager;
	
	private ObjectMouseFollower _tempObjectHand;

	public event Action<Vector3, Base> ObjectPlaced;
	public event Action ObjectTaked;
	public event Action ObjectRemoved;

	[Inject]
	public void Construct(CanvasManager canvasManager)
	{
		CanvasManager = canvasManager;
	}
	
	private void Awake()
	{
		Camera = Camera.main;
	}

	protected virtual void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape))
			RemovePersecutingObject();
	}

	protected void PlaceObject(Vector3 position, Base build)
	{
		ObjectPlaced?.Invoke(position, build);
	}

	protected void RemovePersecutingObject()
	{
		if (_tempObjectHand == null)
			return;

		IsObjectHand = false;
		Destroy(_tempObjectHand.gameObject);
		
		ObjectRemoved?.Invoke();
	}

	protected void TakeObject()
	{
		if (_tempObjectHand != null)
			RemovePersecutingObject();

		IsObjectHand = true;
		_tempObjectHand = Instantiate(_objectMouseFollowerPrefab);
		
		ObjectTaked?.Invoke();
	}
	
	protected abstract void DropObject(RaycastHit hit);
	protected abstract void GetClickInfo();
}