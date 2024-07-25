using UnityEngine;

public class ObjectMouseFollower : MonoBehaviour
{
	private readonly float _distanceOffsetY = 2f;
	
	private Vector3 _position = Vector3.zero;
	private Camera _camera;

	private void Start()
	{
		_camera = Camera.main;
	}

	private void Update()
	{
		Follow();
	}

	private void Follow()
	{
		Ray ray = _camera.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out RaycastHit hit))
		{
			_position = hit.point;
			_position.y += _distanceOffsetY;
		}

		transform.position = _position;
	}
}
