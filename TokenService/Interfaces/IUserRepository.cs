using System.Threading.Tasks;
using UserRegisterData = Shared.View_Model.UserRegistrationViewModel;

namespace TokenService.Interfaces
{
    public interface IUserRepository
    {
        Task CreateUser(UserRegisterData user);
    }
}