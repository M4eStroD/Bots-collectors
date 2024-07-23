using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class UnitMover : MonoBehaviour
{
    private const float SpeedPhysicsFactor = 50f;

    private float _tolerance = 1f;

    private Rigidbody _rigidbody;

    private float _speed = 10f;
    private float _totalSpeed;

    public bool IsStanding { get { return _rigidbody.velocity.sqrMagnitude > _tolerance; } }

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    public bool TryMoveTowards(Vector3 target, float distanceOffset)
    {
        if (GetDistance(target, transform.position) <= distanceOffset)
        {
            return false;
        }

        Vector3 direction = (target - transform.position).normalized;

        transform.rotation = Quaternion.LookRotation(direction);

        _totalSpeed = _speed * SpeedPhysicsFactor * Time.deltaTime;
        _rigidbody.velocity = new Vector3(direction.x * _totalSpeed, _rigidbody.velocity.y, direction.z * _totalSpeed);

        return true;
    }

    public void LookAtCampFire(Vector3 campFire)
    {
        Vector3 direction = (campFire - transform.position).normalized;
        transform.rotation = Quaternion.LookRotation(direction);
    }

    private float GetDistance(Vector3 firstPosition, Vector3 secondPosition)
    {
        return Vector3.SqrMagnitude(firstPosition - secondPosition);
    }
}
