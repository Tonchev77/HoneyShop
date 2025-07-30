namespace HoneyShop.GCommon
{
    public class ValidationConstants
    {

        public static class Product
        {
            public const int NameMaxLength = 80;
            public const int NameMinLength = 4;

            public const string NameMinLengthMessage = "Product name must be at least 4 characters long.";
            public const string NameMaxLengthMessage = "Product name must be at most 80 characters long.";

            public const int DescriptionMaxLength = 600;
            public const int DescriptionMinLength = 20;

            public const string DescriptionMinLengthMessage = "Product name must be at least 20 characters long.";
            public const string DescriptionMaxLengthMessage = "Product name must be at most 600 characters long.";

            public const int ImageUrlMaxLength = 300;

            public const string ImageUrlMaxLengthMessage = "ImageUrl must be at most 300 characters long.";
        }

        public static class Warehouse
        {
            public const int NameMaxLength = 60;
            public const int NameMinLength = 4;

            public const string NameMinLengthMessage = "Warehouse name must be at least 4 characters long.";
            public const string NameMaxLengthMessage = "Warehouse name must be at most 60 characters long.";

            public const int LocationMaxLength = 70;
            public const int LocationMinLength = 3;

            public const string LocationMinLengthMessage = "Warehouse name must be at least 3 characters long.";
            public const string LocationMaxLengthMessage = "Warehouse name must be at most 70 characters long.";
        }

        public static class Order
        {
            public const int ShippingCityMaxLength = 70;
            public const int ShippingCityMinLength = 4;

            public const string ShippingCityMinLengthMessage = "Shipping city must be at least 4 characters long.";
            public const string ShippingCityMaxLengthMessage = "Shipping city must be at most 70 characters long.";

            public const int ShippingAddressMaxLength = 70;
            public const int ShippingAddressMinLength = 4;

            public const string ShippingAddressMinLengthMessage = "Shipping address must be at least 4 characters long.";
            public const string ShippingAddressMaxLengthMessage = "Shipping address must be at most 70 characters long.";

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
