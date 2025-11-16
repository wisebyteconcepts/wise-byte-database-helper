using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WiseByteDatabaseHelper.Core.Attributes;
using WiseByteDatabaseHelper.Core.Context;
using Xunit;
using Microsoft.Data.Sqlite;

namespace WiseByteDatabaseHelper.Tests
{
    [Table("Customers")]
    public class Customer
    {
        [PrimaryKey, AutoIncrement]
        [Column("CustomerId")]
        public int Id { get; set; }

        [Column("CustomerName")]
        public string? Name { get; set; }
    }

    [Table("Orders")]
    public class Order
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }

        [ForeignKey(typeof(Customer), nameof(Customer.Id))]
        public int CustomerId { get; set; }

    }

    public class TestDbContext : DatabaseContext
    {
        public TestDbContext(string file)
        {
            UseSqlite(file);

            Table.Create<Customer>();
            Table.Create<Order>();
        }
    }

    public class DatabaseCreationTests
    {
        private static string CreateTempDbPath()
        {
            string desktop = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string file = Path.Combine(desktop, $"TestDb_{Guid.NewGuid()}.sqlite");

            // Ensure no leftover file with same name exists
            if (File.Exists(file))
                File.Delete(file);

            return file;
        }

        [Fact]
        public async Task CreateDatabase_CreatesTables_WithForeignKeys()
        {
            // Arrange
            var dbPath = CreateTempDbPath();
            var context = new TestDbContext(dbPath);

            // Act
            await context.CreateDatabaseAsync();

            // Assert - database file created
            Assert.True(File.Exists(dbPath));

            // Now inspect SQLite schema
            using var conn = new SqliteConnection($"Data Source={dbPath}");
            conn.Open();

            // Check Customers table exists
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText =
                    "SELECT name FROM sqlite_master WHERE type='table' AND name='Customers';";
                var result = cmd.ExecuteScalar();
                Assert.Equal("Customers", result);
            }

            // Check Orders table exists
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText =
                    "SELECT name FROM sqlite_master WHERE type='table' AND name='Orders';";
                var result = cmd.ExecuteScalar();
                Assert.Equal("Orders", result);
            }

            // Check Orders.CustomerId FK exists
            {
                using var cmd = conn.CreateCommand();
                cmd.CommandText = "PRAGMA foreign_key_list('Orders');";

                using var reader = cmd.ExecuteReader();

                bool found = false;

                while (reader.Read())
                {
                    var table = reader["table"].ToString();        // should be "Customers"
                    var from = reader["from"].ToString();          // "CustomerId"
                    var to = reader["to"].ToString();              // "Id"

                    if (table == "Customers" && from == "CustomerId" && to == "CustomerId")
                        found = true;
                }

                Assert.True(found, "Foreign key Orders.CustomerId → Customers.Id must exist.");
            }
        }
    }
}
