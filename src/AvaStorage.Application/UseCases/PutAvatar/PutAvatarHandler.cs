using MediatR;

namespace AvaStorage.Application.UseCases.PutAvatar
{
    class PutAvatarHandler : IRequestHandler<PutAvatarCommand>
    {
        public Task Handle(PutAvatarCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
