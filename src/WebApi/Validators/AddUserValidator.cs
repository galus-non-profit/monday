namespace Monday.WebApi.Validators;

using FluentValidation;
using Monday.WebApi.Commands;
using Monday.WebApi.Interfaces;

public sealed class AddUserValidator : AbstractValidator<AddUser>
{
    private readonly IUserReadService userReadService;

    public AddUserValidator(IUserReadService userReadService)
    {
        this.userReadService = userReadService;

        RuleFor(user => user.Id)
            .NotEmpty()
            .WithMessage("User Id can not be empty");

        RuleFor(user => user.Name)
            .NotEmpty()
            .MustAsync(this.BeUniqueUserId)
            .WithMessage("User name can not be empty");

        RuleFor(user => user.Email)
            .NotEmpty()
            .WithMessage("Email can not be empty");

        RuleFor(user => user.Password)
            .NotEmpty()
            .WithMessage("User passwordHashed can not be empty");
    }

    private async Task<bool> BeUniqueUserId(string name, CancellationToken cancellationToken = default)
    {
        return await this.userReadService.IsExists(name, cancellationToken);
    }
}