namespace Fitness1919.Web.Controllers.Exceptions
{
    public class ProductNotFoundOrZeroException : Exception
    {
        private const string NOT_FOUND_MESSAGE = "The cart was not found or it doesnt exist.";
        public ProductNotFoundOrZeroException(string message)
            : base(message)
        {
        }
        public ProductNotFoundOrZeroException()
            : base(NOT_FOUND_MESSAGE)
        {
        }
    }
}
