namespace Fitness1919.Services.Data.Exceptions
{
    public class NotFoundShoppingCartException : Exception
    {
        private const string NOT_FOUND_MESSAGE = "The cart was not found or it doesnt exist.";
        public NotFoundShoppingCartException(string message)
            : base(message)
        {
        }
        public NotFoundShoppingCartException()
            :base(NOT_FOUND_MESSAGE)
        {
        }
    }
}
