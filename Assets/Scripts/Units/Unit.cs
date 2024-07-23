using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(UnitMover))]
public class Unit : MonoBehaviour
{
    [SerializeField] private Wood _prefabWood; 

    private Vector3 _placeCamp;
    private Vector3 _firewoodCollector;
    private Vector3 _campFire;

    private Resource _resource;

    private UnitMover _unitMover;
    private Animator _animator;

    private StateMachine _stateMachine;

    public event Action<Unit> ResourceConveyed;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _unitMover = GetComponent<UnitMover>();

        Dictionary<Type, IExitableState> statesMap = new Dictionary<Type, IExitableState>();

        _stateMachine = new StateMachine(statesMap);

        RunStateUnit runState = new RunStateUnit(_animator, _stateMachine, _unitMover);
        CarryStateUnit carryState = new CarryStateUnit(_animator, _stateMachine, _unitMover);

        statesMap.Add(typeof(IdleStateUnit), new IdleStateUnit(_animator));
        statesMap.Add(typeof(CarryStateUnit), carryState);
        statesMap.Add(typeof(RunStateUnit), runState);

        runState.ReachedDestination += TakeResource;
        carryState.ReachedDestination += DropResource;

        StateByDefault();
    }

    private void FixedUpdate()
    {
        _stateMachine?.FixedUpdate();
    }

    public void Initialize(Vector3 placeCamp, Vector3 firewoodCollector, Vector3 campFire)
    {
        _campFire = campFire;
        _placeCamp = placeCamp;
        _firewoodCollector = firewoodCollector;
    }

    public void SetTarget(Resource resource)
    {
        if (resource == null)
            return;

        _resource = resource;

        _stateMachine.Enter<RunStateUnit, Vector3>(_resource.transform.position);
    }

    public Animator GetAnimator()
    {
        return _animator;
    }

    private void TakeResource()
    {
        if (_resource == null)
        {
            StateByDefault();
            return;
        }

        _resource.Take();
        _prefabWood.gameObject.SetActive(true);
        _resource = null;

        _stateMachine.Enter<CarryStateUnit, Vector3>(_firewoodCollector);
    }

    private void DropResource()
    {
        _prefabWood.gameObject.SetActive(false);
        _stateMachine.Enter<RunStateUnit, Vector3>(_placeCamp);

        ResourceConveyed?.Invoke(this);
    }

    private void StateByDefault()
    {
        _stateMachine.Enter<IdleStateUnit>();
        _unitMover.LookAtCampFire(_campFire);
    }
}
