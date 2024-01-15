namespace Monday.WebApi.Exceptions;

public class ValidationException : Exception
{
    public IEnumerable<string> failures;
    public ValidationException(string isbn, IEnumerable<string> failures) : base ("Validation exception thrown see validation messages")
    {
        this.failures = failures;
    }
}