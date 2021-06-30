using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IUserProfileRepository
    {
        public UserProfile Add(UserProfile user);
        UserProfile GetByEmail(string email);
    }
}