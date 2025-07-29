namespace HoneyShop.ViewModels.Shop
{
    using HoneyShop.Services.Core;
    using HoneyShop.ViewModels.Home;
    using System.Collections.Generic;
    public class ShopIndexViewModel
    {
        public ShopIndexViewModel()
        {
            FilterPaginationOptions = new ProductFilterPaginationOptions();
        }

        public IEnumerable<GetAllProductsViewModel> Products { get; set; } = new List<GetAllProductsViewModel>();
        public IEnumerable<GetAllCategoriesViewModel> Categories { get; set; } = new List<GetAllCategoriesViewModel>();

        // Filter and pagination options
        public ProductFilterPaginationOptions FilterPaginationOptions { get; set; }

        // Convenience properties for easy access in views
        public string? SearchString
        {
            get => FilterPaginationOptions.SearchString;
            set => FilterPaginationOptions.SearchString = value;
        }

        public Guid? CategoryId
        {
            get => FilterPaginationOptions.CategoryId;
            set => FilterPaginationOptions.CategoryId = value;
        }

        public SortOption SortBy
        {
            get => FilterPaginationOptions.SortBy;
            set => FilterPaginationOptions.SortBy = value;
        }

        public int CurrentPage
        {
            get => FilterPaginationOptions.CurrentPage;
            set => FilterPaginationOptions.CurrentPage = value;
        }

        public int PageSize
        {
            get => FilterPaginationOptions.PageSize;
            set => FilterPaginationOptions.PageSize = value;
        }

        public int TotalProducts
        {
            get => FilterPaginationOptions.TotalItems;
            set => FilterPaginationOptions.TotalItems = value;
        }

        public int TotalPages => FilterPaginationOptions.TotalPages;

        // Helper method to get sorted and paginated products
        public IEnumerable<GetAllProductsViewModel> PaginatedProducts =>
            FilterPaginationOptions.ApplySortingAndPagination(Products);
    }
}
