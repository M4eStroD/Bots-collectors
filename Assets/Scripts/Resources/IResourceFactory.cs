using UnityEngine;

public interface IResourceFactory
{
    void Clear();
    Resource Create(Vector3 position, Quaternion rotation, Transform container = null);
}