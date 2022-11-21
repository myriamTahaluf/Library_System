using AutoMapper;
using DTO;
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
        public async Task<IActionResult> GetLibList() 
        {
            try
            {
                var data = await _bookService.library_list();
                return Ok(new Respons_Result { isSuccess = true, responseBody = data });

            }
            catch (Exception ex)
            {
                return Ok(new Respons_Result { isSuccess = false, errorCode = ErrorCodesEnum.ServerError });

            }
            }
        [HttpGet]
        public async Task<IActionResult> getBook(Guid id) 
        {
      
            if(id==null)
                return Ok(new Respons_Result { isSuccess = false, errorCode = ErrorCodesEnum.BadInput });
            try
            {
                var result = await _bookService.getbook(id);

                return Ok(new Respons_Result { isSuccess = true, responseBody = result });


            }
            catch (Exception ex)
            {
                return Ok(new Respons_Result { isSuccess = false, errorCode = ErrorCodesEnum.ServerError });
            }

        }
        [HttpPost]
        public async Task<IActionResult> Add_Book(BookInput input) 
        {
            if (input == null)
                return Ok(new Respons_Result { isSuccess = false, errorCode = ErrorCodesEnum.BadInput });

            try
            {
                var data = _mapper.Map<BookDTO>(input);
                var result = await _bookService.add(data);

                switch (result.Result)
                {
                    case AuthenticateResultEnum.ValidationError:
                        return Ok(new Respons_Result { isSuccess = false, responseBody = result.message });
                        ;
                    case AuthenticateResultEnum.Ok:
                        return Ok(new Respons_Result { isSuccess = true, responseBody = result.message });


                }

            }
            catch (Exception ex) 
            {
                return Ok(new Respons_Result { isSuccess = false, errorCode = ErrorCodesEnum.ServerError });

            }
            return Ok(new Respons_Result { isSuccess = false, errorCode = ErrorCodesEnum.ServerError });


        }

        [HttpPost]
        public async Task<IActionResult> Edit_Book(BookEditInput input)
        {
            if (input == null)
                return Ok(new Respons_Result { isSuccess = false, errorCode = ErrorCodesEnum.BadInput });

            try
            {
                var data = _mapper.Map<BookDTO>(input);
                var result = await _bookService.editbook(data);

                switch (result.Result)
                {
                    case AuthenticateResultEnum.ValidationError:
                        return Ok(new Respons_Result { isSuccess = false, responseBody = result.message });
                        ;
                    case AuthenticateResultEnum.Ok:
                        return Ok(new Respons_Result { isSuccess = true, responseBody = result.message });


                }

            }
            catch (Exception ex)
            {
                return Ok(new Respons_Result { isSuccess = false, errorCode = ErrorCodesEnum.ServerError });

            }
            return Ok(new Respons_Result { isSuccess = false, errorCode = ErrorCodesEnum.ServerError });


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
