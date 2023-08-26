namespace Web.Services.Interfaces
{
    public interface IUserService
    {
        bool IsAuthorized(out int userId);
    }
}