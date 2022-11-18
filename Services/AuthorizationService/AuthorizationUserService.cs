using AutoMapper;
using DTO;
using Enum;
using Library_System.Model;
using Library_System.Repositories.Generic.Interface;
using Microsoft.Extensions.Configuration;
using Services.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_System.Services.AuthorizationService
{
    public class AuthorizationUserService : IAuthorizationUserService
    {
        private readonly IUnitOfWork _IUnitOfWork;
        public AuthorizationUserService(IUnitOfWork IUnitOfWork /*Library_System.Services.AuthorizationService.IAuthorizationService authorizationService*/ )
        {
            //_mapper = mapper;
            _IUnitOfWork = IUnitOfWork;
            //_authorizationService = authorizationService;
        }
        public async Task<(AuthenticateResultEnum Result, UserDTO user)> Authenticate_User(string email, string password)
        {
            if (string.IsNullOrEmpty(email))
                throw new ArgumentNullException("email");
            if (string.IsNullOrEmpty(password))
                throw new ArgumentNullException("password");

            email = email.Trim();
            password = password.Trim();

            User data =  await _IUnitOfWork.UserRepository.GetBy(x => x.Email == email );

            if (data == null)
                return (Enum.AuthenticateResultEnum.wrongCredinal, null!);

            password = password.ComputeSha256Hash(data.Id.ToString());

            if (data.Password != password)
                return (Enum.AuthenticateResultEnum.wrongCredinal, null!);

            if (!data.Is_Active)
                return (Enum.AuthenticateResultEnum.AccountDisabled, null!);

            try
            {
                UserDTO Auth_user = new UserDTO()
                {
                    Email = data.Email,
                    Id = data.Id,
                    PhoneNumber = data.PhoneNumber,
                    Full_Name=data.Full_Name                
 
                };

                return (Enum.AuthenticateResultEnum.Authenticated, Auth_user);

            }
            catch (Exception ex)
            {
                return (Enum.AuthenticateResultEnum.AccountDisabled, null!);
            }
        }
    }
}
