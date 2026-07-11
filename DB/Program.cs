using Microsoft.Data.SqlClient;

namespace DB
{
    internal class Program
    {
        static void Main()
        {
            const string connectionString = """
                Data Source=localhost;
                Initial Catalog=FoodTracker;
                Integrated Security=True;
                Encrypt=True;
                TrustServerCertificate=True;
                """;

            const string sql = """
                UPDATE Products
                SET ProductName = 'Говядина'
                WHERE ProductName = 'Курица'
                
                SELECT ProductName AS [Название], IsActive AS [Можно покупать]
                FROM Products
                """;

            using (var connection = new SqlConnection(connectionString))
            using (var command = new SqlCommand(sql, connection))
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

                        Console.Write($"{columnName}: {value} | ");
                    }

                    Console.WriteLine();
                }
            }
        }
    }
}
