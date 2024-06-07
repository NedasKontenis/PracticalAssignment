using PracticalAssignment.Repositories;
using PracticalAssignment.Services.Exceptions;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace PracticalAssignment.Services;

public class NumberService : INumberService
{
    private readonly INumberRepository _numberRepository;
    private readonly ILogger<NumberService> _logger;

    public NumberService(INumberRepository numberRepository,
        ILogger<NumberService> logger)
    {
        _numberRepository = numberRepository;
        _logger = logger;
    }

    public IEnumerable<int> Parse(string number)
    {
        if (string.IsNullOrWhiteSpace(number))
        {
            throw new DomainServiceException($"Cannot parse because it is empty.");
        }

        foreach (var input in number.Split(' '))
        {
            if (!int.TryParse(input, out int result))
            {
                throw new DomainServiceException($"Cannot parse string because {input} is not a number.");
            }

            yield return result;
        }
    }

    public async Task Create(IEnumerable<int> numbers)
    {
        Stopwatch stopwatch = Stopwatch.StartNew();
        var orderedNumbers = numbers.MergeSort();
        stopwatch.Stop();
        var mergeSortTime = stopwatch.ElapsedMilliseconds;

        stopwatch.Restart();
        numbers.InsertionSort();
        stopwatch.Stop();
        var insertionSortTime = stopwatch.ElapsedMilliseconds;

        _logger.LogInformation($"Merge Sort Time: {mergeSortTime} ms, Insertion Sort Time: {insertionSortTime} ms");

        await _numberRepository.Save(orderedNumbers);
    }

    public async Task<string> GetLatest()
    {
        return await _numberRepository.GetLatest();
    }
}