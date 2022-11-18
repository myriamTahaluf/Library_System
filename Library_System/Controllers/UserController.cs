using AutoMapper;
using DTO;
using Enum;
using Library_System.ReturnObject;
using Library_System.Services;
using Library_System.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Library_System.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly Library_System.Services.AuthorizationService.IAuthorizationUserService _authorizationService;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;
        private readonly IMapper _mapper;
        public UserController( IConfiguration configuration ,IMapper mapper, IUserService userService , Library_System.Services.AuthorizationService.IAuthorizationUserService authorizationService)
        {
            _mapper = mapper;
            _configuration = configuration;
            _authorizationService = authorizationService;
            _userService = userService; 
        }
        public class LoginInput
        { 
            public string Email { get; set; }
            public string Password { get; set; }
        }

        [HttpGet]
        public async Task<ActionResult> Get_User_Details(Guid? id) 
        {
            if (!id.HasValue)
                return BadRequest(); 
            try 
            {
                var user = _userService.GetUserById(id.Value);
                return Ok(new Respons_Result { isSuccess = true, responseBody = user });


            }
            catch (Exception ex)
            {
                return Ok(new Respons_Result { isSuccess = false, errorCode= ErrorCodesEnum.ServerError});
            }
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterInput input)
        {
            if (input == null)
                return Ok(new Respons_Result() { isSuccess = false, statusCode = HttpStatusCode.BadRequest });

            try
            {
                UserDTO user = _mapper.Map<UserDTO>(input);
               

                (AuthenticateResultEnum Result, string? message)  result = await _userService.Insert(user);
                switch (result.Result)
                {
                    case AuthenticateResultEnum.ValidationError:
                        return Ok(new Respons_Result { isSuccess = false, errorCode = ErrorCodesEnum.validationError, responseBody = result.message });

                    case AuthenticateResultEnum.Ok:
                        return Ok(new Respons_Result { isSuccess = true,  responseBody = result.message });

                }

            }
            catch (Exception)
            {
                return Ok(new Respons_Result { isSuccess = false, errorCode =ErrorCodesEnum.ServerError });
            }
            return Ok(new Respons_Result { isSuccess = false, errorCode = ErrorCodesEnum.ServerError });

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginInput input )
        {
            if (string.IsNullOrEmpty(input.Password) || string.IsNullOrEmpty(input.Email))
                return BadRequest();

            (Enum.AuthenticateResultEnum Result, UserDTO User) result = await _authorizationService.Authenticate_User(input.Email, input.Password);
            switch (result.Result)
            {
                case AuthenticateResultEnum.AccountDisabled:
                    return Ok(new Respons_Result() { isSuccess = false, errorCode = ErrorCodesEnum.AccountDisabled });



                case AuthenticateResultEnum.wrongCredinal:
                    return Ok(new Respons_Result() { isSuccess = false, errorCode = ErrorCodesEnum.WrongCredentials });


                case AuthenticateResultEnum.Authenticated:
                    UserDTO authUser = result.User as UserDTO;
                    var tokenString = GenerateUserJWT(authUser);

                    var authCookieOptions = new CookieOptions()
                    {
                        HttpOnly = true,
                        SameSite = SameSiteMode.Strict
                    };


                    if (!string.IsNullOrEmpty(tokenString))
                    {

                        Response.Cookies.Append(".AUTHTOKEN", tokenString, authCookieOptions);

                        return Ok(new Respons_Result() { isSuccess = true, responseBody = tokenString, statusCode = HttpStatusCode.OK });
                    }
                    else
                    {
                        return Ok(new Respons_Result() { isSuccess = false, statusCode = HttpStatusCode.InternalServerError });

                    }
                default:
                    return Ok(new Respons_Result() { isSuccess = false, statusCode = HttpStatusCode.InternalServerError });


            }
        }

        [NonAction]

        private string GenerateUserJWT(UserDTO user)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim> {
                new Claim("ID", user.Id.ToString()),
                new Claim("Username", user.Email.ToString())  };
    
          

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Audience"],
                claims,
                expires: DateTime.UtcNow.AddDays(30),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);

           
        }



    }
}
