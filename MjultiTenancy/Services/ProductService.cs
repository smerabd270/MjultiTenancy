using Microsoft.EntityFrameworkCore;

namespace MjultiTenancy.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;
        public ProductService( ApplicationDbContext context)
        {
            _context = context;
            
        }
        public async Task<Product> CreateAsync(Product product)
        {
            _context.Products.Add(product);
           await _context.SaveChangesAsync();
            return product; 
        }

        public async Task<IReadOnlyList<Product>> GetAllAsync()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetById(int Id)
        {
            return await _context.Products.FirstAsync(x => x.Id == Id); 
        }
    }
}
