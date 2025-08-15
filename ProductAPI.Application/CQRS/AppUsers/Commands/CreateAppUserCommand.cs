using MediatR;
using Microsoft.Extensions.Logging;
using ProductAPI.Application.DTOs.AppUserDtos;
using ProductAPI.Core.Entities;
using ProductAPI.Infrastructure.Helpers;
using ProductAPI.Infrastructure.Repositories;

namespace ProductAPI.Application.CQRS.AppUsers.Commands
{
    public record CreateAppUserCommand(AppUserCreateRequestDto request) : IRequest<AppUserLoginResponseDto>;

    public class CreateAppUserCommandHandler(IGenericRepository<AppUser> repo, ITokenHandler tokenHandler, IPasswordHasher passwordHasher, ILogger<CreateAppUserCommand> logger) : IRequestHandler<CreateAppUserCommand, AppUserLoginResponseDto>
    {
        private readonly IGenericRepository<AppUser> _repo = repo;
        private readonly ILogger<CreateAppUserCommand> _logger = logger;
        private readonly IPasswordHasher _passwordHasher = passwordHasher;
        private readonly ITokenHandler _tokenHandler = tokenHandler;

        public async Task<AppUserLoginResponseDto> Handle(CreateAppUserCommand command, CancellationToken ct)
        {
            var req = command.request;
            var isExist = await _repo.GetAllAsync(x => x.UserName == req.UserName);
            if (isExist.Any())
            {
                _logger.LogWarning("User with username {UserName} already exists.", req.UserName);
                throw new InvalidOperationException($"User with username {req.UserName} already exists.");
            }

            var hashed = _passwordHasher.Hash(req.Password);

            var newUser = new AppUser
            {
                Id = Guid.NewGuid(),
                UserName = req.UserName,
                PasswordHash = hashed,
            };

            await _repo.AddAsync(newUser);
            await _repo.SaveChangesAsync();

            var token = _tokenHandler.GenerateToken(newUser.Id, newUser.UserName);
            return new AppUserLoginResponseDto{UserName = newUser.UserName,Token = token};
        }
    }
}
