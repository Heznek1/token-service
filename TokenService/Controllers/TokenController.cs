using System.Net;
using System.Web.Http;
using UserModel = Shared.Identity_Provider.User;

namespace TokenService.Controllers
{
    public class TokenController : ApiController
    {
        [Route("api/authenticate")]
        [HttpPost]
        [AllowAnonymous]
        public string Get(UserModel user)
        {
            if (CheckUser(user))
            {
                return JwtManager.GenerateToken(user);
            }

            throw new HttpResponseException(HttpStatusCode.Unauthorized);
        }

        public bool CheckUser(UserModel user)
        {
            // should check in the database
            return true;
        }
    }
}
