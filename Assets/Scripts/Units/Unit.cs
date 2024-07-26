using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(UnitMover))]
public class Unit : MonoBehaviour
{
	[SerializeField] private EmptyWood _prefabWood;

	private Vector3 _placeCamp;
	private Vector3 _firewoodCollector;
	private Vector3 _campFire;

	private int _costNewBase;
	private int _countTakeResource;
	
	private FlagBase _newBase;
	private Resource _resource;

	private UnitMover _unitMover;
	private Animator _animator;

	private StateMachine _stateMachine;

	public event Action<Unit, int> ResourceConveyed;
	public event Action<int> ResourceTaked;
	public event Action<Unit> Transfered;

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

		runState.ReachedDestination += ChangeState;
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
		_resource.Taked += LoseResource;

		_stateMachine.Enter<RunStateUnit, Vector3>(_resource.transform.position);
	}

	public void SetTarget(FlagBase newBase, int costNewBase)
	{
		_costNewBase = costNewBase;
		_newBase = newBase;
		_newBase.Replaced += BaseReplace;

		_stateMachine.Enter<RunStateUnit, Vector3>(_firewoodCollector);
	}

	private void BaseReplace()
	{
		_stateMachine.Enter<CarryStateUnit, Vector3>(_newBase.gameObject.transform.position);
	}

	private void ChangeState()
	{
		if (_resource == null && _newBase == null)
		{
			StateByDefault();
			return;
		}

		if (_newBase != null)
			TakeResourceBase();
		else
			TakeResourceFloor();
	}

	private void TakeResourceBase()
	{
		ResourceTaked?.Invoke(_costNewBase);
		_countTakeResource = _costNewBase;		
		_prefabWood.gameObject.SetActive(true);

		_stateMachine.Enter<CarryStateUnit, Vector3>(_newBase.gameObject.transform.position);
	}

	private void TakeResourceFloor()
	{
		_resource.Taked -= LoseResource;
		_resource.Take();
		_countTakeResource = _resource.Cost;
		_resource = null;
		
		_prefabWood.gameObject.SetActive(true);

		_stateMachine.Enter<CarryStateUnit, Vector3>(_firewoodCollector);
	}

	private void LoseResource(Resource resource)
	{
		_resource.Taked -= LoseResource;
		_resource = null;

		_stateMachine.Enter<RunStateUnit, Vector3>(_placeCamp);
		ResourceConveyed?.Invoke(this, 0);
	}

	private void DropResource()
	{
		_prefabWood.gameObject.SetActive(false);
		
		if (_newBase != null)
		{
			BuildBase();
			return;
		}

		_stateMachine.Enter<RunStateUnit, Vector3>(_placeCamp);

		ResourceConveyed?.Invoke(this, _countTakeResource);
		_countTakeResource = 0;
		_resource = null;
	}

	private void BuildBase()
	{
		_newBase.Build();
		StateByDefault();
		Transfered?.Invoke(this);
	}

	private void StateByDefault()
	{
		_stateMachine.Enter<IdleStateUnit>();
		_unitMover.LookAtCampFire(_campFire);
	}
}