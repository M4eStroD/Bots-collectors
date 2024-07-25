using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    private readonly int _cost = 3;

    public int Cost => _cost;
    
    public event Action Taked;

    public void Take()
    {
        Taked?.Invoke();
    }
}
