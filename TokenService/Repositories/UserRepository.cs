using System.Threading.Tasks;
using Shared.Utils;
using TokenService.Interfaces;
using UserRegisterData = Shared.View_Model.UserRegistrationViewModel;

namespace TokenService.Repositories
{
    public class UserRepository : IUserRepository
    {
        public async Task CreateUser(UserRegisterData user)
        {
            using (var db = new TokeServiceDbEntities())
            {
                db.Users.Add(this.MapUser(user));
                await db.SaveChangesAsync();
            }
        }

        private User MapUser(UserRegisterData newUser)
        {
            return new User()
            {
                role = (int) Shared.Identity_Provider.Role.Candidate,
                guid = newUser.guid,
                instance = (int) Shared.Identity_Provider.Instance.HeznekService,
                email = newUser.email,
                username = newUser.username,
                password = newUser.password.EncryptPassword()
            };
        }
    }
}