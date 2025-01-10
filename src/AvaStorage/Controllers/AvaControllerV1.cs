using System.ComponentModel.DataAnnotations;
using System.Net;
using AvaStorage.Application.UseCases.GetAvatar;
using AvaStorage.Application.UseCases.PutAvatar;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyLab.WebErrors;

namespace AvaStorage.Controllers
{
    [ApiController]
    [Route("v1/ava")]
    public class AvaControllerV1(IMediator mediator) : ControllerBase
    {

        [HttpPut("{id}")]
        [Consumes("application/octet-stream")]
        [ErrorToResponse(typeof(ValidationException), HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PutAsync
            (
                [FromRoute(Name = "id")][Required(AllowEmptyStrings = false)] string id,
                [FromBody][Required] byte[] picture,
                CancellationToken cancellationToken
            )
        {
            await mediator.Send(new PutAvatarCommand(id, picture), cancellationToken);

            return Ok();
        }

        [HttpGet("{id}")]
        [ErrorToResponse(typeof(ValidationException), HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAsync
            (
                [FromRoute(Name = "id")][Required]string id, 
                [FromQuery(Name = "sz"), Range(0, int.MaxValue)]int? size, 
                [FromQuery(Name = "st")]string? subjectType, 
                CancellationToken cancellationToken
            )
        {
            var result = await mediator.Send(new GetAvatarCommand(id, size, subjectType), cancellationToken);

            if (result.AvatarFile == null) return NotFound();

            return base.File(result.AvatarFile.OpenRead(), "application/octet-stream");
        }
    }
}
