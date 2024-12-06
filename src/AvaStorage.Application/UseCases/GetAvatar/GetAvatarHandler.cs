using AvaStorage.Application.Options;
using AvaStorage.Domain.Repositories;
using AvaStorage.Domain.Tools;
using AvaStorage.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using AvaStorage.Infrastructure.Services;

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

            var loadedPicture = await LoadedPictureAsync(avatarId!, subjectType, request.Size);

            if (loadedPicture != null && request.Size.HasValue)
            {
                var invalidSize = loadedPicture.Size.Height != request.Size.Value ||
                                  loadedPicture.Size.Width != request.Size.Value;
                if (invalidSize)
                {
                    loadedPicture = await pictureTools.NormalizeAsync(loadedPicture, request.Size.Value, cancellationToken);
                    if (loadedPicture == null)
                        throw new InvalidOperationException("Can't normalize picture");
                }
            }

            byte[]? pictureBin = null;

            if (loadedPicture != null)
            {
                pictureBin = loadedPicture.Binary.ToArray();
            }

            return new GetAvatarResult(pictureBin);
        }

        private async Task<AvatarPicture?> LoadedPictureAsync(AvatarId avatarId, SubjectType? subjectType, int? size)
        {
            AvatarPicture? loadedPicture = size.HasValue
                ? await pictureRepo.LoadPersonalPictureWithSizeAsync(avatarId, size.Value)
                : await pictureRepo.LoadOriginalPersonalPictureAsync(avatarId);

            if (loadedPicture != null)
                return loadedPicture;

            if (size.HasValue)
                loadedPicture = await pictureRepo.LoadPersonalPictureWithSizeAsync(avatarId, size.Value);
            loadedPicture ??= await pictureRepo.LoadOriginalPersonalPictureAsync(avatarId);

            if (loadedPicture != null) 
                return loadedPicture;
            
            if (subjectType != null)
            {
                if(size.HasValue)
                    loadedPicture = await pictureRepo.LoadSubjectTypePictureWithSizeAsync(subjectType, size.Value);
                loadedPicture ??= await pictureRepo.LoadDefaultSubjectTypePictureAsync(subjectType);
            }

            if (loadedPicture != null)
                return loadedPicture;

            if (size.HasValue)
                loadedPicture = await pictureRepo.LoadDefaultPictureWithSizeAsync(size.Value);
            loadedPicture ??= await pictureRepo.LoadDefaultPictureAsync();

            return loadedPicture;
        }
    }
}
