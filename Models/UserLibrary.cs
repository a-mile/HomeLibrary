namespace HomeLibrary.Models
{
    public class UserLibrary
    {
        public ApplicationUser ApplicationUser {get;set;}
        public string ApplicationUserId{get;set;}
        public Library Library {get;set;}
        public int LibraryId {get;set;}
    }
}