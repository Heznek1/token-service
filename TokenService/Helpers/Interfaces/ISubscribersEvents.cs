using System;
using System.Threading.Tasks;
using Shared.View_Model;

namespace TokenService.Helpers.Interfaces
{
    public interface ISubscribersEvents
    {
        Task SendEventToSubscriber(UserRegistrationViewModel user);
    }
}