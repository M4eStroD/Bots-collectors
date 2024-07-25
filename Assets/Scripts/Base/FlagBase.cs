using System;
using UnityEngine;

public class FlagBase : MonoBehaviour
{
	[SerializeField] private ParticleSystem _dropEffect;

	public Base Base { get; private set; }

	public event Action Replaced;
	public event Action<Base> Builded;

	public void Initialize(Base extenstionBase)
	{
		Base = extenstionBase;
	}

	public void Replace(Vector3 position)
	{
		transform.position = position;
		_dropEffect.Play();
		Replaced?.Invoke();
	}

	public void Build()
	{
		Builded?.Invoke(Base);
	}
}
