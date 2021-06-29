using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Repositories;
using Microsoft.VisualBasic;
using TabloidMVC.Models.ViewModels;

namespace TabloidMVC.Controllers
{
    public class CommentController : Controller
    {

        private int GetCurrentUserProfileId()
        {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
        }
        private readonly IPostRepository _postRepo;
        private readonly ICommentRepository _commentRepo;

        public CommentController(IPostRepository postRepository, ICommentRepository commentRepository)
        {
            _postRepo = postRepository;
            _commentRepo = commentRepository;
        }
        // GET: CommentController

        public ActionResult Index(int id)
        {
            CommentViewModel vm = new CommentViewModel()
            {
                CommentList = _commentRepo.GetAllCommentsByPostId(id),
                Posts = new Post()
                {
                    Id = id
                }

        };
            
            return View(vm);
        }

        // GET: CommentController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: CommentController/Create
        public ActionResult Create(int id)
        {
           Post posts = _postRepo.GetPublishedPostById(id);

            CommentViewModel vm = new CommentViewModel()
            {
                Comment = new Comment(),
                Posts = posts
            };                                                                             
            vm.Comment.PostId = id;

            return View(vm);
        }

        // POST: CommentController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Comment Comment, Post Posts)
        {
            try
            {
                Comment.UserProfileId = GetCurrentUserProfileId();
                Comment.PostId = Posts.Id;

                _commentRepo.CreateComment(Comment);

                return RedirectToAction("Index", new {id=Comment.PostId});

        }
            catch
            {
                return View(Comment);
    }
}

        // GET: CommentController/Edit/5
        public ActionResult Edit(int id)
        {
            Comment Comment = _commentRepo.GetCommentById(id);
            CommentViewModel vm = new CommentViewModel()
            {
                Comment = Comment

            };

            return View(vm);
        }

        // POST: CommentController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Comment comment, Post Posts)
        {
            try
            {
             
            _commentRepo.UpdateComment(comment);
            return RedirectToAction("Index", new { id = comment.PostId });
        }
            catch
            {
                return View();
    }
}

        // GET: CommentController/Delete/5
        public ActionResult Delete(int id)
        {
           
            Comment Comment = _commentRepo.GetCommentById(id);

            CommentViewModel vm = new CommentViewModel()
            {
                Comment = Comment
              
            };
            
            return View(vm);
        }

        // POST: CommentController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Comment comment, Post Posts)
        {

            try
            {
                _commentRepo.DeleteComment(id);
                return RedirectToAction("Index", new { id = comment.PostId });
        }
            catch
            {
                return View();
    }
}
    }
}
