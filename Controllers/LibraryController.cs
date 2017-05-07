using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HomeLibrary.Infrastructure;
using HomeLibrary.Infrastructure.Alerts;
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

        public LibraryController(IMapper mapper, UserManager<ApplicationUser> userManager, 
                                 ILibraryRepository libraryRepository, IEmailSender emailSender, 
                                 InvitationTokenProvider tokenProvider)
        {
            _mapper = mapper;
            _userManager = userManager;
            _libraryRepository = libraryRepository;
            _emailSender = emailSender;
            _tokenProvider = tokenProvider;
        }

        public IActionResult GetLibrary(int? libraryId)
        {
            var userId = _userManager.GetUserId(User);

            Library library;

            if(libraryId == null)
            {
                library = _libraryRepository.GetLibraryByOwnerId(userId);
                ViewData["Title"] = "My library";
            }
            else
            {
                library = _libraryRepository.GetLibraryById(libraryId.Value);

                if(library == null || !library.Users.Where(x=>x.ApplicationUserId == userId).Any())
                    return View("Error");

                ViewData["Title"] = library.Owner.UserName + " library";
            }

            if(library == null)
                return View("Error");

            var libraryViewModel = new LibraryDetailsViewModel();

            var books = new List<BookDetailsViewModel>();
            var users = new List<LibraryUserDetailsViewModel>();

            foreach(var user in library.Users.ToList())
            {
                var libraryUser = _userManager.FindByIdAsync(user.ApplicationUserId).Result;

                if(libraryUser != null)
                    users.Add(_mapper.Map<LibraryUserDetailsViewModel>(libraryUser));
            }

             foreach(var book in library.Books.ToList())
            {
                var bookViewModel = _mapper.Map<BookDetailsViewModel>(book);
                bookViewModel.AddedBy = book.ApplicationUser.UserName;

                books.Add(bookViewModel);
            }

            libraryViewModel.Users = users;
            libraryViewModel.Books = books;
            libraryViewModel.LibraryId = library.Id;
            libraryViewModel.Owned = userId == library.OwnerId;

            return View(libraryViewModel);
        }

        public IActionResult OtherLibraries()
        {
            var otherLibraries = _libraryRepository.GetOtherUserLibraries(_userManager.GetUserId(User));

            var libraryViewModels = new List<LibrarySummaryViewModel>();

            foreach(var library in otherLibraries)
            {
                libraryViewModels.Add(new LibrarySummaryViewModel()
                {
                    LibraryId = library.Id,
                    Owner = library.Owner.UserName,
                    BooksCount = library.Books.Count(),
                    UsersCount = library.Users.Count() + 1 
                });
            }

            return View(libraryViewModels);
        }

        public IActionResult CreateBook(int libraryId)
        {
            CreateBookViewModel viewModel = new CreateBookViewModel()
            {
                LibraryId = libraryId
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult CreateBook(CreateBookViewModel viewModel)
        {
            var userId = _userManager.GetUserId(User);
            var library = _libraryRepository.GetLibraryById(viewModel.LibraryId);

            if(library == null)
                return View("Error");    

            if(library.OwnerId != userId && !library.Users.Where(x=>x.ApplicationUserId == userId).Any())
                return View("Error"); 

            if (ModelState.IsValid)
            {
                var newBook = _mapper.Map<Book>(viewModel);

                newBook.ApplicationUserId = userId;
                newBook.LibraryId = library.Id;

                library.Books.Add(newBook);
                _libraryRepository.SaveChanges();

                return RedirectToAction(nameof(LibraryController.GetLibrary),new {libraryId = viewModel.LibraryId}).WithSuccess("Successfull added new book.");
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
                var userLibrary =  _libraryRepository.GetLibraryByOwnerId(_userManager.GetUserId(User));

                var invitation = new Invitation()
                {
                    Email = viewModel.Email,
                    LibraryId = userLibrary.Id,
                    Date = DateTime.Now
                };

                var code = _tokenProvider.Generate(viewModel.Email);

                var callbackUrl = Url.Action("ConfirmInvitation", "Library", new { email = viewModel.Email, code = code, libraryId = invitation.LibraryId }, protocol: HttpContext.Request.Scheme);

                await _emailSender.SendEmailAsync(viewModel.Email, "Home Library invitation link", $"{_userManager.GetUserName(User)} invited you to his home library please click link below" 
                 + "to confirm invitation : "+ callbackUrl);         

                userLibrary.Invitations.Add(invitation);

                _libraryRepository.SaveChanges();

                return RedirectToAction(nameof(LibraryController.GetLibrary)).WithSuccess("Invitation sended.");
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
                return RedirectToAction("Account",nameof(AccountController.Register)).WithInfo("You have to register home library account first.");
            }

            var result = _tokenProvider.Validate(code, email);

            if(result)
            {
                var library = _libraryRepository.GetLibraryById(libraryId.Value);

                if(library != null)
                {
                    var invitation = library.Invitations.Where(x=>x.Email == email).FirstOrDefault();

                    if(invitation != null)
                    {
                        library.Invitations.Remove(invitation);

                        var userLibrary = new UserLibrary()
                        {
                            ApplicationUserId = user.Id,
                            LibraryId = library.Id
                        };

                        library.Users.Add(userLibrary);

                        _libraryRepository.SaveChanges();

                        return RedirectToAction(nameof(LibraryController.GetLibrary)).WithSuccess("Invitation confirmed.");
                    }
                }
            }

            return View("Error");
        }
    }
}
