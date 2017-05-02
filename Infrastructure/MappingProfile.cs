using AutoMapper;
using HomeLibrary.Models;
using HomeLibrary.Models.LibraryViewModels;

namespace HomeLibrary.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, UserViewModel>();
            CreateMap<Book, BookViewModel>();
            CreateMap<Library, LibraryViewModel>();
        }
    }
}