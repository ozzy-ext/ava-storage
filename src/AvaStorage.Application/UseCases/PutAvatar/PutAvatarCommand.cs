using AvaStorage.Domain.ValueObjects;
using MediatR;

namespace AvaStorage.Application.UseCases.PutAvatar
{
    public record PutAvatarCommand(AvatarId Id, SubjectType SubjectType, byte[] Picture) : IRequest;

}
