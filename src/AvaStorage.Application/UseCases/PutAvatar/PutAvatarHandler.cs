using System.ComponentModel.DataAnnotations;
using AvaStorage.Application.Options;
using AvaStorage.Domain.Repositories;
using AvaStorage.Domain.Tools;
using AvaStorage.Domain.ValueObjects;
using MediatR;
using Microsoft.Extensions.Options;

namespace AvaStorage.Application.UseCases.PutAvatar
{
    class PutAvatarHandler : IRequestHandler<PutAvatarCommand>
    {
        private readonly AvaStorageOptions _opts;
        private readonly IPictureRepository _picRepo;

        public PutAvatarHandler(IOptions<AvaStorageOptions> opts, IPictureRepository picRepo)
            : this(opts.Value, picRepo)
        {
        }

        public PutAvatarHandler(AvaStorageOptions opts, IPictureRepository picRepo)
        {
            _opts = opts;
            _picRepo = picRepo;
        }

        public Task Handle(PutAvatarCommand request, CancellationToken cancellationToken)
        {
            if (!AvatarId.TryParse(request.Id, out var avatarId))
                throw new ValidationException("Avatar ID has wrong format");

            SubjectType? subjectType = null;
            if (request.SubjectType != null && !SubjectType.TryParse(request.SubjectType, out subjectType))
                throw new ValidationException("Subject type has wrong format");

            if (!AvatarPicture.TryLoad(request.Picture, out var avatarPicture))
                throw new ValidationException("Avatar picture has wrong format");

            if(new PictureValidator{MaxPictureSize = _opts.MaxAvaSize}.IsValid(avatarPicture!))
                throw new ValidationException("Avatar picture is invalid");

            return _picRepo.SavePictureAsync(avatarId!, subjectType!, avatarPicture!);
        }
    }
}
