namespace Monday.WebApi.Validators;

using FluentValidation;
using Monday.WebApi.Commands;
using Monday.WebApi.Interfaces;

public sealed class AddBookValidator : AbstractValidator<AddBook>
{
    private readonly IBookReadService readService;

    public AddBookValidator(IBookReadService readService)
    {
        this.readService = readService;

        RuleFor(book => book.Name)
            .NotEmpty()
            .WithMessage("Book name can not be empty");

        RuleFor(book => book.ISBN)
            .NotEmpty()
            .WithMessage("Book ISBN can not be empty");

        RuleFor(book => book.ISBN)
            .MustAsync(this.BeUniqueIsbn)
            .WithMessage("Book ISBN must be unique");
    }

    private async Task<bool> BeUniqueIsbn(string isbn, CancellationToken cancellationToken = default)
    {
        return await this.readService.IsExists(isbn, cancellationToken) is false;
    }
}
