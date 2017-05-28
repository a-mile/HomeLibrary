using System.Collections.Generic;
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
            CreateMap<LibraryUser, LibraryUserDetailsViewModel>()
                .ConstructProjectionUsing(x=> Mapper.Map<LibraryUserDetailsViewModel>(x.ApplicationUser));
            CreateMap<Book, BookDetailsViewModel>()
                .ForMember(dest => dest.AddedBy, opt => opt.MapFrom(src => src.ApplicationUser.UserName));
            CreateMap<Library, LibraryDetailsViewModel>();
            CreateMap<Library, LibrarySummaryViewModel>()
                .ForMember(dest => dest.Owner, opt => opt.MapFrom(src => src.Owner.UserName))
                .ForMember(dest => dest.BooksCount, opt => opt.MapFrom(src => src.Books.Count))
                .ForMember(dest => dest.UsersCount, opt => opt.MapFrom(src => src.Users.Count + 1));
            CreateMap<CreateBookViewModel, Book>();
            CreateMap<Library, LibraryDetailsViewModel>();
        }
    }
}