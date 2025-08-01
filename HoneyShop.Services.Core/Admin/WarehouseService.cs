﻿namespace HoneyShop.Services.Core.Admin
{
    using HoneyShop.Data.Models;
    using HoneyShop.Data.Repository.Interfaces;
    using HoneyShop.Services.Core.Admin.Contracts;
    using HoneyShop.ViewModels.Admin.ProductManagment;
    using HoneyShop.ViewModels.Admin.WarehouseManagment;
    using Microsoft.EntityFrameworkCore;
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class WarehouseService : IWarehouseService
    {
        private readonly IWarehouseRepository warehouseRepository;
        private readonly IProductStockRepository productStockRepository;
        private readonly IProductRepository productRepository;

        public WarehouseService(IWarehouseRepository warehouseRepository, IProductStockRepository productStockRepository, IProductRepository productRepository)
        {
            this.warehouseRepository = warehouseRepository;
            this.productStockRepository = productStockRepository;
            this.productRepository = productRepository;
        }

        public async Task<bool> AddProductToWarehouseAsync(AddProductToWarehouseViewModel inputModel)
        {
            bool opResult = false;

            Product? productRef = await this.productRepository
                .FirstOrDefaultAsync(c => c.Id == inputModel.ProductId);

            if (productRef != null)
            {
                ProductStock? stock = await this.productStockRepository
                    .FirstOrDefaultAsync(ps => ps.WarehouseId == inputModel.WarehouseId
                                          && ps.ProductId == inputModel.ProductId
                                          && !ps.IsDeleted);

                if (stock != null)
                {
                    stock.Quantity += inputModel.Quantity;
                    this.productStockRepository.Update(stock);
                }
                else
                {
                    ProductStock newProductStock = new ProductStock()
                    {

                        WarehouseId = inputModel.WarehouseId,
                        ProductId = inputModel.ProductId,
                        Quantity = inputModel.Quantity,
                        IsDeleted = false
                    };

                    await this.productStockRepository.AddAsync(newProductStock);
                }


                await this.productStockRepository.SaveChangesAsync();
                opResult = true;
            }

            return opResult;
        }

        public async Task<bool> AddWarehouseAsync(AddWarehouseViewModel inputModel)
        {
            bool opResult = false;

            if (inputModel != null)
            {
                Warehouse newWarehouse = new Warehouse()
                {
                    Name = inputModel.Name,
                    Location = inputModel.Location,
                };

                await this.warehouseRepository.AddAsync(newWarehouse);
                await this.warehouseRepository.SaveChangesAsync();

                opResult = true;
            }

            return opResult;
        }

        public async Task<IEnumerable<WarehouseManagmentIndexViewModel>> GetAllWarehousesAsync()
        {
            List<Warehouse> warehouses = await this.warehouseRepository
            .GetAllAttached()
            .ToListAsync();

            IEnumerable<WarehouseManagmentIndexViewModel> allWarehouses = warehouses
                .Select(p => new WarehouseManagmentIndexViewModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Location = p.Location
                })
                .ToList();

            return allWarehouses;
        }

        public async Task<DeleteProductFromWarehouseViewModel?> GetProductFromWarehouseForDeleteAsync(Guid? warehouseId, Guid productId)
        {
            DeleteProductFromWarehouseViewModel? deleteModel = null;

            if (warehouseId != null)
            {
                var productStockWithDetails = await this.productStockRepository
                    .GetAllAttached()
                    .Where(ps => ps.WarehouseId == warehouseId
                          && ps.ProductId == productId
                          && !ps.IsDeleted)
                    .Join(this.productRepository.GetAllAttached()
                        .Where(p => !p.IsDeleted),
                        ps => ps.ProductId,
                        p => p.Id,
                        (ps, p) => new
                        {
                            ps.WarehouseId,
                            ps.ProductId,
                            p.Name,
                            p.Description,
                            ps.Quantity,
                            p.ImageUrl,
                            p.Price
                        })
                    .FirstOrDefaultAsync();

                if (productStockWithDetails != null)
                {
                    deleteModel = new DeleteProductFromWarehouseViewModel()
                    {
                        WarehouseId = productStockWithDetails.WarehouseId,
                        ProductId = productStockWithDetails.ProductId,
                        ProductName = productStockWithDetails.Name,
                        ProductDescription = productStockWithDetails.Description,
                        Quantity = productStockWithDetails.Quantity,
                        ImageUrl = productStockWithDetails.ImageUrl,
                        Price = productStockWithDetails.Price
                    };
                }
            }

            return deleteModel;
        }

        public async Task<EditProductFromWarehouseViewModel?> GetProductFromWarehouseForEditingAsync(Guid? warehouseId, Guid? productId)
        {
            EditProductFromWarehouseViewModel? editModel = null;

            if (warehouseId != null && productId != null)
            {

                ProductStock? productStock = await this.productStockRepository
                    .FirstOrDefaultAsync(ps => ps.WarehouseId == warehouseId
                                        && ps.ProductId == productId
                                        && !ps.IsDeleted);

                if (productStock != null)
                {

                    List<ProductManagmentIndexViewModel> allProducts = await this.productRepository
                        .GetAllAttached()
                        .Where(p => !p.IsDeleted)
                        .Select(p => new ProductManagmentIndexViewModel
                        {
                            Id = p.Id,
                            Name = p.Name,
                            Description = p.Description,
                            Price = p.Price,
                            IsActive = p.IsActive
                        })
                        .ToListAsync();


                    editModel = new EditProductFromWarehouseViewModel()
                    {
                        WarehouseId = productStock.WarehouseId,
                        ProductId = productStock.ProductId,
                        Quantity = productStock.Quantity,
                        Products = allProducts
                    };
                }
            }

            return editModel;
        }

        public async Task<IEnumerable<GetProductsInWarehouseViewModel>> GetProductsInWarehouseAsync(Guid warehouseId)
        {
            IEnumerable<GetProductsInWarehouseViewModel> result = await this.productStockRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .Where(ps => ps.WarehouseId == warehouseId)
                .Join(this.productRepository.GetAllAttached()
                        .IgnoreQueryFilters(),
                        ps => ps.ProductId,
                        p => p.Id,
                        (ps, p) => new GetProductsInWarehouseViewModel
                        {
                            WarehouseId = ps.WarehouseId,
                            ProductId = p.Id,
                            ProductName = p.Name,
                            ProductDescription = p.Description,
                            Quantity = ps.Quantity,
                            ImageUrl = p.ImageUrl,
                            Price = p.Price,
                            IsDeleted = ps.IsDeleted
                        })
                        .ToListAsync();

            return result;
        }

        public async Task<DeleteWarehouseManagmentViewModel?> GetWarehouseForDeleteAsync(Guid? warehouseId)
        {
            DeleteWarehouseManagmentViewModel? deleteModel = null;

            if (warehouseId != null)
            {
                Warehouse? deleteWarehouseModel = await this.warehouseRepository
                    .SingleOrDefaultAsync(c => c.Id == warehouseId);

                if (deleteWarehouseModel != null)
                {
                    deleteModel = new DeleteWarehouseManagmentViewModel()
                    {
                        Id = deleteWarehouseModel.Id,
                        Name = deleteWarehouseModel.Name,
                        Location = deleteWarehouseModel.Location
                    };
                }
            }

            return deleteModel;
        }

        public async Task<EditWarehouseManagmentViewModel?> GetWarehouseForEditingAsync(Guid? warehouseId)
        {
            EditWarehouseManagmentViewModel? editModel = null;

            if (warehouseId != null)
            {
                Warehouse? editWarehouseModel = await this.warehouseRepository
                    .GetAllAttached()
                    .SingleOrDefaultAsync(c => c.Id == warehouseId);

                if (editWarehouseModel != null)
                {
                    editModel = new EditWarehouseManagmentViewModel()
                    {
                        Id = editWarehouseModel.Id,
                        Name = editWarehouseModel.Name,
                        Location = editWarehouseModel.Location
                    };
                }
            }

            return editModel;
        }

        public async Task<bool> PersistUpdateProductFromWarehouseAsync(EditProductFromWarehouseViewModel inputModel)
        {
            bool opResult = false;

            if (inputModel != null)
            {
                ProductStock? productStock = await this.productStockRepository
                    .FirstOrDefaultAsync(ps => ps.WarehouseId == inputModel.WarehouseId
                                          && ps.ProductId == inputModel.ProductId
                                          && !ps.IsDeleted);

                if (productStock != null)
                {

                    productStock.Quantity = inputModel.Quantity;

                    this.productStockRepository.Update(productStock);

                    await this.productStockRepository.SaveChangesAsync();
                    opResult = true;
                }
            }

            return opResult;
        }

        public async Task<bool> PersistUpdateWarehouseAsync(EditWarehouseManagmentViewModel inputModel)
        {
            bool opResult = false;

            Warehouse? updatedWarehouse = await this.warehouseRepository
                .FirstOrDefaultAsync(c => c.Id == inputModel.Id);


            if (updatedWarehouse != null)
            {
                updatedWarehouse.Id = inputModel.Id;
                updatedWarehouse.Name = inputModel.Name;
                updatedWarehouse.Location = inputModel.Location;


                await this.warehouseRepository.SaveChangesAsync();

                opResult = true;
            }

            return opResult;
        }

        public async Task<bool> RecoverProductFromWarehouseAsync(Guid warehouseId, Guid productId)
        {
            bool opResult = false;

            ProductStock? productStock = await this.productStockRepository
                .GetAllAttached()
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(ps => ps.WarehouseId == warehouseId
                                    && ps.ProductId == productId
                                    && ps.IsDeleted);

            if (productStock != null)
            {

                productStock.IsDeleted = false;

                this.productStockRepository.Update(productStock);

                await this.productStockRepository.SaveChangesAsync();
                opResult = true;
            }

            return opResult;
        }

        public async Task<bool> SoftDeleteProductFromWarehouseAsync(DeleteProductFromWarehouseViewModel inputModel)
        {
            bool opResult = false;

            if (inputModel != null)
            {
                ProductStock? productStock = await this.productStockRepository
                    .FirstOrDefaultAsync(ps => ps.WarehouseId == inputModel.WarehouseId
                                        && ps.ProductId == inputModel.ProductId
                                        && !ps.IsDeleted);

                if (productStock != null)
                {
                    productStock.IsDeleted = true;

                    this.productStockRepository.Update(productStock);

                    await this.productStockRepository.SaveChangesAsync();
                    opResult = true;
                }
            }

            return opResult;
        }

        public async Task<bool> SoftDeleteWarehouseAsync(DeleteWarehouseManagmentViewModel inputModel)
        {
            bool opResult = false;

            Warehouse? deletedWarehouse = await this.warehouseRepository
                .FirstOrDefaultAsync(c => c.Id == inputModel.Id);

            if (deletedWarehouse != null)
            {
                deletedWarehouse.IsDeleted = true;

                await this.warehouseRepository.SaveChangesAsync();

                opResult = true;
            }

            return opResult;
        }
    }
}
