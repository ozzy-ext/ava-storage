using AvaStorage.Domain.ValueObjects;
using MediatR;

namespace AvaStorage.Application.UseCases.PutAvatar
{
    public record PutAvatarCommand(string Id, string? SubjectType, byte[] Picture) : IRequest;

}
