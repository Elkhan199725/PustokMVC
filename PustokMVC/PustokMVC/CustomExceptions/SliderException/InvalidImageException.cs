namespace PustokMVC.CustomExceptions.SliderException
{
    public class InvalidImageException : Exception
    {
        public InvalidImageException()
        {
        }

        public InvalidImageException(string? message) : base(message)
        {
        }
    }
}
