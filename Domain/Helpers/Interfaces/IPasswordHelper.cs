namespace Domain.Helpers.Interfaces
{
    public interface IPasswordHelper
    {
        string ComputeSha256Hash(string password);
    }
}