namespace HoneyShop.GCommon
{
    public class NotificationMessages
    {
        public static class Category
        {
            public const string CategoryAddedSuccessfully = "Category added successfully.";
            public const string CategoryEditedSuccessfully = "Category edited successfully.";
            public const string CategoryEditError = "Fatal error occurred while updating the category!";
            public const string CategoryDeletedSuccessfully = "Category deleted successfully.";
            public const string CategoryDeletedError = "Error occurred while deleting the category!";
            public const string CategoryNotFound = "Category not found.";
            public const string CategoryFatalError = "Fatal error occurred while processing the category!";

        }

        public static class Product
        {
            public const string ProductAddedSuccessfully = "Product added successfully.";
            public const string ProductEditedSuccessfully = "Product edited successfully.";
            public const string ProductEditError = "Fatal error occurred while updating the product!";
            public const string ProductDeletedSuccessfully = "Product deleted successfully.";
            public const string ProductDeletedError = "Error occurred while deleting the product!";
            public const string ProductNotFound = "Product not found.";
            public const string ProductFatalError = "Fatal error occurred while processing the product!";
        }
    }
}
