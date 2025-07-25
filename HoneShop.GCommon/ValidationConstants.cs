namespace HoneyShop.GCommon
{
    public class ValidationConstants
    {

        public static class Product
        {
            public const int NameMaxLength = 80;
            public const int NameMinLength = 4;

            public const int DescriptionMaxLength = 600;
            public const int DescriptionMinLength = 20;

            public const int ImageUrlMaxLength = 300;
        }

        public static class Warehouse
        {
            public const int NameMaxLength = 60;
            public const int NameMinLength = 4;

            public const int LocationMaxLength = 70;
            public const int LocationMinLength = 3;
        }

        public static class Order
        {
            public const int ShippingCityMaxLength = 70;
            public const int ShippingCityMinLength = 4;

            public const int ShippingAddressMaxLength = 70;
            public const int ShippingAddressMinLength = 4;

        }

        public static class Promotion
        {
            public const int NameMaxLength = 80;
            public const int NameMinLength = 4;

            public const int DescriptionMaxLength = 600;
            public const int DescriptionMinLength = 20;

        }

        public static class OrderStatus
        {
            public const int NameMaxLength = 20;
            public const int NameMinLength = 4;

            public const int DescriptionMaxLength = 200;

        }

        public static class Category
        {
            public const int NameMaxLength = 30;
            public const int NameMinLength = 4;

            public const string NameMinLengthMessage = "Category name must be at least 4 characters long.";
            public const string NameMaxLengthMessage = "Category name must be at most 30 characters long.";

            public const int DescriptionMaxLength = 200;

            public const string DescriptionMaxLengthMessage = "Category description must be at most 200 characters long.";

        }
    }
}
