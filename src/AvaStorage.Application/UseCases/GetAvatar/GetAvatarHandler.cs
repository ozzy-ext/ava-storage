using AvaStorage.Application.Options;
using AvaStorage.Domain.Repositories;
using AvaStorage.Domain.Tools;
using AvaStorage.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using AvaStorage.Application.Services;

namespace AvaStorage.Application.UseCases.GetAvatar
{
    internal class GetAvatarHandler
    (
        IOptions<AvaStorageOptions> options, 
        IPictureRepository pictureRepo,
        IPictureTools pictureTools
    ) : IRequestHandler<GetAvatarCommand, GetAvatarResult>
    {   public async Task<GetAvatarResult> Handle(GetAvatarCommand request, CancellationToken cancellationToken)
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
            AvatarPictureBin? loadedPictureBin = size.HasValue
                ? await pictureRepo.LoadPersonalPictureWithSizeAsync(avatarId, size.Value, cancellationToken)
                : await pictureRepo.LoadOriginalPersonalPictureAsync(avatarId, cancellationToken);

            if (loadedPictureBin != null)
                return loadedPictureBin;

            if (size.HasValue)
                loadedPictureBin = await pictureRepo.LoadPersonalPictureWithSizeAsync(avatarId, size.Value, cancellationToken);
            loadedPictureBin ??= await pictureRepo.LoadOriginalPersonalPictureAsync(avatarId, cancellationToken);

            if (loadedPictureBin != null) 
                return loadedPictureBin;
            
            if (subjectType != null)
            {
                if(size.HasValue)
                    loadedPictureBin = await pictureRepo.LoadSubjectTypePictureWithSizeAsync(subjectType, size.Value, cancellationToken);
                loadedPictureBin ??= await pictureRepo.LoadDefaultSubjectTypePictureAsync(subjectType, cancellationToken);
            }

            if (loadedPictureBin != null)
                return loadedPictureBin;

            if (size.HasValue)
                loadedPictureBin = await pictureRepo.LoadDefaultPictureWithSizeAsync(size.Value, cancellationToken);
            loadedPictureBin ??= await pictureRepo.LoadDefaultPictureAsync(cancellationToken);

            return loadedPictureBin;
        }
    }
}
