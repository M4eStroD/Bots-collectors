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
		float radius = 0.2f;
		float coefficient = 20;

		Collider[] colliders = Physics.OverlapCapsule(hit.point, Vector3.down * coefficient, radius);

		var countBase = 0;

		foreach (Collider collider in colliders)
		{
			if (collider.TryGetComponent(out BaseArea tempBase) == false)
				continue;

			if (tempBase.TryGetComponent(out FlagBase tempFlag))
				if (tempFlag.Base == TargetBase)
					countBase--;

			countBase++;
		}

		var countZone = (from item in colliders where item.TryGetComponent(out BaseSpawnArea _) select item).Count();

		if (countBase <= 0 && countZone > 0)
			PlaceObject(hit.point, TargetBase);

		RemovePersecutingObject();
	}
}