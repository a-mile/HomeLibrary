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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace HomeLibrary.Controllers
{
    public class LibraryController : Controller
    {
        private readonly IMapper _mapper;        
        private readonly ILibraryRepository _libraryRepository;
        private readonly IEmailSender _emailSender;
        private readonly InvitationTokenProvider _tokenProvider;
        private readonly ICurrentUser _currentUser;

        public LibraryController(IMapper mapper, ILibraryRepository libraryRepository, 
                                 IEmailSender emailSender, InvitationTokenProvider tokenProvider, ICurrentUser currentUser)
        {
            _mapper = mapper;            
            _libraryRepository = libraryRepository;
            _emailSender = emailSender;
            _tokenProvider = tokenProvider;
            _currentUser = currentUser;
        }     

        public IActionResult OwnLibrary()
        {
            var library = _libraryRepository.GetLibraryByOwnerId(_currentUser.User.Id);

            return RedirectToAction(nameof(LibraryController.GetLibrary), new {libraryId = library.Id});
        }   

        public IActionResult GetLibrary(int libraryId)
        {          
            var userLibraries = _libraryRepository.GetAllUserLibraries(_currentUser.User.Id).ToList();
            var library = userLibraries.Where(x=>x.Id == libraryId).FirstOrDefault();

            if(library == null)
                return View("Error");                       

            var libraryDetailsViewModel = _mapper.Map<LibraryDetailsViewModel>(library); 
            libraryDetailsViewModel.Owned = library.OwnerId == _currentUser.User.Id;           

            return View(libraryDetailsViewModel);
        }

        public IActionResult OtherLibraries()
        {
            var otherLibraries = _libraryRepository.GetOtherUserLibraries(_currentUser.User.Id);
            var otherLibrariesViewModels = _mapper.Map<IEnumerable<LibrarySummaryViewModel>>(otherLibraries);

            return View(otherLibrariesViewModels);
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
                var userLibrary =  _libraryRepository.GetLibraryByOwnerId(_currentUser.User.Id);

                var invitation = new Invitation()
                {
                    Email = viewModel.Email,
                    LibraryId = userLibrary.Id,
                    Date = DateTime.Now
                };

                var code = _tokenProvider.Generate(viewModel.Email);

                var callbackUrl = Url.Action("ConfirmInvitation", "Library", new { email = viewModel.Email, code = code, libraryId = invitation.LibraryId }, protocol: HttpContext.Request.Scheme);

                await _emailSender.SendEmailAsync(viewModel.Email, "Home Library invitation link", 
                    $"{_currentUser.User.Id} invited you to his home library. Please click link below to confirm invitation : " + 
                        callbackUrl);         

                userLibrary.Invitations.Add(invitation);
                _libraryRepository.SaveChanges();

                return RedirectToAction(nameof(LibraryController.GetLibrary)).WithSuccess("Invitation sended.");
            }

            return View(viewModel);
        }

        [AllowAnonymous]
        public IActionResult ConfirmInvitation(string code, string email, int? libraryId)
        {
            if(code == null || email == null || libraryId == null)
            {
                return View("Error");
            }      

            if(!User.Identity.IsAuthenticated)   
                return RedirectToAction("Account",nameof(AccountController.Register)).WithInfo("You have to sign in to account first.");   

            if(_currentUser.User.Email != email)            
                return RedirectToAction("Account",nameof(AccountController.Register)).WithInfo("You account is registered with other email than the one invitation sended to.");            

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

                        var libraryUser = new LibraryUser()
                        {
                            ApplicationUserId = _currentUser.User.Id,
                            LibraryId = library.Id
                        };

                        library.Users.Add(libraryUser);
                        _libraryRepository.SaveChanges();

                        return RedirectToAction(nameof(LibraryController.GetLibrary)).WithSuccess("Invitation confirmed.");
                    }
                }
            }

            return View("Error");
        }
    }
}
