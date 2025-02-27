﻿using AvaStorage.Application.UseCases.GetAvatar;
using AvaStorage.Domain.Repositories;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace AvaStorage.Tests
{
    public static class TestTools
    {
        public static readonly Mock<IPictureRepository> DefaultRepoMock = new();

        public static readonly byte[] PictureBin = new byte[] { 1, 2, 3 };

        public static Action<IServiceCollection> AddGetMocks(
            Mock<IPictureRepository> picRepo,
            Mock<IRequestHandler<GetAvatarCommand, GetAvatarResult>> handler)
        {
            var outHandlerDescriptor = ServiceDescriptor.Transient(typeof(IRequestHandler<GetAvatarCommand, GetAvatarResult>), s => handler.Object);

            return s => s
                .Replace(outHandlerDescriptor)
                .AddSingleton(picRepo.Object);
        }
    }
}