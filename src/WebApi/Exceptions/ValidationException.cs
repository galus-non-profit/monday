namespace Monday.WebApi.Exceptions;

public sealed class ValidationException : Exception
{
    public ValidationException(string isbn, IEnumerable<string> failures) : base ("Validation exception thrown see validation messages")
    {
        this.Failures = failures;
    }

    public IEnumerable<string> Failures { get; private set; }
}
