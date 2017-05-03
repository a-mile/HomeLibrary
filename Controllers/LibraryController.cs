using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HomeLibrary.Models;
using HomeLibrary.Models.BookViewModels;
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

            var books = new List<ReadBookViewModel>();
            var users = new List<LibraryUserViewModel>();

            foreach(var user in userLibrary.Users.ToList())
            {
                users.Add(_mapper.Map<LibraryUserViewModel>(user));
            }

             foreach(var book in userLibrary.Books.ToList())
            {
                var bookViewModel = _mapper.Map<ReadBookViewModel>(book);
                bookViewModel.AddedBy = book.ApplicationUser.UserName;
                books.Add(bookViewModel);
            }

            libraryViewModel.Users = users;
            libraryViewModel.Books = books;

            return View(libraryViewModel);
        }

        public IActionResult CreateBook()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateBook(CreateBookViewModel viewModel)
        {
            var userLibrary = _libraryRepository.GetUserLibrary(_userManager.GetUserId(User));

            if (ModelState.IsValid)
            {
                var newBook = _mapper.Map<Book>(viewModel);
                newBook.ApplicationUserId = _userManager.GetUserId(User);
                newBook.LibraryId = userLibrary.Id;

                userLibrary.Books.Add(newBook);
                _libraryRepository.SaveChanges();

                return RedirectToAction(nameof(LibraryController.Index));
            }

            return View(viewModel);
        }
    }
}
