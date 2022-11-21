using DTO;
using DTO.Shared;
using Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_System.Services.BookService
{
    public interface IBookService
    {
        Task<DataTableReturn> Get_Book_List(PaginationInputs inputs);
        Task<List<LibraryDTO>> library_list();
        Task<(AuthenticateResultEnum Result, string? message)> add(BookDTO input);
        Task<BookDTO> getbook(Guid id);
        Task<(AuthenticateResultEnum Result, string? message)> editbook(BookDTO input);
    }
}
