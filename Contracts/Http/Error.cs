namespace Contracts.Http
{
    public enum ErrorCode
    {
        BadRequest = 40000,
        InvalidMessage = 40001,
        Unauthorized = 40100,
        WrongPassword = 40301,
        ExecutAccessForbidden = 40302,
        UserNotFound = 40401,
        CommentNotFound = 40402,
        GameNotFound = 40403,
        PageNotFound = 40404,
        NotAcceptable = 40600,
        UsernameIsAlreadyInUse = 40901,
        EmailIsAlreadyInUse = 40902,
        CommetIsAlreadyRated = 40903,
        InternalServerError = 50000,
        DbFailureError = 50001
    }

    public class ErrorResponse
    {
        public ErrorCode Code { get; init; }
        public string Message { get; init; }
    }
}