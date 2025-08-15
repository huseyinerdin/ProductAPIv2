using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductAPI.Application.CQRS.AppUsers.Commands;
using ProductAPI.Application.DTOs.AppUserDtos;

namespace ProductAPI.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IMediator mediatr) : ControllerBase
    {
        private readonly IMediator _mediatr = mediatr;

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AppUserCreateRequestDto dto)
        {
            var created = await _mediatr.Send(new CreateAppUserCommand(dto));
            return created is null ? BadRequest("User could not be created.") : Ok(created);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AppUserLoginRequestDto dto)
        {
            var loginResponse = await _mediatr.Send(new LoginAppUserCommand(dto));
            return loginResponse is null ? Unauthorized("Invalid username or password.") : Ok(loginResponse);
        }
    }
}
