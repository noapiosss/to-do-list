using Microsoft.AspNetCore.Http;
using Web.Services.Interfaces;

namespace Web.Services
{
    public class UserService : IUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ITokenHandler _tokenHandler;

        public UserService(IHttpContextAccessor httpContextAccessor,ITokenHandler tokenHandler)
        {
            _httpContextAccessor = httpContextAccessor;
            _tokenHandler = tokenHandler;
        }

        public bool IsAuthorized(out int userId)
        {
            if (_httpContextAccessor.HttpContext.Request.Cookies.TryGetValue("jwt", out string token))
            {
                return _tokenHandler.Validate(token, out userId);
            }

            userId = -1;
            return false;
        }
    }
}