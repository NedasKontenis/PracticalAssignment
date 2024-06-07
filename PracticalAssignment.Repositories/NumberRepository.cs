namespace PracticalAssignment.Repositories;

public class NumberRepository : INumberRepository
{
    private const string _fileName = "result.txt";

    public async Task Save(IEnumerable<int> numbers)
    {
        await File.WriteAllTextAsync(_fileName, string.Join(' ', numbers));
    }

    public async Task<string> GetLatest()
    {
        var content = await File.ReadAllTextAsync(_fileName);
        return content;
    }
}