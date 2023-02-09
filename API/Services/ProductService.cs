using API.Models;
using Microsoft.EntityFrameworkCore;
using WebAPI.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace API.Services
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllAsync();

        Task<Product> GetByIdAsync(int id);

        Task<Product> AddAsync(Product product);

        Task DeleteAsync(int id);

        Task<IEnumerable<Product>> GetProductsAsync(string name, decimal price);
    }

    public class ProductService : IProductService
    {
        private readonly AppDbContext appDbContext;

        public ProductService(AppDbContext appDbContext)
        {
            this.appDbContext = appDbContext;
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await appDbContext.Products.ToListAsync();
        }

        public async Task<Product> GetByIdAsync(int id)
        {
            return await appDbContext.Products.Where(o => o.Id == id).FirstAsync();
        }

        public async Task<Product> AddAsync(Product product)
        {
            // valid product

            await appDbContext.Products.AddAsync(product);
            await appDbContext.SaveChangesAsync();

            return product;
        }

        public async Task DeleteAsync(int id)
        {
            var product = await GetByIdAsync(id);

            if (product != null)
            {
                appDbContext.Products.Remove(product);
                await appDbContext.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Product>> GetProductsAsync(string name, decimal price)
        {
            var query = "SELECT * FROM [dbo].[Products] WHERE [Name] LIKE @Name AND [Price] >= @Price";

            using (SqlConnection conn = new SqlConnection(appDbContext.Database.GetConnectionString()))
            {
                return await conn.QueryAsync<Product>(query, new { Name = string.Format("%{0}%", name), Price = price });
            }
        }
    }
}
