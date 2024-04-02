namespace PustokMVC.CustomExceptions.SliderException
{
    public class SliderTitleAlreadyExistException : Exception
    {
        public string PropertyName { get; }
        public string PropertyValue { get; }

        public SliderTitleAlreadyExistException(string propertyName, string message) : base(message)
        {
            PropertyName = propertyName;
            PropertyValue = message;
        }
    }
}
