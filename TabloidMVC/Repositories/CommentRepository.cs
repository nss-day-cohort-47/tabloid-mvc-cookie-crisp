using Microsoft.Extensions.Configuration;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class CommentRepository : ICommentRepository
    {
            private readonly IConfiguration _config;
            public CommentRepository(IConfiguration config)
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
            public List<Comment> GetAllCommentsByPostId(int postid)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"select c.id, c.postid as commentpostid, c.userprofileid, c.subject, c.content, c.createdatetime, p.id as Postid from comment c 
                                        left join post p on c.postId = p.id 
                                        where p.id =@postid";

                    cmd.Parameters.AddWithValue("@postid", postid);

                    SqlDataReader reader = cmd.ExecuteReader();

                    List<Comment> comments = new List<Comment>();

                        while (reader.Read())
                    {
                        Comment comment = new Comment()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            PostId = reader.GetInt32(reader.GetOrdinal("commentpostid")),
                            UserProfileId = reader.GetInt32(reader.GetOrdinal("userprofileid")),
                            Subject = reader.GetString(reader.GetOrdinal("subject")),
                            Content = reader.GetString(reader.GetOrdinal("content")),
                            CreateDateTime= reader.GetDateTime(reader.GetOrdinal("createdatetime"))

                        };
                        comments.Add(comment);

                    }
                    reader.Close();
                    return comments;

                }
            }
        }

        public void CreateComment(Comment comment)
        {
        using(SqlConnection conn = Connection)
            { 
            conn.Open();
            using (var cmd = conn.CreateCommand())
            
                    {
                        cmd.CommandText = @"
                            INSERT INTO Comment (postid, userprofileid, subject, content, createdatetime
                            OUT Inserted.Id
                            VALUES (@postid, @userprofileid, @subject, @content, @createdatetime);      ";
                        cmd.Parameters.AddWithValue("@postid", comment.PostId);
                        cmd.Parameters.AddWithValue("@userprofileid", comment.UserProfileId);
                        cmd.Parameters.AddWithValue("@subject", comment.Subject);
                        cmd.Parameters.AddWithValue("@content", comment.Content);
                        cmd.Parameters.AddWithValue("@createdatetime", comment.CreateDateTime);

                        int id = (int)cmd.ExecuteScalar();
                        comment.Id = id;
                    }   
                
            }

        }
    }
}
