namespace HomeLibrary.Models
{
    public class LibraryUser
    {
        public ApplicationUser ApplicationUser {get;set;}
        public string ApplicationUserId{get;set;}
        
        public Library Library {get;set;}
        public int LibraryId {get;set;}
    }
}