using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HomeLibrary.Models;
using HomeLibrary.Models.LibraryViewModels;
using HomeLibrary.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeLibrary.Controllers
{
    public class LibraryController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILibraryRepository _libraryRepository;

        public LibraryController(IMapper mapper, UserManager<ApplicationUser> userManager, ILibraryRepository libraryRepository)
        {
            _mapper = mapper;
            _userManager = userManager;
            _libraryRepository = libraryRepository;
        }


        public IActionResult Index()
        {
            var userLibrary = _libraryRepository.GetUserLibrary(_userManager.GetUserId(User));

            if(userLibrary == null)
                return View("Error");

            var libraryViewModel = new LibraryViewModel();

            var books = new List<BookViewModel>();
            var users = new List<UserViewModel>();

            foreach(var user in userLibrary.Users.ToList())
            {
                users.Add(_mapper.Map<UserViewModel>(user));
            }

             foreach(var book in userLibrary.Books.ToList())
            {
                books.Add(_mapper.Map<BookViewModel>(book));
            }

            libraryViewModel.Users = users;
            libraryViewModel.Books = books;

            return View(libraryViewModel);
        }

        public IActionResult NewBook()
        {
            return View();
        }

        [HttpPost]
        public IActionResult NewBook(NewBookViewModel viewModel)
        {
            return RedirectToAction(nameof(LibraryController.Index));
        }
    }
}
