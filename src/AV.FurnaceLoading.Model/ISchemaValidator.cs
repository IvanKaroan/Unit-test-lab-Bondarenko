namespace AV.FurnaceLoading.Model;

public interface ISchemaValidator
{
    bool SchemaValidFor(LoadSchema schema, Cassette cassette);
}
