//holds constructor (where all methods are written)

using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public class TagRepository : ITagRepository
    // ^ allows TagRepository to use methods declared in ITagRepository
    {
        private readonly IConfiguration _config;
        // The constructor accepts an IConfiguration object as a parameter. This class comes from the ASP.NET framework and is useful for retrieving things out of the appsettings.json file like connection strings.

        public TagRepository(IConfiguration config)
        {
            _config = config;
        }

        public SqlConnection Connection
        {
            get
            {
                return new SqlConnection(_config.GetConnectionString("DefaultConnection"));
            }
        }
        public List<Tag> GetAllTags()
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                //open connection

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    SELECT Id, [Name] 
                    FROM Tag
                    ORDER BY name ASC ";
                    //SQL request to database

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Tag> tags = new List<Tag>();
                    while (reader.Read())
                    {
                        Tag tag = new Tag
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Name = reader.GetString(reader.GetOrdinal("Name"))
                            // ^ passed in from the Tag class in Tag.cs
                        };

                        tags.Add(tag);
                    }
                    //while connection is open- read specified columns in database, use them to create an object, then add new object to list

                    reader.Close();
                    return tags;
                    //close connection and return full list
                }
            }
        }

        public void AddTag(Tag tag)
        {
            using (SqlConnection conn = Connection)
            {
                conn.Open();
                //open connection

                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                    INSERT INTO Tag ([Name])
                    OUTPUT INSERTED.ID
                    VALUES (@name);
                    ";

                    cmd.Parameters.AddWithValue("@name", tag.Name);
                    // "OUTPUT INSERTED.ID" tells program to create id // AddWithValue adds object to the database and tag.Name tells which column and row

                    int newlyCreatedId = (int)cmd.ExecuteScalar();
                    tag.Id = newlyCreatedId;
                }
            }

        }
    }
}
