using Data;

public interface IDataProvider
{
    ResourceData GetResource(string id);
    UnitData GetUnit(string id);
    BaseData GetBase(string id);
    void Load();
}