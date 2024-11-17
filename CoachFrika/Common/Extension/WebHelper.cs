using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Xml.Linq;

namespace CoachFrika.Common.Extension
{

    public interface IWebHelpers
    {
        public string CurrentUser();
        public string CurrentUserId();
        public string CurrentUserRole();
    }
    public class WebHelpers : IWebHelpers
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        public string LoggedInUserId;
        public WebHelpers(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;

        }
        public string CurrentUser()
        {
            string name = "";
            ClaimsIdentity currentUser = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;

            if (currentUser.Claims != null)
            {
                name = currentUser.FindFirst(ClaimTypes.Name)?.Value;
                return name;
            }

            throw new NotImplementedException("User not found");
        }

        public string CurrentUserId()
        {
            string nameIdentifier = "";
            ClaimsIdentity currentUser = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;

            if (currentUser.Claims != null)
            {
                nameIdentifier = currentUser.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                return nameIdentifier;
            }
            throw new NotImplementedException("User not found");
        }

        public string CurrentUserRole()
        {
            string role = "";

            ClaimsIdentity currentUser = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;

            if (currentUser.Claims != null)
            {
                var roles = currentUser.FindFirst(ClaimTypes.Role)?.Value;
                role = roles != null && roles.Any() ? string.Join(", ", roles) : null;
                return role;
            }

            throw new NotImplementedException("User not found");
        }

    }
}
