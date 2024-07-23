using UnityEngine;

public interface IUnitFactory
{
    void Clear();
    Unit Create(Vector3 position, Quaternion rotation);
}