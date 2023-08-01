namespace Fitness1919.Common
{
    public static class EntityValidationConstants
    {
        public static class Category
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 20;
            public const string NameErrorMessage = "The category name must be between 2 and 20 letters long!";
        }
        public static class Contact
        {
            public const string PhoneNumberName = "Phone Number";
            public const string PhoneNumberExpression = @"\+359\d{9}";
            public const string PhoneNumberErrorMessage = "The Phone number should be in the following format: +359 000 000 000";

            public const string EmailExpression = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+.[A-Za-z]{2,}$";
            public const string EmailErrorMessage = "The email must be in the following format: xxxxx@xxx.xxx";
        }
        public static class Product
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 20;
            public const string NameErrorMessage = "The product name must be between 2 and 20 letters long!";

            public const int DescriptionMaxLength = 100;
            public const string DescriptionErrorMessage = "The description's max lenght is 100.";

            public const string PriceMinValue = "0";
            public const string PriceMaxValue = "500";
            public const string PriceColumnType = "decimal(18, 6)";
            public const string PriceErrorMessage = "The price must be between 0 and 500.";

            public const int QuantityMinRange = 0;
            public const int QuantityMaxRange = 100;
            public const string QuantityErrorMessage = "The quantity must be between 0 and 100.";

            public const int ImageUrlMaxLength = 2048;
        }
        public static class Brand
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 20;
            public const string NameErrorMessage = "The brand name must be between 2 and 20 letters long!";
        }
        public static class FeedBack
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 15;
            public const string NameErrorMessage = "The name must be between 2 and 15 letters long.";

            public const int CityMinLength = 2;
            public const int CityMaxLength = 20;
            public const string CityErrorMessage = "The city should be between 2 and 20 lettwes long.";

            public const int DescriptionMinLength = 2;
            public const string DescriptionErrorMessage = "The description should be at least 2 letters long.";
        }
        public static class ApplicationUser
        {
            public const int FirstNameMinLength = 1;
            public const int FirstNameMaxLength = 15;

            public const int LastNameMinLength = 1;
            public const int LastNameMaxLength = 15;

            public const int PasswordMinLength = 1;
            public const int PasswordMaxLength = 25;
        }
        public static class Order
        {
            public const string OrderPriceColumnType = "decimal(18, 6)";
            public const string OrderPriceMin = "0";
            public const string OrderPriceMax = "79228162514264337593543950335";

        }
    }
}