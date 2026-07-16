using DB.Data;
using DB.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DB.Program
{
    internal class Program
    {
        static async Task Main()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            string connectionString = config.GetConnectionString("DefaultConnection")!;
            var adoRepository = new AdoProductRepository(connectionString);
            DbContextOptions<FoodTrackerContext> options = new DbContextOptionsBuilder<FoodTrackerContext>()
                .UseSqlServer(connectionString)
                .Options;
            var efRepository = new EfProductRepository(new FoodTrackerContext(options));

            while (true)
            {
                Console.Write("""
                    1 — ADO.NET: показать продукты
                    2 — ADO.NET: добавить продукт
                    3 — ADO.NET: изменить продукт
                    4 — ADO.NET: удалить продукт
                    
                    5 — Entity Framework: показать продукты
                    6 — Entity Framework: добавить продукт
                    7 — Entity Framework: изменить продукт
                    8 — Entity Framework: удалить продукт
                    
                    0 — Выход

                    Выберите действие: 
                    """);

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine(await adoRepository.ReadProductsAsync());

                        break;

                    case "2":
                        Console.Write("Введите название продукта: ");
                        string productName = Console.ReadLine();
                        Console.Write("Введите название категории: ");
                        string categoryName = Console.ReadLine();
                        Console.Write("Введите название единицы измерения: ");
                        string unitName = Console.ReadLine();

                        Console.WriteLine($"Затронуто строк: {await adoRepository.CreateProductAsync(productName, categoryName, unitName)}");

                        break;

                    case "3":
                        Console.Write("Введите старое название продукта: ");
                        string oldProductName = Console.ReadLine();
                        Console.Write("Введите новое название продукта: ");
                        string newProductName = Console.ReadLine();

                        Console.WriteLine($"Затронуто строк: {await adoRepository.UpdateProductAsync(oldProductName, newProductName)}");

                        break;

                    case "4":
                        Console.Write("Введите название продукта: ");
                        productName = Console.ReadLine();

                        Console.WriteLine($"Затронуто строк: {await adoRepository.DeleteProductAsync(productName)}");

                        break;

                    case "5":

                        Console.WriteLine(await efRepository.ReadProductsAsync());

                        break;

                    case "6":
                        Console.Write("Введите название продукта: ");
                        productName = Console.ReadLine();
                        Console.Write("Введите название категории: ");
                        categoryName = Console.ReadLine();
                        Console.Write("Введите название единицы измерения: ");
                        unitName = Console.ReadLine();

                        Console.WriteLine($"Затронуто строк: {await efRepository.CreateProductAsync(productName, categoryName, unitName)}");

                        break;

                    case "7":
                        Console.Write("Введите старое название продукта: ");
                        oldProductName = Console.ReadLine();
                        Console.Write("Введите новое название продукта: ");
                        newProductName = Console.ReadLine();

                        Console.WriteLine($"Затронуто строк: {await efRepository.UpdateProductAsync(oldProductName, newProductName)}");

                        break;

                    case "8":
                        Console.Write("Введите название продукта: ");
                        productName = Console.ReadLine();

                        Console.WriteLine($"Затронуто строк: {await efRepository.DeleteProductAsync(productName)}");

                        break;

                    case "0":

                        return;

                    default:
                        Console.WriteLine("Неизвестная команда.");

                        break;
                }
            }
        }
    }
}
