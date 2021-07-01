using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {
        public UserProfile GetProfileById(int id);
        public UserProfile Add(UserProfile user);
        UserProfile GetByEmail(string email);
        List<UserProfile> GetAllUserProfiles();
    }
}