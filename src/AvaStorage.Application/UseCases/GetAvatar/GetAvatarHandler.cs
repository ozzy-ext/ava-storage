using AvaStorage.Application.Options;
using AvaStorage.Domain.Repositories;
using AvaStorage.Domain.Tools;
using AvaStorage.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Text;
using AvaStorage.Application.Services;
using AvaStorage.Domain.PictureAddressing;
using Microsoft.Extensions.Logging;
using MyLab.Log.Dsl;

namespace AvaStorage.Application.UseCases.GetAvatar
{
    internal class GetAvatarHandler
    (
        IOptions<AvaStorageOptions> options, 
        IPictureRepository pictureRepo,
        IPictureTools pictureTools,
        ILogger<GetAvatarHandler>? logger = null
    ) : IRequestHandler<GetAvatarCommand, GetAvatarResult>
    {
        private IDslLogger? Logger => logger?.Dsl();

        public async Task<GetAvatarResult> Handle(GetAvatarCommand request, CancellationToken cancellationToken)
        {
            if (!AvatarId.TryParse(request.Id, out var avatarId))
                throw new ValidationException("Avatar ID has wrong format");

            SubjectType? subjectType = null;
            if (request.SubjectType != null && !SubjectType.TryParse(request.SubjectType, out subjectType))
                throw new ValidationException("Subject type has wrong format");

            if (request.Size.HasValue && !new PictureSizeValidator(options.Value.MaxSize).IsValid(request.Size.Value))
                throw new ValidationException("Wrong size value");

            var loadedPictureBin = await LoadedPictureAsync(avatarId!, subjectType, request.Size, cancellationToken);
            AvatarPicture? loadedPicture = null;

            if (loadedPictureBin != null)
            {
                loadedPicture = await pictureTools.DeserializeAsync(loadedPictureBin, cancellationToken);

                if (loadedPicture != null && request.Size.HasValue)
                {
                    var invalidSize = loadedPicture.Size.Height != request.Size.Value ||
                                      loadedPicture.Size.Width != request.Size.Value;
                    if (invalidSize)
                    {
                        loadedPicture = await pictureTools.FitIntoSizeAsync(loadedPicture, request.Size.Value, cancellationToken);
                        if (loadedPicture == null)
                            throw new InvalidOperationException("Can't normalize picture");
                    }
                }
            }

            byte[]? pictureBin = null;

            if (loadedPicture != null)
            {
                pictureBin = loadedPicture.Binary.Binary.ToArray();
            }

            return new GetAvatarResult(pictureBin);
        }

        private async Task<AvatarPictureBin?> LoadedPictureAsync(AvatarId avatarId, SubjectType? subjectType, int? size, CancellationToken cancellationToken)
        {
            var searchLog = new StringBuilder();

            AvatarPictureBin? loadedPictureBin = null;

            if (size.HasValue)
            {
                loadedPictureBin = await pictureRepo.LoadPictureAsync
                (
                    new PersonalWithSizePicAddrProvider(avatarId, size.Value), 
                    cancellationToken
                );

                AppendSearchLog("Person pic with size");
            }

            if (loadedPictureBin == null)
            {
                loadedPictureBin = await pictureRepo.LoadPictureAsync
                (
                    new OriginalPersonalPicAddrProvider(avatarId),
                    cancellationToken
                );
                AppendSearchLog("Person original pic");
            }

            if (loadedPictureBin == null)
            {
                if (subjectType != null)
                {
                    if (size.HasValue)
                    {
                        loadedPictureBin = await pictureRepo.LoadPictureAsync(
                            new DefaultSubjectTypeWithSizeAddPicProvider(subjectType, size.Value), cancellationToken);
                        AppendSearchLog("Default subject type pic with size");
                    }

                    if (loadedPictureBin == null)
                    {
                        loadedPictureBin =
                            await pictureRepo.LoadPictureAsync(new DefaultSubjectTypePicAddrProvider(subjectType),
                                cancellationToken);
                        AppendSearchLog("Default subject type pic");
                    }
                }

                if (loadedPictureBin == null)
                {
                    if (size.HasValue)
                    {
                        loadedPictureBin = await pictureRepo.LoadPictureAsync(
                            new DefaultPicWithSizeAddrProvider(size.Value),
                            cancellationToken);
                        AppendSearchLog("Default global pic with size");
                    }

                    if (loadedPictureBin == null)
                    {
                        loadedPictureBin =
                            await pictureRepo.LoadPictureAsync(new DefaultPicAddrProvider(), cancellationToken);
                        AppendSearchLog("Default global pic");
                    }
                }
            }

            Logger?.Debug("Searching for picture")
                .AndFactIs("ava-id", avatarId.ToString())
                .AndFactIs("sub-type", subjectType?.ToString())
                .AndFactIs("size", size)
                .AndFactIs("log", searchLog.ToString())
                .AndFactIs("result", loadedPictureBin != null ? "ok" : "not found")
                .Write();

            return loadedPictureBin;

            void AppendSearchLog(string picName)
            {
                searchLog!.AppendLine(picName + " - " + (loadedPictureBin != null ? "ok" : "not found"));
            }
        }
    }
}
