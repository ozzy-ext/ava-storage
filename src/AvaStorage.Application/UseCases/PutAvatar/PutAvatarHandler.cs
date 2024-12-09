using System.ComponentModel.DataAnnotations;
using AvaStorage.Application.Options;
using AvaStorage.Application.Services;
using AvaStorage.Domain.Repositories;
using AvaStorage.Domain.Tools;
using AvaStorage.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Options;

namespace AvaStorage.Application.UseCases.PutAvatar
{
    class PutAvatarHandler
    (
        IOptions<AvaStorageOptions> options, 
        IPictureRepository pictureRepo,
        IPictureTools pictureTools
    ) : IRequestHandler<PutAvatarCommand>
    {
        public async Task Handle(PutAvatarCommand request, CancellationToken cancellationToken)
        {
            if (!AvatarId.TryParse(request.Id, out var avatarId))
                throw new ValidationException("Avatar ID has wrong format");

            if(!AvatarPictureBin.TryDeserialize(request.Picture, out var pictureBin))
                throw new ValidationException("Avatar picture wrong binary");

            var avatarPicture = await pictureTools.DeserializeAsync(pictureBin!, cancellationToken);

            if (avatarPicture == null)
                throw new ValidationException("Avatar picture has wrong format");

            if(!new PictureValidator(options.Value.MaxSize).IsValid(avatarPicture!))
                throw new ValidationException("Avatar picture is invalid");

            await pictureRepo.SavePictureAsync(avatarId!, avatarPicture.Binary);
        }
    }
}
