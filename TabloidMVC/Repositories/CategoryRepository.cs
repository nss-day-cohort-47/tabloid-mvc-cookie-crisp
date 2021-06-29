using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class CategoryRepository : BaseRepository, ICategoryRepository
    {
        public CategoryRepository(IConfiguration config) : base(config) { }

        public List<Category> GetAll()
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT id, name FROM Category
                        ORDER BY name ASC";

                    var reader = cmd.ExecuteReader();

                    var categories = new List<Category>();

                    while (reader.Read())
                    {
                        Category category = new Category
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("name")),
                        };
                        categories.Add(category);
                    }

                    reader.Close();

                    return categories;
                }
            }
        }



        public void AddCategory(Category category)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
             

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO Category([Name])
                    OUTPUT INSERTED.ID
                    VALUES (@name);
                    ";

                    cmd.Parameters.AddWithValue("@name", category.Name);
                 

                    int newlyCreatedId = (int)cmd.ExecuteScalar();
                    category.Id = newlyCreatedId;
                }
            }

        }

        public Category GetCategoryById(int id)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        SELECT Id, [Name]
                        FROM Category
                        WHERE Id = @id";

                    cmd.Parameters.AddWithValue("@id", id);

                    SqlDataReader reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        Category category = new Category
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                        };
                        reader.Close();
                        return category;
                    }
                    else
                    {
                        reader.Close();
                        return null;
                    }
                }
            }
        }

        public void UpdateCategory(Category category)
        {
            using (var conn = Connection)
            {
                conn.Open();

                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"UPDATE Category 
                                        SET Name = @name
                                        WHERE Id = @id;";

                    cmd.Parameters.AddWithValue("@id", category.Id);
                    cmd.Parameters.AddWithValue("@name", category.Name);

                    cmd.ExecuteNonQuery();
                }
            }
        }
        public void DeleteCategory(int categoryId)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        DELETE FROM Category
                        WHERE Id = @id
                        ";

                    cmd.Parameters.AddWithValue("@id", categoryId);

                    cmd.ExecuteNonQuery();
                    
                }

            }

        }

    }
}