namespace Fitness1919.Services.Data.Exceptions
{
    public class EmptyShoppingCartException : Exception
    {
        private const string NOT_FOUND_MESSAGE = "The cart was not found or it doesnt exist.";
        public EmptyShoppingCartException(string message)
            : base(message)
        {
        }
        public EmptyShoppingCartException()
            :base(NOT_FOUND_MESSAGE)
        {
        }
    }
}
