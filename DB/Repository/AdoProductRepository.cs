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

        public string ReadProducts()
        {
            const string Sql = """
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

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(Sql, connection))
            {
                connection.Open();
                using var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        string columnName = reader.GetName(i);

                        object value = reader.IsDBNull(i)
                            ? "NULL"
                            : reader.GetValue(i);

                        result.Append($"{columnName}: {value} | ");
                    }
                }
            }

            return result.ToString();
        }

        public int CreateProduct(string productName, string categoryName, string unitName)
        {
            const string Sql = """
                INSERT  INTO Products (ProductName, CategoryID, UnitID)
                VALUES               (@ProductName, (SELECT CategoryID
                                                   FROM   Categories
                                                   WHERE  CategoryName = @CategoryName), (SELECT UnitID
                                                                                     FROM   Units
                                                                                     WHERE  UnitName = @UnitName));
                """;

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(Sql, connection))
            {
                command.Parameters.Add("@ProductName", SqlDbType.NVarChar, 100).Value = productName;
                command.Parameters.Add("@CategoryName", SqlDbType.NVarChar, 100).Value = categoryName;
                command.Parameters.Add("@UnitName", SqlDbType.NVarChar, 100).Value = unitName;

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public int DeleteProduct(string productName)
        {
            const string Sql = """
                DELETE FROM Products
                WHERE ProductName = @ProductName
                """;

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(Sql, connection))
            {
                command.Parameters.Add("@ProductName", SqlDbType.NVarChar, 100).Value = productName;

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }

        public int UpdateProduct(string oldProductName, string newProductName)
        {
            const string Sql = """
                UPDATE Products
                SET ProductName = @NewProductName
                WHERE ProductName = @OldProductName
                """;

            using (var connection = new SqlConnection(_connectionString))
            using (var command = new SqlCommand(Sql, connection))
            {
                command.Parameters.Add("@NewProductName", SqlDbType.NVarChar, 100).Value = newProductName;
                command.Parameters.Add("@OldProductName", SqlDbType.NVarChar, 100).Value = oldProductName;

                connection.Open();
                return command.ExecuteNonQuery();
            }
        }
    }
}
