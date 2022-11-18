using AutoMapper;
using DTO.Shared;
using Enum;
using Library_System.Model;
using Library_System.ReturnObject;
using Library_System.Services.BookService;
using Library_System.Services.UserService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library_System.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    [Authorize]

    public class BookController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;
        public BookController(IConfiguration configuration, IMapper mapper, IBookService bookService)
        {
            _mapper = mapper;
            _configuration = configuration;
            _bookService = bookService; 
          
        }
        [HttpGet]
        public async Task<IActionResult> GetBookList([FromQuery]PaginationInputs inputs)
        {
            try
            {
                var data = await _bookService.Get_Book_List(inputs);
                return Ok(new Respons_Result { isSuccess = true, responseBody = data });

            }
            catch (Exception ex)
            {
                return Ok(new Respons_Result { isSuccess = false, errorCode = ErrorCodesEnum.ServerError });

            }
        }

    }
}
