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

        public static class Warehouse
        {
            public const string WarehouseAddedSuccessfully = "Warehouse added successfully.";
            public const string WarehouseEditedSuccessfully = "Warehouse edited successfully.";
            public const string WarehouseEditError = "Fatal error occurred while updating the warehouse!";
            public const string WarehouseDeletedSuccessfully = "Warehouse deleted successfully.";
            public const string WarehouseDeletedError = "Error occurred while deleting the warehouse!";
            public const string WarehouseNotFound = "Warehouse not found.";
            public const string WarehouseFatalError = "Fatal error occurred while processing the warehouse!";
        }

        public static class ProductStock 
        {
            public const string ProductStockAddedSuccessfully = "Product added to warehouse successfully.";
            public const string ProductStockEditedSuccessfully = "Product from warehouse edited successfully.";
            public const string ProductStockEditError = "Fatal error occurred while updating the product in warehouse!";
            public const string ProductStockDeletedSuccessfully = "Product in warehouse deleted successfully.";
            public const string ProductStockDeletedError = "Error occurred while deleting the product in warehouse!";
            public const string ProductStockNotFound = "Product in warehouse not found.";
            public const string ProductStockFatalError = "Fatal error occurred while processing the product in warehouse!";
            public const string ProductStockRecoverError = "Failed to recover product in warehouse.";
            public const string ProductStockRecoverSuccessfully = "Product in warehouse recovered successfully.";
        }
    }
}
