using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HomeLibrary.Infrastructure;
using HomeLibrary.Models;
using HomeLibrary.Models.BookViewModels;
using HomeLibrary.Models.LibraryViewModels;
using HomeLibrary.Repositories;
using HomeLibrary.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace HomeLibrary.Controllers
{
    public class LibraryController : Controller
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILibraryRepository _libraryRepository;
        private readonly IEmailSender _emailSender;
        private readonly InvitationTokenProvider _tokenProvider;

        public LibraryController(IMapper mapper, UserManager<ApplicationUser> userManager, ILibraryRepository libraryRepository, IEmailSender emailSender, InvitationTokenProvider tokenProvider)
        {
            _mapper = mapper;
            _userManager = userManager;
            _libraryRepository = libraryRepository;
            _emailSender = emailSender;
            _tokenProvider = tokenProvider;
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
                var libraryUser = _userManager.FindByIdAsync(user.ApplicationUserId).Result;

                users.Add(_mapper.Map<LibraryUserViewModel>(libraryUser));
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

        public IActionResult InviteUser()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> InviteUser(InviteUserViewModel viewModel)
        {
            if (ModelState.IsValid)
            {
                var invitation = new Invitation();

                invitation.Email = viewModel.Email;
                invitation.LibraryId = _libraryRepository.GetUserLibrary(_userManager.GetUserId(User)).Id;

                var code = _tokenProvider.Generate(viewModel.Email);

                var callbackUrl = Url.Action("ConfirmInvitation", "Library", new { email = viewModel.Email, code = code, libraryId = invitation.LibraryId }, protocol: HttpContext.Request.Scheme);

                await _emailSender.SendEmailAsync(viewModel.Email, "Home Library invitation link", $"{_userManager.GetUserName(User)} invited you to his home library please click link below" 
                 + "to confirm invitation : "+ callbackUrl);         

                _libraryRepository.AddInvitation(invitation);
                _libraryRepository.SaveChanges();

                return RedirectToAction(nameof(LibraryController.Index));
            }

            return View(viewModel);
        }

        public async Task<IActionResult> ConfirmInvitation(string code, string email, int? libraryId)
        {
            if(code == null || email == null || libraryId == null)
            {
                return View("Error");
            }

            var user = await _userManager.FindByEmailAsync(email);

            if(user == null)
            {
                return RedirectToAction("Account",nameof(AccountController.Register), new { email = email});
            }

            var result = _tokenProvider.Validate(code, email);

            if(result)
            {
                _libraryRepository.RemoveInvitation(libraryId.Value, email);
                _libraryRepository.AddUserToLibrary(libraryId.Value, email);
                _libraryRepository.SaveChanges();

                return RedirectToAction(nameof(LibraryController.Index));
            }

            return View("Error");
        }
    }
}
