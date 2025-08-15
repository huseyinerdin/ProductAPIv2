namespace ProductAPI.Infrastructure.Helpers
{
    public interface ITokenHandler
    {
        string GenerateToken(Guid userId, string userName);
    }
}
