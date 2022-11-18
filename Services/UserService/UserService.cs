using AutoMapper;
using DTO;
using Enum;
using Library_System.Repositories.Generic;
using Library_System.Repositories.Generic.Interface;
using Services.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_System.Services.UserService
{
    public class UserService : IUserService
    {      
        private readonly IMapper _mapper;

        private readonly IUnitOfWork _IUnitOfWork;
        public UserService(IUnitOfWork IUnitOfWork , IMapper mapper )
        {
            _mapper = mapper;
            _IUnitOfWork = IUnitOfWork;
            //_authorizationService = authorizationService;
        }

        public async Task<UserDTO> GetUserById(Guid id)
        {
            if (id==null)
                throw new ArgumentNullException("id");  
           var  data=  _IUnitOfWork.UserRepository.GetById(id); 
            UserDTO userdetails = _mapper.Map<UserDTO>(data);

            return userdetails; 

        }

        public async Task<(AuthenticateResultEnum Result, string? message)> Insert(UserDTO user)
        {
            if (user == null)
                throw new ArgumentNullException(nameof(user));
            if (await _IUnitOfWork.UserRepository.Any(X => X.Email == user.Email))
                return (AuthenticateResultEnum.ValidationError, "email exist");
            
            if (await _IUnitOfWork.UserRepository.Any(x => x.PhoneNumber == user.PhoneNumber))          
                return (AuthenticateResultEnum.ValidationError, "phonenumber exist");

            user.Password = user.Password.ComputeSha256Hash(user.Id.ToString());
            var data = _mapper.Map<Model.User>(user);
            data.Is_Active = true;
            try
            {
                _IUnitOfWork.UserRepository.Add(data);
                _IUnitOfWork.Save();
            }catch(Exception ex) { }
            return (AuthenticateResultEnum.Ok, ""); 
        }
    }
}
