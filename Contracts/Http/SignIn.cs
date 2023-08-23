namespace Contracts.Http
{
    public class SignInRequest
    {
        public string Login { get; init; }
        public string Password { get; init; }
    }

    public class SignInResponse
    {
        public string Token { get; init; }
        public int ExpireAt { get; init; }
    }
}