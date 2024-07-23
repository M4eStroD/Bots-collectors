using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataProvider : IDataProvider
{
    private Dictionary<string, UnitData> _unitsData;
    private Dictionary<string, ResourceData> _resourcesData;

    public void Load()
    {
        _unitsData = Resources.LoadAll<UnitData>(AssetsPath.UNITS_PATH).ToDictionary(unit => unit.ID, unit => unit);
        _resourcesData = Resources.LoadAll<ResourceData>(AssetsPath.RESOURCES_PATH).ToDictionary(resource => resource.ID, resource => resource);
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
}
