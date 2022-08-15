using System;
using System.Collections.Generic;
using System.Data;
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
                //UpdateCategory(connection);
                //ListCategories(connection);
                //CreateCategory(connection); ( Se nao cria mais de uma vez )
                //DeleteCategory(connection);
                //GetCategory(connection);
                //CreateManyCategory(connection);
                //ExecuteProcedure(connection);
                //ExecuteReadProcedure(connection);
                //ExecuteScalar(connection);
                //ReadView(connection);

            }

            Console.ReadLine();
        }

        static void GetCategory(SqlConnection connection)
        {
            var category = connection
                .QueryFirstOrDefault<Category>(
                    "SELECT TOP 1 [Id], [Title] FROM [Category] WHERE [Id]=@id",
                    new
                    {
                        id = "af3407aa-11ae-4621-a2ef-2028b85507c4"
                    });
            Console.WriteLine($"{category.Id} - {category.Title}");

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

        static void DeleteCategory(SqlConnection connection)
        {
            var deleteQuery = "DELETE [Category] WHERE [Id]=@id";
            var rows = connection.Execute(deleteQuery, new
            {
                id = new Guid("ea8059a2-e679-4e74-99b5-e4f0b310fe6f"),
            });

            Console.WriteLine($"{rows} registros excluídos");
        }

        static void CreateManyCategory(SqlConnection connection)
        {
            var category = new Category();
            category.Id = Guid.NewGuid();
            category.Title = "Amazon";
            category.Url = "amazon";
            category.Summary = "AWS Cloud";
            category.Description = "Categoria destinada a sevicos.";
            category.Order = 8;
            category.Featured = false;

            var category2 = new Category();
            category2.Id = Guid.NewGuid();
            category2.Title = "Categoria Nova";
            category2.Url = "Categoria-Nova";
            category2.Summary = "Categoria Nova";
            category2.Description = "Categoria Nova.";
            category2.Order = 9;
            category2.Featured = true;

            var insertSql = "INSERT INTO [Category] VALUES(@Id,@Title,@Url,@Summary,@Order,@Description,@Featured)";

            var rows = connection.Execute(insertSql, new[]{
                new
                {
                    category.Id,
                    category.Title,
                    category.Url,
                    category.Summary,
                    category.Order,
                    category.Description,
                    category.Featured
                },
                new
                {
                    category2.Id,
                    category2.Title,
                    category2.Url,
                    category2.Summary,
                    category2.Order,
                    category2.Description,
                    category2.Featured
                }
            });
            Console.WriteLine($"{rows} linhas inseridas");
            Console.WriteLine();
        }

        static void ExecuteProcedure(SqlConnection connection)
        {
            var procedure = "[spDeleteStudent]";
            var pars = new { StudentId = "0255a4e3-b86c-40d5-bd3d-16d8d64d65c1" };

            var affectedRows = connection.Execute(procedure, pars, commandType: CommandType.StoredProcedure);

            Console.WriteLine($"{affectedRows} Linhas");
        }
        static void ExecuteReadProcedure(SqlConnection connection)
        {
            var procedure = "[spGetCoursesByCategory]";
            var pars = new { CategoryId = "09ce0b7b-cfca-497b-92c0-3290ad9d5142" };

            var courses = connection.Query(procedure, pars, commandType: CommandType.StoredProcedure);

            foreach(var row in courses)
            {
                Console.WriteLine(row.Title);
            }
        }

        static void ExecuteScalar(SqlConnection connection)
        {
            var category = new Category();
            category.Title = "Amazon";
            category.Url = "amazon";
            category.Summary = "AWS Cloud";
            category.Description = "Categoria destinada a sevicos.";
            category.Order = 8;
            category.Featured = false;

            var insertSql = "INSERT INTO [Category] OUTPUT inserted.[Id] VALUES(NEWID(),@Title,@Url,@Summary,@Order,@Description,@Featured)";

            var id = connection.ExecuteScalar<Guid>(insertSql, new { category.Title, category.Url, category.Summary, category.Order, category.Description, category.Featured });
            Console.WriteLine($"{id} linhas inseridas");
            Console.WriteLine();
        }

        static void ReadView (SqlConnection connection)
        {
            var sql = "Select * FROM [vwCourses]";
            var courses = connection.Query(sql);
            foreach (var item in courses)
            {
                Console.WriteLine($"{item.Id} - - {item.Title}");
            }
        }



    }
}
