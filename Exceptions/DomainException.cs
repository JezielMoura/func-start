using FluentValidation.Results;

public class DomainException : ValidationException
{    
    public DomainException(string message) : base(message, new List<ValidationFailure> { new("DomainException", message) })
    { }
}