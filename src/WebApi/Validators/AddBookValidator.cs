using FluentValidation;
using Monday.WebApi.Commands;
using Monday.WebApi.Interfaces;

namespace Monday.WebApi.Validators;

public sealed class AddBookValidator : AbstractValidator<AddBook>
{
    private readonly IBookReadService readService;

    public AddBookValidator(IBookReadService readService)
    {
        this.readService = readService;

        this.RuleFor(book => book.Name)
            .NotEmpty()
            .WithMessage("Book name can not be empty");

        this.RuleFor(book => book.ISBN)
            .NotEmpty()
            .WithMessage("Book ISBN can not be empty");

        this.RuleFor(book => book.ISBN)
            .MustAsync(BeUniqueIsbn)
            .WithMessage("Book ISBN must be unique");
    }

    private async Task<bool> BeUniqueIsbn(string isbn, CancellationToken cancellationToken = default)
    {
        return await this.readService.IsExists(isbn, cancellationToken) is false;
    }
}