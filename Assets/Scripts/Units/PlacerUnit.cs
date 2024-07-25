using UnityEngine;

public class PlacerUnit : ObjectPlacer
{
	protected override void Update()
	{
		base.Update();

		if (IsObjectHand)
			if (Input.GetMouseButtonDown(0))
				GetClickInfo();
	}

	public void AddNewUnit()
	{
		TakeObject();
	}

	protected override void GetClickInfo()
	{
		if (CanvasManager.IsCursoreBusy && IsObjectHand == false)
			return;
		
		Ray ray = Camera.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out RaycastHit hit))
			if (hit.collider.TryGetComponent(out TargetBase))
				DropObject(hit);

		RemovePersecutingObject();
	}
	
	protected override void DropObject(RaycastHit hit)
	{
		PlaceObject(hit.point, TargetBase);
	}
	
}