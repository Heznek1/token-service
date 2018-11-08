using System.Linq;
using System.Net;
using System.Web.Http;
using Shared.Utils;
using UserCredentials = Shared.View_Model.UserAuthenticationViewModel;

namespace TokenService.Controllers
{
    public class TokenController : ApiController
    {
        [Route("api/authenticate")]
        [HttpPost]
        [AllowAnonymous]
        public dynamic Get(UserCredentials credentials)
        {
            if (string.IsNullOrEmpty(credentials?.username) || string.IsNullOrEmpty(credentials?.password))
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }

            var user = this.CheckUser(credentials);
            if (user.id != 0)
            {
                return JwtManager.GenerateToken(user);
            }

            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        private User CheckUser(UserCredentials credentials)
        {
            var found = this.GetUserFromDatabase(credentials?.username, credentials?.password.EncryptPassword());
            if (found != null)
            {
                return found;
            }
            return new User(){ id = 0 };
        }

        private User GetUserFromDatabase(string username, string password)
        {
            using (var tokenServiceEntities = new TokeServiceDbEntities())
            {
                return tokenServiceEntities.Users.FirstOrDefault(user => user.username == username && user.password == password);
            }
        }
    }
}