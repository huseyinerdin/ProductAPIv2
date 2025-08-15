namespace ProductAPI.Core.Entities
{
    public class AppUser
    {
        public Guid Id { get; set; }

        public string UserName { get; set; } = string.Empty;

        public string PasswordHash { get; set; } = string.Empty;
    }
}
