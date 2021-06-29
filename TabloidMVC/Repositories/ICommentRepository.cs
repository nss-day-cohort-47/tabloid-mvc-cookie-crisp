using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;

namespace TabloidMVC.Repositories
{
    public interface ICommentRepository
    {
      List<Comment> GetAllCommentsByPostId(int postid);

        void CreateComment(Comment comment);
    }
}
