namespace ChatNest.Services
{
    public interface IJwtService
    {
        Task<string> GenerateAccessTokenAsync(Guid userID);
        Task<string> GenerateRefreshTokenAsync();
    }

}
