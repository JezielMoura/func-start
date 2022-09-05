using FluentValidation;

public class AddUserCommandValidator : AbstractValidator<AddUserCommand>
{
    public AddUserCommandValidator()
    {
        RuleFor(c => c.Name).NotEmpty().WithMessage("O nome deve ser informado");
        RuleFor(c => c.Id).NotEmpty().WithMessage("O id deve ser informado");
    }
}