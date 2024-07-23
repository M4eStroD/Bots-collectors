public interface IDataProvider
{
    ResourceData GetResource(string id);
    UnitData GetUnit(string id);
    void Load();
}