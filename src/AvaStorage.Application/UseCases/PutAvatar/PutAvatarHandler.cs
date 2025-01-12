using System.ComponentModel.DataAnnotations;
using AvaStorage.Application.Options;
using AvaStorage.Application.Services;
using AvaStorage.Domain;
using AvaStorage.Domain.PictureAddressing;
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
        IImageMetadataExtractor metadataExtractor
    ) : IRequestHandler<PutAvatarCommand>
    {
        public async Task Handle(PutAvatarCommand request, CancellationToken cancellationToken)
        {
            if (!AvatarId.TryParse(request.Id, out var avatarId))
                throw new ValidationException("Avatar ID has wrong format");

            ImageMetadata avatarImageMetadata;

            using var mem = new MemoryStream(request.Picture);
            try
            {
                avatarImageMetadata = await metadataExtractor.ExtractAsync(mem, cancellationToken);
            }
            catch (Exception e)
            {
                throw new ValidationException("Avatar picture has wrong format", e);
            }

            if(!new ImageValidator(options.Value.MaxSize).IsValid(avatarImageMetadata!))
                throw new ValidationException("Avatar picture is invalid");

            await pictureRepo.SavePictureAsync
                (
                    new OriginalPersonalPicAddrProvider(avatarId!), 
                    new MemoryAvatarFile(request.Picture, name: "origin"), 
                    cancellationToken
                );
        }
    }
}
