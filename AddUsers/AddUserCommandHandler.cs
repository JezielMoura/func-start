using MediatR;

public class AddUserCommandHandler : IRequestHandler<AddUserCommand, bool>
{
    public async Task<bool> Handle(AddUserCommand request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(true);
    }
}