using AutoMapper;
using DTO;
using Library_System.Model;

namespace Library_System.AutoMapper
{
    public class AutoMapperConfiguration : Profile
    {
        public AutoMapperConfiguration()
        {
            ConfigureDTO();
        }
        private void ConfigureDTO()
        {

            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<UserDTO, RegisterInput>().ReverseMap();
            CreateMap<Library, LibraryDTO>().ReverseMap();
            CreateMap<Book, BookDTO>().ReverseMap();
         
        }

    }
}