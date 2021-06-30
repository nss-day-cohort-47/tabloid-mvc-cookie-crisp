using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using TabloidMVC.Models;
using TabloidMVC.Utils;

namespace TabloidMVC.Repositories
{
    public class UserProfileRepository : BaseRepository, IUserProfileRepository
    {
        public UserProfileRepository(IConfiguration config) : base(config) { }
        public List<UserProfile> GetAllUserProfiles()
        {
            using(SqlConnection conn = Connection)
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                                    SELECT up.displayname, up.firstname, up.lastname, up.id, ut.name 
                                    FROM userprofile up 
                                    LEFT JOIN usertype ut on up.usertypeid = ut.id 
                                    ORDER BY up.displayname";
                   SqlDataReader reader = cmd.ExecuteReader();

                    List<UserProfile> userProfiles = new List<UserProfile>();

                    while(reader.Read())
                    {
                        userProfiles.Add(new UserProfile()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("id")),
                            DisplayName = reader.GetString(reader.GetOrdinal("displayname")),
                            FirstName = reader.GetString(reader.GetOrdinal("firstname")),
                            LastName = reader.GetString(reader.GetOrdinal("lastname")),
                            UserType = new UserType()
                            { 
                            Name = reader.GetString(reader.GetOrdinal("name"))
                            }
                        });
                           
                    }
                    reader.Close();
                    return userProfiles;
                }
            }
        }
        public UserProfile Add(UserProfile user)


        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                        INSERT INTO Userprofile (
                            DisplayName, FirstName, LastName, Email, CreateDateTime, UserTypeId )
                        OUTPUT INSERTED.ID
                        VALUES ( @DisplayName, @FirstName, @LastName, @Email, @CreateDateTime, @UserTypeId)";
                    cmd.Parameters.AddWithValue("@DisplayName", user.DisplayName);
                    cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@LastName", user.LastName);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@CreateDateTime", DateTime.Now);
                    cmd.Parameters.AddWithValue("@UserTypeId", 2);


                    user.Id = (int)cmd.ExecuteScalar();
                    return user;
                }
            }
        }

        public UserProfile GetByEmail(string email)
        {
            using (var conn = Connection)
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = @"
                       SELECT u.id, u.FirstName, u.LastName, u.DisplayName, u.Email,
                              u.CreateDateTime, u.ImageLocation, u.UserTypeId,
                              ut.[Name] AS UserTypeName
                         FROM UserProfile u
                              LEFT JOIN UserType ut ON u.UserTypeId = ut.id
                        WHERE email = @email";
                    cmd.Parameters.AddWithValue("@email", email);

                    UserProfile userProfile = null;
                    var reader = cmd.ExecuteReader();

                    if (reader.Read())
                    {
                        userProfile = new UserProfile()
                        {
                            Id = reader.GetInt32(reader.GetOrdinal("Id")),
                            Email = reader.GetString(reader.GetOrdinal("Email")),
                            FirstName = reader.GetString(reader.GetOrdinal("FirstName")),
                            LastName = reader.GetString(reader.GetOrdinal("LastName")),
                            DisplayName = reader.GetString(reader.GetOrdinal("DisplayName")),
                            CreateDateTime = reader.GetDateTime(reader.GetOrdinal("CreateDateTime")),
                            ImageLocation = DbUtils.GetNullableString(reader, "ImageLocation"),
                            UserTypeId = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                            UserType = new UserType()
                            {
                                Id = reader.GetInt32(reader.GetOrdinal("UserTypeId")),
                                Name = reader.GetString(reader.GetOrdinal("UserTypeName"))
                            },
                        };
                    }

                    reader.Close();

                    return userProfile;
                }
            }
        }
    }
}
