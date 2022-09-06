using MediatR;

public class AddUserCommandHandler : IRequestHandler<AddUserCommand, bool>
{
    public async Task<bool> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        if (request.Name.Equals("Jeziel Moura"))
            throw new DomainException("Usuário já cadastrado");

        return await Task.FromResult(true);
    }
}