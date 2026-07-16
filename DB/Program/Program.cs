using DB.Data;
using DB.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DB.Program
{
    internal class Program
    {
        /// <summary>
        /// Выводит сообщение, считывает строку с консоли, и проверяет что она не пустая.
        /// </summary>
        /// <param name="message">Выводимое в консоль сообщение пользователю.</param>
        /// <returns>Непустая строка.</returns>
        private static string ReadRequired(string message)
        {
            while (true)
            {
                Console.Write(message);

                string? value = Console.ReadLine();

                if (!string.IsNullOrWhiteSpace(value))
                {
                    return value.Trim();
                }

                Console.WriteLine("Значение не может быть пустым.");
            }
        }
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

                try
                {
                    switch (choice)
                    {
                        case "1":
                            Console.WriteLine(await adoRepository.ReadProductsAsync());

                            break;

                        case "2":
                            string productName = ReadRequired("Введите название продукта: ");
                            string categoryName = ReadRequired("Введите название категории: ");
                            string unitName = ReadRequired("Введите название единицы измерения: ");

                            Console.WriteLine($"Затронуто строк: {await adoRepository.CreateProductAsync(productName, categoryName, unitName)}");

                            break;

                        case "3":
                            string oldProductName = ReadRequired("Введите старое название продукта: ");
                            string newProductName = ReadRequired("Введите новое название продукта: ");

                            Console.WriteLine($"Затронуто строк: {await adoRepository.UpdateProductAsync(oldProductName, newProductName)}");

                            break;

                        case "4":
                            productName = ReadRequired("Введите название продукта: ");

                            Console.WriteLine($"Затронуто строк: {await adoRepository.DeleteProductAsync(productName)}");

                            break;

                        case "5":

                            Console.WriteLine(await efRepository.ReadProductsAsync());

                            break;

                        case "6":
                            productName = ReadRequired("Введите название продукта: ");
                            categoryName = ReadRequired("Введите название категории: ");
                            unitName = ReadRequired("Введите название единицы измерения: ");

                            Console.WriteLine($"Затронуто строк: {await efRepository.CreateProductAsync(productName, categoryName, unitName)}");

                            break;

                        case "7":
                            oldProductName = ReadRequired("Введите старое название продукта: ");
                            newProductName = ReadRequired("Введите новое название продукта: ");

                            Console.WriteLine($"Затронуто строк: {await efRepository.UpdateProductAsync(oldProductName, newProductName)}");

                            break;

                        case "8":
                            productName = ReadRequired("Введите название продукта: ");

                            Console.WriteLine($"Затронуто строк: {await efRepository.DeleteProductAsync(productName)}");

                            break;

                        case "0":

                            return;

                        default:
                            Console.WriteLine("Неизвестная команда.");

                            break;
                    }
                }
                catch (InvalidOperationException exception)
                {
                    Console.WriteLine($"Ошибка операции: {exception.Message}");
                }
                catch (DbUpdateException exception)
                {
                    Console.WriteLine($"""
                        Не удалось сохранить изменения в базе данных:                        
                        {exception.InnerException?.Message ?? exception.Message}
                        """);
                }
                catch (SqlException exception)
                {
                    Console.WriteLine($"Ошибка SQL Server: {exception.Message}");
                }
            }
        }
    }
}
