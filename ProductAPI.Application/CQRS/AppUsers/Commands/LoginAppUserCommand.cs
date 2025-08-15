using MediatR;
using Microsoft.Extensions.Logging;
using ProductAPI.Application.DTOs.AppUserDtos;
using ProductAPI.Core.Entities;
using ProductAPI.Infrastructure.Helpers;
using ProductAPI.Infrastructure.Repositories;

namespace ProductAPI.Application.CQRS.AppUsers.Commands
{
    public record LoginAppUserCommand(AppUserLoginRequestDto Request) : IRequest<AppUserLoginResponseDto>;

    public class LoginUserCommandHandler(IGenericRepository<AppUser> repo, ITokenHandler tokenHandler, IPasswordHasher passwordHasher, ILogger<LoginAppUserCommand> logger) : IRequestHandler<LoginAppUserCommand, AppUserLoginResponseDto>
    {
        private readonly IGenericRepository<AppUser> _repo;
        private readonly ILogger<LoginAppUserCommand> _logger = logger;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly ITokenHandler _tokenHandler = tokenHandler;

        public async Task<AppUserLoginResponseDto> Handle(LoginAppUserCommand command, CancellationToken ct)
        {
            var req = command.Request;

            var user = (await _repo.GetAllAsync(x => x.UserName == req.UserName)).FirstOrDefault()
                        ?? throw new ArgumentException("Invalid username or password");
            if(!_passwordHasher.Verify(req.Password, user.PasswordHash))
            {
                _logger.LogInformation("Invalid password for user {UserName}", req.UserName);
                throw new ArgumentException("Invalid username or password");
            }

            var token = _tokenHandler.GenerateToken(user.Id, user.UserName);
            return new AppUserLoginResponseDto { UserName = user.UserName, Token = token };
        }
    }
}
