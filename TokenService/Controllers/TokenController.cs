using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using Shared.Utils;
using TokenService.Helpers.Interfaces;
using TokenService.Interfaces;
using UserCredentials = Shared.View_Model.UserAuthenticationViewModel;
using UserRegisterData = Shared.View_Model.UserRegistrationViewModel;

namespace TokenService.Controllers
{
    public class TokenController : ApiController
    {
        private readonly IUserRepository userRepository;
        private readonly ISubscribersEvents subscribersEvents;

        public TokenController(IUserRepository userRepository, ISubscribersEvents subscribersEvents)
        {
            this.userRepository = userRepository;
            this.subscribersEvents = subscribersEvents;
        }

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

        [Route("api/register")]
        [HttpPost]
        [AllowAnonymous]
        public async Task<IHttpActionResult> RegisterNewUser([FromBody] UserRegisterData user)
        {
            if (this.UserAlreadyExists(user.username))
            {
                return BadRequest("User already exists!");
            }

            if (!this.PasswordsMatch(user.password, user.confirm))
            {
                return BadRequest("Passwords don't match");
            }

            try
            {
                await this.userRepository.CreateUser(user);
                await this.subscribersEvents.SendEventToSubscriber(user);

                return Ok("User created successfully");
            }
            catch (System.Exception ex)
            {
                throw new HttpException(ex.Message);
            }
        }

        private User CheckUser(UserCredentials credentials)
        {
            var found = this.GetUserFromDatabase(credentials?.username, credentials?.password.EncryptPassword());
            if (found != null)
            {
                return found;
            }
            return new User() { id = 0 };
        }

        private User GetUserFromDatabase(string username, string password)
        {
            using (var tokenServiceEntities = new TokeServiceDbEntities())
            {
                return tokenServiceEntities.Users.FirstOrDefault(user => user.username == username && user.password == password);
            }
        }

        private bool PasswordsMatch(string password, string confirm)
        {
            return string.Equals(password, confirm);
        }

        private bool UserAlreadyExists(string username)
        {
            using (var tokenServiceEntities = new TokeServiceDbEntities())
            {
                return tokenServiceEntities.Users.Any(user => user.username == username);
           }
        }
    }
}