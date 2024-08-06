namespace MovieAppBackend.Custom.Exceptions
{
    public class DuplicateValuesException : Exception
    {
        public DuplicateValuesException() : base() { }
        public DuplicateValuesException(string message) : base(message) { }
        public DuplicateValuesException(string message, Exception innerException) : base(message, innerException) { }
    }
}
