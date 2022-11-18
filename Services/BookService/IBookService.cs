using DTO.Shared;
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
    }
}
