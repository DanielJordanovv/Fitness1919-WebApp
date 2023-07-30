namespace Fitness1919.Services.Data.Exceptions
{
    public class ProductNotFoundException : Exception
    {
        private const string NOT_FOUND_MESSAGE = "The product was not found or it doesnt exist.";
        public ProductNotFoundException(string message)
            : base(message)
        {
        }
        public ProductNotFoundException()
            : base(NOT_FOUND_MESSAGE)
        {
        }
    }
}
