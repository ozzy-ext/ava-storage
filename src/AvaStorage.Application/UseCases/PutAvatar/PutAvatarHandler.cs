using System.ComponentModel.DataAnnotations;
using System.Threading;
using AvaStorage.Application.Services;
using AvaStorage.Domain;
using AvaStorage.Domain.PictureAddressing;
using AvaStorage.Domain.Repositories;
using AvaStorage.Domain.Tools;
using AvaStorage.Domain.ValueObjects;
using MediatR;

namespace AvaStorage.Application.UseCases.PutAvatar
{
    class PutAvatarHandler
    (
        IPictureRepository pictureRepo,
        IImageMetadataExtractor metadataExtractor,
        IImageModifier imageModifier
    ) : IRequestHandler<PutAvatarCommand>
    {
        public async Task Handle(PutAvatarCommand request, CancellationToken cancellationToken)
        {
            if (!AvatarId.TryParse(request.Id, out var avatarId))
                throw new ValidationException("Avatar ID has wrong format");

            var imgFormat = await CheckAndExtractFormat(request.Picture, cancellationToken);

            IAvatarFile avatarFile = new MemoryAvatarFile(request.Picture);

            if (imgFormat != "PNG")
            {
                avatarFile = await imageModifier.ConvertToInnerFormatAsync(avatarFile, cancellationToken);
            }

            await pictureRepo.SavePictureAsync
                (
                    new OriginalPersonalPicAddrProvider(avatarId!), 
                    avatarFile, 
                    cancellationToken
                );
        }

        private async Task<string> CheckAndExtractFormat(byte[] requestPicture, CancellationToken cancellationToken)
        {
            using var mem = new MemoryStream(requestPicture);
            ImageMetadata imgMeta;
            try
            {
                imgMeta = await metadataExtractor.ExtractAsync(mem, cancellationToken);
                
            }
            catch (Exception e)
            {
                throw new ValidationException("Avatar picture has wrong format", e);
            }

            if (imgMeta.Format == null)
            {
                throw new ValidationException("Can't detect image format");
            }

            return imgMeta.Format;
        }
    }
}
