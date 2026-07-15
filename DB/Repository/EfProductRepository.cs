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

        public string ReadProducts()
        {
            var products = _context.Products
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
                .ToList();

            var result = new StringBuilder();

            foreach (var product in products)
            {
                result.AppendLine(
                    $"Название: {product.ProductName} | " +
                    $"Категория: {product.CategoryName ?? "NULL"} | " +
                    $"Единица измерения: {product.UnitName ?? "NULL"}");
            }

            return result.ToString();
        }

        public int CreateProduct(string productName, string categoryName, string unitName)
        {
            var category = _context.Categories
                .Single(category => category.CategoryName == categoryName);
            var unit = _context.Units
                .Single(unit => unit.UnitName == unitName);
            var product = new Product()
            {
                ProductName = productName,
                Category = category,
                Unit = unit
            };

            _context.Products.Add(product);

            return _context.SaveChanges();
        }

        public int DeleteProduct(string productName)
        {
            var products = _context.Products
                .Where(product => product.ProductName == productName)
                .ToList();

            _context.Products.RemoveRange(products);

            return _context.SaveChanges();
        }

        public int UpdateProduct(string oldProductName, string newProductName)
        {
            var products = _context.Products
                .Where(product => product.ProductName == oldProductName)
                .ToList();

            foreach (var product in products)
            {
                product.ProductName = newProductName;
            }

            return _context.SaveChanges();
        }
    }
}
