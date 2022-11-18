using AutoMapper;
using DTO;
using DTO.Shared;
using Library_System.Repositories.Generic.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library_System.Services.BookService
{
    public class BookService : IBookService
    {
        private readonly IMapper _mapper;

        private readonly IUnitOfWork _IUnitOfWork;
        public BookService(IUnitOfWork IUnitOfWork, IMapper mapper)
        {
            _mapper = mapper;
            _IUnitOfWork = IUnitOfWork;
        }

        public async Task<DataTableReturn> Get_Book_List(PaginationInputs inputs)
        {
            int skip = (inputs.index - 1) * inputs.size;
            int take = inputs.size;

            var data = _IUnitOfWork.BookRepository.AllAsIQueryable();

            

            var count = data.Count();
            var result = _mapper.Map<List<BookDTO>>(data.OrderByDescending(o => o.Id).Skip(skip).Take(take));

            return new DataTableReturn() { total = count, list = result, pagesCount = (int)Math.Ceiling((decimal)count / take) };
        }
    }
}
