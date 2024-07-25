using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Data", menuName = "StaticData/Unit", order = 51)]
public class UnitData : ScriptableObject
{
	[SerializeField] private string _id;
	[SerializeField] private int _cost;
	[SerializeField] private Unit _prefab;

	public string ID => _id;
	public int Cost => _cost;
	public Unit Prefab => _prefab;
}