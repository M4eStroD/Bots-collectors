using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    private readonly int _cost = 1;

    public int Cost => _cost;
    
    public event Action<Resource> Taked;

    public void Take()
    {
        Taked?.Invoke(this);
    }
}
