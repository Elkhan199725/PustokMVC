namespace PustokMVC.CustomExceptions.BookExceptions
{
    public class BookInvalidCredentialException : Exception
    {
        public string PropertyName { get; set; }
        public BookInvalidCredentialException()
        {
        }

        public BookInvalidCredentialException(string? message) : base(message)
        {
        }

        public BookInvalidCredentialException(string? propertyName,string? message) : base(message)
        {
            PropertyName = propertyName;
        }
    }
}
