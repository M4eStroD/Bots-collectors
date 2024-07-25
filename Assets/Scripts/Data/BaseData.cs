using UnityEngine;

namespace Data
{
	[CreateAssetMenu(fileName = "New Base Data", menuName = "StaticData/Base", order = 51)]
	public class BaseData : ScriptableObject
	{
		[SerializeField] private string _id;
		[SerializeField] private int _cost;
		[SerializeField] private Base _prefab;

		public string ID => _id;
		public int Cost => _cost;
		public Base Prefab => _prefab;
	}
}