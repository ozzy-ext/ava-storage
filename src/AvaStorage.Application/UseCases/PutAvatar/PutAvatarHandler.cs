using System.ComponentModel.DataAnnotations;
using AvaStorage.Application.Options;
using AvaStorage.Domain.Repositories;
using AvaStorage.Domain.Tools;
using AvaStorage.Domain.ValueObjects;
using AvaStorage.Infrastructure.Services;
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

            var avatarPicture = await pictureTools.DeserializeAsync(request.Picture, cancellationToken);

            if (avatarPicture == null)
                throw new ValidationException("Avatar picture has wrong format");

            if(!new PictureValidator(options.Value.MaxSize).IsValid(avatarPicture!))
                throw new ValidationException("Avatar picture is invalid");

            await pictureRepo.SavePictureAsync(avatarId!, avatarPicture!);
        }
    }
}
