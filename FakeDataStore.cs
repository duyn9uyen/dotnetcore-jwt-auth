using System.Collections.Generic;
using System.Threading.Tasks;

public class FakeDataStore
{
    private static List<string> _values;

    public FakeDataStore()
    {
        _values = new List<string>
        {
            "a",
            "b",
            "c"
        };
    }


    public void AddValue(string value)
    {
        _values.Add(value);
    }

    public IEnumerable<string> GetAllValues()
    {
        return _values;
    }

    public async Task<IEnumerable<string>> GetAllValuesAsync()
    {
        //return _values;

        // don't actually do this. This is a hard coded value, so no need for an await here. Just an example to get this to compile
        return await Task.Run(() => _values);

    }
}