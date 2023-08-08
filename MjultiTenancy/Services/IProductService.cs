namespace MjultiTenancy.Services
{
    public interface IProductService
    {
        Task<Product> CreateAsync(Product product);
        Task<Product> GetById(int Id);
        Task<IReadOnlyList<Product>> GetAllAsync();
    }
}
