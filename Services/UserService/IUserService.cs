using DTO;
using Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_System.Services.UserService
{
    public interface IUserService
    {
        Task<(AuthenticateResultEnum Result, string? message)> Insert(UserDTO user);
        Task<UserDTO> GetUserById(Guid id);
    }
}
