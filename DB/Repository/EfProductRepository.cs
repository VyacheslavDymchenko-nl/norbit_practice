using DB.Data;
using DB.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace DB.Repository
{
    public class EfProductRepository
    {
        private readonly FoodTrackerContext _context;

        public EfProductRepository(FoodTrackerContext context)
        {
            _context = context;
        }

        public async Task<string> ReadProductsAsync()
        {
            var products = await _context.Products
                .AsNoTracking()
                .Select(product => new
                {
                    product.ProductName,

                    CategoryName = product.Category != null
                        ? product.Category.CategoryName
                        : null,

                    UnitName = product.Unit != null
                        ? product.Unit.UnitName
                        : null
                })
                .ToListAsync();
            var result = new StringBuilder();

            foreach (var product in products)
            {
                result.AppendLine(
                    $"""
                    Название: {product.ProductName} 
                    Категория: {product.CategoryName ?? "NULL"} 
                    Единица измерения: {product.UnitName ?? "NULL"}

                    """);
            }

            return result.ToString();
        }

        public async Task<int> CreateProductAsync(string productName, string categoryName, string unitName)
        {
            var category = await _context.Categories
                .SingleAsync(category => category.CategoryName == categoryName);
            var unit = await _context.Units
                .SingleAsync(unit => unit.UnitName == unitName);
            var product = new Product()
            {
                ProductName = productName,
                Category = category,
                Unit = unit
            };

            _context.Products.Add(product);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> DeleteProductAsync(string productName)
        {
            var products = await _context.Products
                .Where(product => product.ProductName == productName)
                .ToListAsync();

            _context.Products.RemoveRange(products);

            return await _context.SaveChangesAsync();
        }

        public async Task<int> UpdateProductAsync(string oldProductName, string newProductName)
        {
            var products = await _context.Products
                .Where(product => product.ProductName == oldProductName)
                .ToListAsync();

            foreach (var product in products)
            {
                product.ProductName = newProductName;
            }

            return await _context.SaveChangesAsync();
        }
    }
}
