using System.ComponentModel.DataAnnotations;
using AvaStorage.Domain;
using AvaStorage.Domain.PictureAddressing;
using AvaStorage.Domain.Repositories;
using AvaStorage.Domain.ValueObjects;
using MediatR;

namespace AvaStorage.Application.UseCases.PutAvatar
{
    class PutAvatarHandler
    (
        IPictureRepository pictureRepo
    ) : IRequestHandler<PutAvatarCommand>
    {
        public async Task Handle(PutAvatarCommand request, CancellationToken cancellationToken)
        {
            if (!AvatarId.TryParse(request.Id, out var avatarId))
                throw new ValidationException("Avatar ID has wrong format");
            
            await pictureRepo.SavePictureAsync
                (
                    new OriginalPersonalPicAddrProvider(avatarId!), 
                    new MemoryAvatarFile(request.Picture, name: "origin"), 
                    cancellationToken
                );
        }
    }
}
