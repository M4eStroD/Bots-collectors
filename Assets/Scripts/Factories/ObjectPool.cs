using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> where T : MonoBehaviour
{
    private Queue<T> _pool = new Queue<T>();

    public T GetObject()
    {
        if (_pool.Count == 0)
            return null;

        return _pool.Dequeue();
    }

    public void PutObject(T tempObject)
    {
        _pool.Enqueue(tempObject);
        tempObject.gameObject.SetActive(false);
    }

    public void Clear()
    {
        _pool.Clear();
    }
}
