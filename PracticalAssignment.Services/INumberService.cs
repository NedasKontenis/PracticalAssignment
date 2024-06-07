namespace PracticalAssignment.Services;

public interface INumberService
{
    IEnumerable<int> Parse(string number);

    Task Create(IEnumerable<int> numbers);

    Task<string> GetLatest();
}