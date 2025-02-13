using MediatR;

namespace AvaStorage.Application.UseCases.PutAvatar
{
    public record PutAvatarCommand(string Id, byte[] Picture, ImageFormat Format) : IRequest;

}
