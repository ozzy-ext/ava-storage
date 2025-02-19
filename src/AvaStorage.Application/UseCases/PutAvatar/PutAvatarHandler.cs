using System.ComponentModel.DataAnnotations;
using System.Threading;
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
        IPictureRepository pictureRepo,
        IImageMetadataExtractor metadataExtractor,
        IImageModifier imageModifier,
        IOptions<AvaStorageOptions>? opt = null
        ) : IRequestHandler<PutAvatarCommand>
    {
        public async Task Handle(PutAvatarCommand request, CancellationToken cancellationToken)
        {
            if (!AvatarId.TryParse(request.Id, out var avatarId))
                throw new ValidationException("Avatar ID has wrong format");

            var imgMeta = await CheckAndExtractFormat(request.Picture, cancellationToken);

            IAvatarFile avatarFile = new MemoryAvatarFile(request.Picture);

            if (IsNotInnerFormat(imgMeta))
            {
                avatarFile = await imageModifier.ConvertToInnerFormatAsync(avatarFile, cancellationToken);
            }

            var predefinedSizes = opt?.Value.PredefinedSizes?.Where(s => s > 0).ToArray();

            if (predefinedSizes != null)
            {
                await CreatePredefinedSizeCopiesAsync(request.Id, predefinedSizes, avatarFile, cancellationToken);
            }

            await pictureRepo.SavePictureAsync
                (
                    new OriginalPersonalPicAddrProvider(avatarId!), 
                    avatarFile, 
                    cancellationToken
                );
        }

        private async Task CreatePredefinedSizeCopiesAsync(string avaId, int[] predefinedSizes, IAvatarFile avaFile, CancellationToken cancellationToken)
        {
            foreach (var predefinedSize in predefinedSizes)
            {
                var sizeAvaFile = await imageModifier.FitIntoSizeAsync(avaFile, predefinedSize, cancellationToken);
                var addrProvider = new PersonalWithSizePicAddrProvider(avaId, predefinedSize);

                await pictureRepo.SavePictureAsync(addrProvider, sizeAvaFile,cancellationToken);
            }
        }

        bool IsNotInnerFormat(ImageMetadata imgMeta) => imgMeta.Format != "PNG";

        private async Task<ImageMetadata> CheckAndExtractFormat(byte[] requestPicture, CancellationToken cancellationToken)
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

            return imgMeta;
        }
    }
}
