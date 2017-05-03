using AutoMapper;
using HomeLibrary.Models;
using HomeLibrary.Models.BookViewModels;
using HomeLibrary.Models.LibraryViewModels;

namespace HomeLibrary.Infrastructure
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, LibraryUserViewModel>();
            CreateMap<Book, ReadBookViewModel>();
            CreateMap<Library, LibraryViewModel>();
            CreateMap<CreateBookViewModel, Book>();
        }
    }
}