using MediatR;

public record AddUserCommand(Guid Id, string Name) : IRequest<bool>;