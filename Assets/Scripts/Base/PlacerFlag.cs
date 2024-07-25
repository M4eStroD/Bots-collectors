using System.Linq;
using UnityEngine;

public class PlacerFlag : ObjectPlacer
{
	protected override void Update()
	{
		base.Update();
		
		if (Input.GetMouseButtonDown(0))
			GetClickInfo();
	}
		
	protected override void GetClickInfo()
	{
		if (CanvasManager.IsCursoreBusy && IsObjectHand == false)
			return;
		
		Ray ray = Camera.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out RaycastHit hit) == false)
			return;

		if (IsObjectHand)
			DropObject(hit);
		else if (hit.collider.TryGetComponent(out TargetBase))
			TakeObject();
	}

	protected override void DropObject(RaycastHit hit)
	{
		float radius = 3;

		Collider[] colliders = Physics.OverlapSphere(hit.point, radius);

		BaseArea tempBase = null;
		
		var countBase = (from item in colliders where item.TryGetComponent(out tempBase) select item).Count();

		if (tempBase != null)
			if (tempBase.TryGetComponent(out FlagBase tempFlag))
				if (tempFlag.Base == TargetBase)
					countBase--;
		
		var countZone = (from item in colliders where item.TryGetComponent(out BaseSpawnArea _)select item).Count();

		if (countBase <= 0 && countZone > 0)
			PlaceObject(hit.point, TargetBase);

		RemovePersecutingObject();
	}
}