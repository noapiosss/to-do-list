namespace Web.Services.Interfaces
{
    public interface ITokenHandler
    {
        string GenerateToken(int userId);
        bool Validate(string token, out int userId);
    }
}