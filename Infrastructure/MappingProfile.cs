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
            CreateMap<ApplicationUser, LibraryUserDetailsViewModel>();
            CreateMap<Book, BookDetailsViewModel>();
            CreateMap<Library, LibraryDetailsViewModel>();
            CreateMap<CreateBookViewModel, Book>();
        }
    }
}