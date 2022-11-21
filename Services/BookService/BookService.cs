using AutoMapper;
using DTO;
using DTO.Shared;
using Enum;
using Library_System.Model;
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
        public async Task<List<LibraryDTO>> library_list() 
        {
            try 
            {
                var data = _IUnitOfWork.LibraryRepository.AllAsIQueryable();
                return _mapper.Map<List<LibraryDTO>>(data); 
            }
            catch (Exception ex) { throw; }
        }

        public async Task<BookDTO> getbook(Guid id) 
        {
        if(id== null)
                throw new ArgumentNullException("id");

            try { 
            var data = _IUnitOfWork.BookRepository.GetById(id);
                return _mapper.Map<BookDTO>(data); 
            }catch(Exception ex) { throw; }
        }

        public async Task<(AuthenticateResultEnum Result, string? message)> editbook(BookDTO input)
        {
            if (input.Id == null)
                throw new ArgumentNullException("id");

            try
            {
                var data = _IUnitOfWork.BookRepository.GetById(input.Id);

                data.description = input.description; 
                data.Library_Id = input.Library_Id; 
                data.Book_Name = input.Book_Name;  
                data.Type= input.Type;  
                
                _IUnitOfWork.BookRepository.Update(data);
                _IUnitOfWork.Save();
                return (AuthenticateResultEnum.Ok, "Edited successfully");

            }
            catch (Exception ex) { throw; }
        }

        public async Task<(AuthenticateResultEnum Result, string? message)> add(BookDTO input) 
        {
            if (input == null) 
                throw new ArgumentNullException();

            if (!await _IUnitOfWork.LibraryRepository.Any(x => x.Id == input.Library_Id))
                return (AuthenticateResultEnum.ValidationError,  "library not exist"); 
            
            try { 
            var book =_mapper.Map<Book>(input);
                book.Is_Available_To_Reserve = true;    
             _IUnitOfWork.BookRepository.Add(book);
                _IUnitOfWork.Save();

                return (AuthenticateResultEnum.Ok, "added successfully"); 
            }
            catch(Exception ex) { throw; }       
        
        }
        public async Task<DataTableReturn> Get_Book_List(PaginationInputs inputs)
        {
            int skip = (inputs.index - 1) * inputs.size;
            int take = inputs.size;

            var data = _IUnitOfWork.BookRepository.AllAsIQueryable(null,new string[] { "library" } );

            

            var count = data.Count();
            var result = _mapper.Map<List<BookDTO>>(data.OrderByDescending(o => o.Id).Skip(skip).Take(take));

            return new DataTableReturn() { total = count, list = result, pagesCount = (int)Math.Ceiling((decimal)count / take) };
        }
    }
}
