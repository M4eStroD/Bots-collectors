using System;
using System.Collections.Generic;
using UnityEngine;

public class Base : MonoBehaviour
{
    [SerializeField] private ResourceGenerator _woodGenerator;
    [SerializeField] private UnitSpawner _unitSpawner;

    private Queue<Unit> _freeUnits = new Queue<Unit>();
    private Queue<Resource> _freeResource = new Queue<Resource>();

    private int _score = 0;

    public event Action<int> ScoreChanged;

    private void Awake()
    {
        _unitSpawner.UnitAdded += AddUnit;
        _woodGenerator.ResourceAdded += AddResource;
    }

    private void Update()
    {
        if (_freeResource.Count > 0 && _freeUnits.Count > 0)
        {
            Resource resource = _freeResource.Dequeue();
            Unit unit = _freeUnits.Dequeue();

            unit.SetTarget(resource);
        }            
    }

    private void AddUnit(Unit unit)
    {
        ReturnUnit(unit);
        unit.ResourceConveyed += ReturnUnit;
        unit.ResourceConveyed += IncreaseScore;
    }

    private void IncreaseScore(Unit unit)
    {
        _score++;
        ScoreChanged?.Invoke(_score);
    }

    private void ReturnUnit(Unit unit)
    {
        _freeUnits.Enqueue(unit);
    }

    private void AddResource(Resource resource)
    {
        _freeResource.Enqueue(resource);
    }
}
