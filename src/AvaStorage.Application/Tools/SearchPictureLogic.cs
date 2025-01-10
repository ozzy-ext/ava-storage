using AvaStorage.Domain.PictureAddressing;
using AvaStorage.Domain;
using AvaStorage.Domain.Repositories;
using AvaStorage.Domain.ValueObjects;
using System.Text;
using MyLab.Log.Dsl;

namespace AvaStorage.Application.Tools;

class SearchPictureLogic(IPictureRepository pictureRepo)
{
    public IDslLogger? Logger { get; set; }

    public async Task<FoundPicture> SearchPictureAsync(AvatarId avatarId, SubjectType? subjectType, int? size, CancellationToken cancellationToken)
    {
        var searchLog = new StringBuilder();

        IAvatarFile? loadedPictureBin = null;
        bool bySize = false;

        if (size.HasValue)
        {
            loadedPictureBin = await pictureRepo.LoadPictureAsync
            (
                new PersonalWithSizePicAddrProvider(avatarId, size.Value),
                cancellationToken
            );
            bySize = loadedPictureBin != null;

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
                    bySize = loadedPictureBin != null;
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
                    bySize = loadedPictureBin != null;
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

        return new FoundPicture(loadedPictureBin, bySize);

        void AppendSearchLog(string picName)
        {
            searchLog!.AppendLine(picName + " - " + (loadedPictureBin != null ? "ok" : "not found"));
        }
    }
}

public record FoundPicture(IAvatarFile? File, bool BySize);