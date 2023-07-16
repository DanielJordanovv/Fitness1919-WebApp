namespace Fitness1919.Common
{
    public static class EntityValidationConstants
    {
        public static class Category
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 50;
        }
        public static class Contact
        {
            public const string PhoneNumberName = "Phone Number";
            public const string PhoneNumberExpression = @"\+359\d{9}";
            public const string EmailExpression = @"^[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+.[A-Za-z]{2,}$";
        }
        public static class Product
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 50;
            public const int DescriptionMaxLength = 100;
            public const string PriceMinValue = "0";
            public const string PriceMaxValue = "500";
            public const int QuantityMinRange = 0;
            public const int QuantityMaxRange = 100;
            public const int ImageUrlMaxLength = 2048;
        }
        public static class Brand
        {
            public const int NameMinLength = 2;
            public const int NameMaxLength = 50;
        }

    }
}
