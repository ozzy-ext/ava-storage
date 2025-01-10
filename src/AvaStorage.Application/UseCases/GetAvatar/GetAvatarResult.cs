using AvaStorage.Domain;

namespace AvaStorage.Application.UseCases.GetAvatar;

public record GetAvatarResult(IAvatarFile? AvatarFile);