using AvaStorage.Application.Options;
using AvaStorage.Domain.Repositories;
using AvaStorage.Domain.Tools;
using AvaStorage.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Options;
using System.ComponentModel.DataAnnotations;
using System.Text;
using AvaStorage.Application.Services;
using AvaStorage.Application.Tools;
using AvaStorage.Domain;
using AvaStorage.Domain.PictureAddressing;
using Microsoft.Extensions.Logging;
using MyLab.Log.Dsl;

namespace AvaStorage.Application.UseCases.GetAvatar
{
    internal class GetAvatarHandler
    (
        IOptions<AvaStorageOptions> options, 
        IPictureRepository pictureRepo,
        IImageModifier imageModifier,
        ILogger<GetAvatarHandler>? logger = null
    ) : IRequestHandler<GetAvatarCommand, GetAvatarResult>
    {
        private readonly SearchPictureLogic _searchLogic = new (pictureRepo) { Logger = logger?.Dsl() };

        public async Task<GetAvatarResult> Handle(GetAvatarCommand request, CancellationToken cancellationToken)
        {
            if (!AvatarId.TryParse(request.Id, out var avatarId))
                throw new ValidationException("Avatar ID has wrong format");

            SubjectType? subjectType = null;
            if (request.SubjectType != null && !SubjectType.TryParse(request.SubjectType, out subjectType))
                throw new ValidationException("Subject type has wrong format");

            if (request.Size.HasValue && !new ImageValidator(options.Value.MaxRequestedSize).IsValidSize(request.Size.Value))
                throw new ValidationException("Wrong size value");

            var foundPicture = await _searchLogic.SearchPictureAsync(avatarId!, subjectType, request.Size, cancellationToken);

            var resultPicture = foundPicture.File;

            if (foundPicture.File == null || !request.Size.HasValue || foundPicture.BySize)
                return new GetAvatarResult(resultPicture);
            
            resultPicture = await imageModifier.FitIntoSizeAsync
                (
                    foundPicture.File, 
                    request.Size.Value, 
                    cancellationToken
                );
            
            if (resultPicture == null)
                throw new InvalidOperationException("Can't normalize picture");

            return new GetAvatarResult(resultPicture);
        }
    }
}
