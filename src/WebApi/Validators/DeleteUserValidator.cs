namespace Monday.WebApi.Validators;

using FluentValidation;
using Monday.WebApi.Commands;

public sealed class DeleteUserValidator : AbstractValidator<AddUser>
{
    public DeleteUserValidator()
    {
        RuleFor(user => user.Id)
            .NotEmpty()
            .WithMessage("User Id can not be empty");
    }
}
