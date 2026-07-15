using DB.Data;
using DB.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DB
{
    internal class Program
    {
        static void Main()
        {
            IConfiguration config = new ConfigurationBuilder()
                .SetBasePath(AppContext.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();
            string connectionString = config.GetConnectionString("DefaultConnection")!;

            var adoRepository = new AdoProductRepository(connectionString);
            var options = new DbContextOptionsBuilder<FoodTrackerContext>()
                .UseSqlServer(connectionString)
                .Options;
            var efRepository = new EfProductRepository(new FoodTrackerContext(options));

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("1 — ADO.NET: показать продукты");
                Console.WriteLine("2 — ADO.NET: добавить продукт");
                Console.WriteLine("3 — ADO.NET: изменить продукт");
                Console.WriteLine("4 — ADO.NET: удалить продукт");
                Console.WriteLine();
                Console.WriteLine("5 — Entity Framework: показать продукты");
                Console.WriteLine("6 — Entity Framework: добавить продукт");
                Console.WriteLine("7 — Entity Framework: изменить продукт");
                Console.WriteLine("8 — Entity Framework: удалить продукт");
                Console.WriteLine();
                Console.WriteLine("0 — Выход");
                Console.Write("Выберите действие: ");

                string? choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.WriteLine(adoRepository.ReadProducts());
                        break;

                    case "2":
                        Console.Write("Введите название продукта: ");
                        string productName = Console.ReadLine();
                        Console.Write("Введите название категории: ");
                        string categoryName = Console.ReadLine();
                        Console.Write("Введите название единицы измерения: ");
                        string unitName = Console.ReadLine();

                        Console.WriteLine("Затронуто строк: " + adoRepository.CreateProduct(productName, categoryName, unitName));
                        break;

                    case "3":
                        Console.Write("Введите старое название продукта: ");
                        string oldProductName = Console.ReadLine();
                        Console.Write("Введите новое название продукта: ");
                        string newProductName = Console.ReadLine();

                        Console.WriteLine("Затронуто строк: " + adoRepository.UpdateProduct(oldProductName, newProductName));
                        break;

                    case "4":
                        Console.Write("Введите название продукта: ");
                        productName = Console.ReadLine();

                        Console.WriteLine("Затронуто строк: " + adoRepository.DeleteProduct(productName));
                        break;

                    case "5":
                        Console.WriteLine(efRepository.ReadProducts());
                        break;

                    case "6":
                        Console.Write("Введите название продукта: ");
                        productName = Console.ReadLine();
                        Console.Write("Введите название категории: ");
                        categoryName = Console.ReadLine();
                        Console.Write("Введите название единицы измерения: ");
                        unitName = Console.ReadLine();

                        Console.WriteLine("Затронуто строк: " + efRepository.CreateProduct(productName, categoryName, unitName));
                        break;

                    case "7":
                        Console.Write("Введите старое название продукта: ");
                        oldProductName = Console.ReadLine();
                        Console.Write("Введите новое название продукта: ");
                        newProductName = Console.ReadLine();

                        Console.WriteLine("Затронуто строк: " + efRepository.UpdateProduct(oldProductName, newProductName));
                        break;

                    case "8":
                        Console.Write("Введите название продукта: ");
                        productName = Console.ReadLine();

                        Console.WriteLine("Затронуто строк: " + efRepository.DeleteProduct(productName));
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
