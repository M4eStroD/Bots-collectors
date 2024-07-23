using System;
using UnityEngine;

public class Resource : MonoBehaviour
{
    public event Action Taked;

    public void Take()
    {
        Taked?.Invoke();
    }
}
