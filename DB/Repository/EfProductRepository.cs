using DB.Data;
using DB.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace DB.Repository
{
    /// <summary>
    /// Представляет репозиторий для сущности Product, используя Entity Framework Core.
    /// </summary>
    public class EfProductRepository
    {
        private readonly FoodTrackerContext _context;

        /// <summary>
        /// Создаёт новый экземпляр репозитория продуктов, используя готовый контекст.
        /// </summary>
        /// <param name="context">Контекст базы данных, используемый для работы с продуктами.</param>
        /// <exception cref="ArgumentNullException">Если контекст равен null.</exception>
        public EfProductRepository(FoodTrackerContext context)
        {
            ArgumentNullException.ThrowIfNull(context, nameof(context));

            _context = context;
        }

        /// <summary>
        /// Читает из базы данных и возвращает список всех продуктов.
        /// </summary>
        /// <returns>Список продуктов (атрибуты: название, категория, единица измерения).</returns>
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

        /// <summary>
        /// Добавляет продукт с указанным названием, существующей категорией и единицей измерения.
        /// </summary>
        /// <param name="productName">Название добавляемого продукта.</param>
        /// <param name="categoryName">Категория добавляемого продукта.</param>
        /// <param name="unitName">Единица измерения добавляемого продукта.</param>
        /// <returns>Количество затронутых строк.</returns>
        public async Task<int> CreateProductAsync(string productName, string categoryName, string unitName)
        {
            var category = await _context.Categories
                .SingleAsync(category => category.CategoryName == categoryName);
            var unit = await _context.Units
                .SingleAsync(unit => unit.UnitName == unitName);
            var product = new Product
            {
                ProductName = productName,
                Category = category,
                Unit = unit
            };

            _context.Products.Add(product);

            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Удаляет продукт по названию.
        /// </summary>
        /// <param name="productName">Название удаляемого продукта.</param>
        /// <returns>Количество затронутых строк.</returns>
        public async Task<int> DeleteProductAsync(string productName)
        {
            var products = await _context.Products
                .Where(product => product.ProductName == productName)
                .ToListAsync();

            _context.Products.RemoveRange(products);

            return await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Изменяет название продукта.
        /// </summary>
        /// <param name="oldProductName">Название изменяемого продукта.</param>
        /// <param name="newProductName">Новое название.</param>
        /// <returns>Количество затронутых строк.</returns>
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
