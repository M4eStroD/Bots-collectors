using UnityEngine;

[CreateAssetMenu(fileName = "New Resource Data", menuName = "StaticData/Resource", order = 51)]
public class ResourceData : ScriptableObject
{
    [SerializeField] private string _id;
    [SerializeField] private Resource _prefab;

    public string ID => _id;
    public Resource Prefab => _prefab;
}
