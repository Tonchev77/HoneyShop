namespace HoneyShop.Services.Core
{
    using HoneyShop.ViewModels.Shop;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    public enum SortOption
    {
        Default,
        PriceAscending,
        PriceDescending,
        NameAscending,
        NameDescending,
        Newest
    }
    public class ProductFilterPaginationOptions
    {
        // Filter properties
        public string? SearchString { get; set; }
        public Guid? CategoryId { get; set; }
        public SortOption SortBy { get; set; } = SortOption.Default;

        // Pagination properties
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 9;
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling(TotalItems / (double)PageSize);

        public IEnumerable<T> ApplySortingAndPagination<T>(IEnumerable<T> items) where T : GetAllProductsViewModel
        {
            // Apply sorting
            var sortedItems = SortBy switch
            {
                SortOption.PriceAscending => items.OrderBy(p => p.Price),
                SortOption.PriceDescending => items.OrderByDescending(p => p.Price),
                SortOption.NameAscending => items.OrderBy(p => p.Name),
                SortOption.NameDescending => items.OrderByDescending(p => p.Name),
                SortOption.Newest => items.OrderByDescending(p => p.Id),
                _ => items // Default sorting (no change)
            };

            // Apply pagination
            return sortedItems.Skip((CurrentPage - 1) * PageSize).Take(PageSize);
        }
    }
}
