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
    [Host(ListenConstants.AdminHost)]
    [Route("v1/ava")]
    public class AdminController(IMediator mediator) : ControllerBase
    {

        [HttpPut]
        [Consumes("application/octet-stream")]
        [ErrorToResponse(typeof(ValidationException), HttpStatusCode.BadRequest)]
        public async Task<IActionResult> PutAsync
            (
                [FromQuery(Name = "id")][Required(AllowEmptyStrings = false)] string id,
                [FromBody][Required] byte[] picture,
                CancellationToken cancellationToken
            )
        {
            await mediator.Send(new PutAvatarCommand(id, picture), cancellationToken);

            return Ok();
        }

        [HttpGet]
        [ErrorToResponse(typeof(ValidationException), HttpStatusCode.BadRequest)]
        public async Task<HttpResponseMessage> GetAsync
            (
                [FromQuery(Name = "id")][Required]string id, 
                [FromQuery(Name = "sz")]int? size, 
                [FromQuery(Name = "st")]string? subjectType, 
                CancellationToken cancellationToken
            )
        {
            var result = await mediator.Send(new GetAvatarCommand(id, size, subjectType), cancellationToken);

            if (result.AvatarPicture == null) return new HttpResponseMessage(HttpStatusCode.NotFound);

            return new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new ByteArrayContent(result.AvatarPicture)
            };
        }
    }
}
