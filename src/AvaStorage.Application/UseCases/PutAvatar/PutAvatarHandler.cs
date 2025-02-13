using System.ComponentModel.DataAnnotations;
using System.Threading;
using AvaStorage.Application.Services;
using AvaStorage.Domain;
using AvaStorage.Domain.PictureAddressing;
using AvaStorage.Domain.Repositories;
using AvaStorage.Domain.ValueObjects;
using MediatR;

namespace AvaStorage.Application.UseCases.PutAvatar
{
    class PutAvatarHandler
    (
        IPictureRepository pictureRepo,
        IImageMetadataExtractor metadataExtractor
    ) : IRequestHandler<PutAvatarCommand>
    {
        public async Task Handle(PutAvatarCommand request, CancellationToken cancellationToken)
        {
            if (request.Format == ImageFormat.Undefined)
                throw new ValidationException("Undefined image format");

            if (!AvatarId.TryParse(request.Id, out var avatarId))
                throw new ValidationException("Avatar ID has wrong format");

            await CheckRightImageFormatAsync(request.Picture, cancellationToken);

            await pictureRepo.SavePictureAsync
                (
                    new OriginalPersonalPicAddrProvider(avatarId!), 
                    new MemoryAvatarFile(request.Picture, name: "origin"), 
                    cancellationToken
                );
        }

        private async Task  CheckRightImageFormatAsync(byte[] requestPicture, CancellationToken cancellationToken)
        {
            using var mem = new MemoryStream(requestPicture);
            try
            {
                var avatarImageMetadata = await metadataExtractor.ExtractAsync(mem, cancellationToken);
            }
            catch (Exception e)
            {
                throw new ValidationException("Avatar picture has wrong format", e);
            }
        }
    }
}
