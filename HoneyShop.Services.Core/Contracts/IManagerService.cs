namespace HoneyShop.Services.Core.Contracts
{
    public interface IManagerService
    {
        Task<Guid?> GetIdByUserIdAsync(string? userId);

        Task<bool> ExistsByIdAsync(string? id);

        Task<bool> ExistsByUserIdAsync(string? userId);
    }
}
