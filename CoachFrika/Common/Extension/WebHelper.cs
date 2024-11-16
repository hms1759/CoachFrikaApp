using System.Security.Claims;

namespace CoachFrika.Common.Extension
{

    public interface IWebHelpers
    {
        public string CurrentUser();
        public string CurrentUserId();
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
            string name ="";
            ClaimsIdentity user = (ClaimsIdentity)_httpContextAccessor.HttpContext.User.Identity;

            if (user.Name == null)
            {
                var email = user.FindFirst("unique_name");
                name = email?.Value;
            }
           
            return name;
        }

        public string CurrentUserId()
        {
            throw new NotImplementedException();
        }
    }
}
