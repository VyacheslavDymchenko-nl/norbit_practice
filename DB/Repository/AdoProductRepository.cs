using Microsoft.Data.SqlClient;
using System.Data;
using System.Text;

namespace DB.Repository
{
    public class AdoProductRepository
    {
        private readonly string _connectionString;

        public AdoProductRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public async Task<string> ReadProductsAsync()
        {
            const string CommandText = """
                SELECT p.ProductName AS [Название],
                       c.CategoryName AS [Категория],
                       u.UnitName AS [Единица измерения]
                FROM   Products AS p
                       LEFT OUTER JOIN
                       Categories AS c
                       ON p.CategoryID = c.CategoryID
                       LEFT OUTER JOIN
                       Units AS u
                       ON p.UnitID = u.UnitID;
                """;
            var result = new StringBuilder();
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(CommandText, connection);

            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    string columnName = reader.GetName(i);
                    object value = reader.IsDBNull(i)
                        ? "NULL"
                        : reader.GetValue(i);

                    result.AppendLine($"{columnName}: {value}");
                }
                result.AppendLine();
            }

            return result.ToString();
        }

        public async Task<int> CreateProductAsync(string productName, string categoryName, string unitName)
        {
            const string CommandText = """
                INSERT  INTO Products (ProductName, CategoryID, UnitID)
                VALUES               (@ProductName, (SELECT CategoryID
                                                   FROM   Categories
                                                   WHERE  CategoryName = @CategoryName), (SELECT UnitID
                                                                                     FROM   Units
                                                                                     WHERE  UnitName = @UnitName));
                """;
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(CommandText, connection);

            command.Parameters.Add("@ProductName", SqlDbType.NVarChar, 100).Value = productName;
            command.Parameters.Add("@CategoryName", SqlDbType.NVarChar, 100).Value = categoryName;
            command.Parameters.Add("@UnitName", SqlDbType.NVarChar, 100).Value = unitName;

            await connection.OpenAsync();

            return await command.ExecuteNonQueryAsync();
        }

        public async Task<int> DeleteProductAsync(string productName)
        {
            const string CommandText = """
                DELETE FROM Products
                WHERE ProductName = @ProductName;
                """;
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(CommandText, connection);

            command.Parameters.Add("@ProductName", SqlDbType.NVarChar, 100).Value = productName;

            await connection.OpenAsync();

            return await command.ExecuteNonQueryAsync();
        }

        public async Task<int> UpdateProductAsync(string oldProductName, string newProductName)
        {
            const string CommandText = """
                UPDATE Products
                SET ProductName = @NewProductName
                WHERE ProductName = @OldProductName;
                """;
            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(CommandText, connection);

            command.Parameters.Add("@NewProductName", SqlDbType.NVarChar, 100).Value = newProductName;
            command.Parameters.Add("@OldProductName", SqlDbType.NVarChar, 100).Value = oldProductName;

            await connection.OpenAsync();

            return await command.ExecuteNonQueryAsync();
        }
    }
}
