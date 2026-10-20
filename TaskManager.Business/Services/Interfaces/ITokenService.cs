namespace TaskManager.Business.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(int userId, string name, string email);
    }
}