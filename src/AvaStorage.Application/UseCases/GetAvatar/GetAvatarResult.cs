using AvaStorage.Domain.ValueObjects;

namespace AvaStorage.Application.UseCases.GetAvatar;

public record GetAvatarResult(byte[]? AvatarPicture);