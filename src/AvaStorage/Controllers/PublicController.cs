using System.ComponentModel.DataAnnotations;
using System.Net;
using AvaStorage.Application.UseCases.GetAvatar;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyLab.WebErrors;

namespace AvaStorage.Controllers
{
    [ApiController]
    [Host(ListenConstants.PublicHost)]
    [Route("v1/ava")]
    public class PublicController(IMediator mediator) : ControllerBase
    {
        [HttpGet("{id}")]
        [ErrorToResponse(typeof(ValidationException), HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAsync
        (
            [FromRoute(Name = "id")][Required] string id,
            [FromQuery(Name = "sz"), Range(0, int.MaxValue)] int? size,
            [FromQuery(Name = "st")] string? subjectType,
            CancellationToken cancellationToken
        )
        {
            var result = await mediator.Send(new GetAvatarCommand(id, size, subjectType), cancellationToken);

            if (result?.AvatarPicture == null) return NotFound();

            var mem = new MemoryStream(result.AvatarPicture);
            return base.File(mem, "application/octet-stream");
        }
    }
}