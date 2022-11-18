using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DTO;
using Library_System.Model;
namespace Library_System.Services.AuthorizationService
{
    public interface IAuthorizationUserService
    {
        Task<(Enum.AuthenticateResultEnum Result, UserDTO user)> Authenticate_User(string username, string password);

    }
}
