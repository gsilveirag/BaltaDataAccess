using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BaltaDataAccess.Models;
using Dapper;
using Microsoft.Data.SqlClient;

namespace BaltaDataAccess
{
    internal class Program
    {
        static void Main(string[] args)
        {
            const string connectionString = "Server=localhost,1433;Database=balta-completo;User ID=sa;Password=1q2w3e4r@#$;Trusted_Connection=False; TrustServerCertificate=True;";
            //Integrated Security= SSPI ( No casa de windowns usuario )
            //Microsoft.Data.SqlClient ou OracleClient etc ( Pacote (Nuget) para conectar ao banco de dados )

            

            using (var connection = new SqlConnection(connectionString))
            {
                UpdateCategory(connection);
                ListCategories(connection);
                //CreateCategory(connection); ( Se nao cria mais de uma vez )
                
            }

            Console.ReadLine();
        }

        static void ListCategories (SqlConnection connection)
        {
            var categories = connection.Query<Category>("SELECT [Id],[Title] FROM [Category]");
            foreach (var item in categories)
            {
                Console.WriteLine($"{item.Id} - - {item.Title}");
            }
        }

        static void CreateCategory(SqlConnection connection)
        {
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Amazon";
            category.Url = "amazon";
            category.Summary = "AWS Cloud";
            category.Description = "Categoria destinada a sevicos.";
            category.Order = 8;
            category.Featured = false;

            var insertSql = "INSERT INTO [Category] VALUES(@Id,@Title,@Url,@Summary,@Order,@Description,@Featured)";

            var rows = connection.Execute(insertSql, new { category.Id, category.Title, category.Url, category.Summary, category.Order, category.Description, category.Featured });
            Console.WriteLine($"{rows} linhas inseridas");
            Console.WriteLine();
        }

        static void UpdateCategory(SqlConnection connection)
        {
            var updateQuery = "UPDATE [Category] SET [Title] = @title WHERE [Id] =@id";

            var rows = connection.Execute(updateQuery, new { id = new Guid("af3407aa-11ae-4621-a2ef-2028b85507c4"), Title = "Frontend 2021" });

            Console.WriteLine($"{rows} linhas atualizadas.");
        }




    }
}
