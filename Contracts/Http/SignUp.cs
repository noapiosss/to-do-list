namespace Contracts.Http
{
    public class SignUpRequest
    {
        public string Username { get; init; }
        public string Email { get; init; }
        public string Password { get; init; }
    }

    public class SignUpResponse
    {
        public bool Success { get; init; }
    }
}