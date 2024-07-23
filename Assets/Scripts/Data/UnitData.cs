using UnityEngine;

[CreateAssetMenu(fileName = "New Unit Data", menuName = "StaticData/Unit", order = 51)]
public class UnitData : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private Unit _prefab; 

    public string ID => _id;
    public Unit prefab => _prefab;
}
