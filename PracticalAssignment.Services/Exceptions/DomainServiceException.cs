namespace PracticalAssignment.Services.Exceptions;

public class DomainServiceException : Exception
{
    public DomainServiceException(string message)
        : base(message)
    { }
}