using System.ComponentModel.DataAnnotations;
using AvaStorage.Application.UseCases.PutAvatar;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AvaStorage.Controllers
{
    [ApiController]
    [Route("v1/ava")]
    public class AvaController(IMediator mediator) : ControllerBase
    {

        [HttpPut]
        public async Task<IActionResult> PutAsync
            (
                [FromQuery(Name = "id")][Required(AllowEmptyStrings = false)] string id,
                [FromQuery(Name = "st")] string? subjectType,
                [FromBody][Required] byte[] picture,
                CancellationToken cancellationToken
            )
        {
            await mediator.Send(new PutAvatarCommand(id, subjectType, picture), cancellationToken);

            return Created();
        }

        [HttpGet]
        public Task<IActionResult> GetAsync
            (
                [FromQuery]string id, 
                [FromQuery]int? size, 
                CancellationToken cancellationToken
            )
        {

        }
    }
}
