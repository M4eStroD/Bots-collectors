using System.Collections.Generic;
using System.Linq;
using Data;
using UnityEngine;

public class DataProvider : IDataProvider
{
    private Dictionary<string, UnitData> _unitsData;
    private Dictionary<string, ResourceData> _resourcesData;
    private Dictionary<string, BaseData> _baseData;

    public void Load()
    {
        _unitsData = Resources.LoadAll<UnitData>(AssetsPath.UnitsPath).ToDictionary(unit => unit.ID, unit => unit);
        _resourcesData = Resources.LoadAll<ResourceData>(AssetsPath.ResourcesPath).ToDictionary(resource => resource.ID, resource => resource);
        _baseData = Resources.LoadAll<BaseData>(AssetsPath.BasePath).ToDictionary(build => build.ID, build => build);
    }

    public UnitData GetUnit(string id)
    {
        if (_unitsData.ContainsKey(id) == false)
            throw new KeyNotFoundException($"Unit with ID '{id}' not found.");

        return _unitsData[id];
    }

    public ResourceData GetResource(string id)
    {
        if (_resourcesData.ContainsKey(id) == false)
            throw new KeyNotFoundException($"Resource with ID '{id}' not found.");

        return _resourcesData[id];
    }

    public BaseData GetBase(string id)
    {
        if (_baseData.ContainsKey(id) == false)
            throw new KeyNotFoundException($"Base with ID '{id}' not found.");

        return _baseData[id];
    }
}
