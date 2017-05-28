using System;
using System.Linq;
using AutoMapper;
using HomeLibrary.Infrastructure;
using HomeLibrary.Infrastructure.Alerts;
using HomeLibrary.Models;
using HomeLibrary.Models.BookViewModels;
using HomeLibrary.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeLibrary.Controllers
{
    public class BookController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ICurrentUser _currentUser;
        private readonly ILibraryRepository _libraryRepository;

        public BookController(IMapper mapper, ICurrentUser currentUser, 
                                 ILibraryRepository libraryRepository)
        {
            _mapper = mapper;
            _currentUser = currentUser;
            _libraryRepository = libraryRepository;
        }

        public IActionResult GetBook(int bookId)
        {
            var userBooks = _libraryRepository.GetAllUserBooks(_currentUser.User.Id).ToList();
            var book = userBooks.Where(x=>x.Id == bookId).FirstOrDefault();

            if(book == null)
                return View("Error");            

            var viewModel = _mapper.Map<BookDetailsViewModel>(book);

            return View(viewModel);
        }
        
        public IActionResult CreateBook(int libraryId)
        {
            var userLibraries = _libraryRepository.GetAllUserLibraries(_currentUser.User.Id).ToList();
            var library = userLibraries.Where(x=>x.Id == libraryId).FirstOrDefault();
            
            if(library == null)
                return View("Error");

            CreateBookViewModel viewModel = new CreateBookViewModel()
            {
                LibraryId = libraryId,
                ApplicationUserId = _currentUser.User.Id
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult CreateBook(CreateBookViewModel viewModel)
        {
           var userLibraries = _libraryRepository.GetAllUserLibraries(_currentUser.User.Id).ToList();
           var library = userLibraries.Where(x=>x.Id == viewModel.LibraryId).FirstOrDefault();

            if(library == null || viewModel.ApplicationUserId != _currentUser.User.Id)
                return View("Error");               

            if (ModelState.IsValid)
            {
                var newBook = _mapper.Map<Book>(viewModel);

                library.Books.Add(newBook);
                
                _libraryRepository.SaveChanges();

                return RedirectToAction(nameof(LibraryController.GetLibrary),new {Id = viewModel.LibraryId}).WithSuccess("Successfull added new book.");
            }

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult LoanBookOutside(BookDetailsViewModel viewModel)
        {
            var userBooks = _libraryRepository.GetAllUserBooks(_currentUser.User.Id).ToList();
            var book = userBooks.Where(x=>x.Id == viewModel.Id).FirstOrDefault();

            if(book == null)
                return View("Error");                             

            _libraryRepository.LoanBookOutsideSystem(book);
            _libraryRepository.SaveChanges();

            return RedirectToAction(nameof(LibraryController.GetLibrary),new {libraryId = book.LibraryId}).WithSuccess("Successfull book loan");
        }
    }
}