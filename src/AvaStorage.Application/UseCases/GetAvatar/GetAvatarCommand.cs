using AvaStorage.Domain.ValueObjects;
using MediatR;

namespace AvaStorage.Application.UseCases.GetAvatar;

public record GetAvatarCommand(string Id, int? Size, string? SubjectType) : IRequest<GetAvatarResult>;