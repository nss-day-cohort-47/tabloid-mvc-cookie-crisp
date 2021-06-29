using System.Collections.Generic;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface IPostRepository
    {
        void Add(Post post);
        List<Post> GetAllPublishedPosts();
        List<Post> GetUsersPosts(int userProfileId);
        Post GetPublishedPostById(int id);
        void UpdatePost(Post post);
        Post GetUserPostById(int id, int userProfileId);
    }
}