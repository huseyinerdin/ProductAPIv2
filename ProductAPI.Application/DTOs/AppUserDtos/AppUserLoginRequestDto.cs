namespace ProductAPI.Application.DTOs.AppUserDtos
{
    public class AppUserLoginRequestDto
    {
        public string UserName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
