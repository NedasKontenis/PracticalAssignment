namespace PracticalAssignment.Repositories;

public interface INumberRepository
{
    Task Save(IEnumerable<int> numbers);

    Task<string> GetLatest();
}