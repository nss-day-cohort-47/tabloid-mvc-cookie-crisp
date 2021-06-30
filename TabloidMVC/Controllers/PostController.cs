using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using TabloidMVC.Models;
using TabloidMVC.Models.ViewModels;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    [Authorize]
    public class PostController : Controller
    {
        private readonly IPostRepository _postRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly ITagRepository _tagRepository;

        public PostController(IPostRepository postRepository, ICategoryRepository categoryRepository, ITagRepository tagRepository)
        {
            _postRepository = postRepository;
            _categoryRepository = categoryRepository;
            _tagRepository = tagRepository;
        }
        //^^ allows PostContoller to access the methods in the TagRepository

        public IActionResult Index()
        {
            var posts = _postRepository.GetAllPublishedPosts();
            return View(posts);
        }

        public IActionResult Details(int id)
        {
            var post = _postRepository.GetPublishedPostById(id);
            if (post == null)
            {
                int userId = GetCurrentUserProfileId();
                post = _postRepository.GetUserPostById(id, userId);
                if (post == null)
                {
                    return NotFound();
                }
            }
            return View(post);
        }

        public IActionResult Create()
        {
            var vm = new PostCreateViewModel();
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        [HttpPost]
        public IActionResult Create(PostCreateViewModel vm)
        {
            try
            {
                vm.Post.CreateDateTime = DateAndTime.Now;
                vm.Post.IsApproved = true;
                vm.Post.UserProfileId = GetCurrentUserProfileId();

                _postRepository.Add(vm.Post);

                return RedirectToAction("Details", new { id = vm.Post.Id });
            }
            catch
            {
                vm.CategoryOptions = _categoryRepository.GetAll();
                return View(vm);
            }
        }

        public ActionResult Delete(int id)
        {
            Post post = _postRepository.GetPublishedPostById(id);

            return View(post);
        }

        // POST: Post/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int Id, Post post)
        {
            try
            {
                _postRepository.DeletePost(Id);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(post);
            }
        }

        public ActionResult Edit(int id)
        {
            Post post = _postRepository.GetPublishedPostById(id);

            if (post == null)
            {
                return NotFound();
            }

            var vm = new PostCreateViewModel();
            vm.Post = post;
            vm.CategoryOptions = _categoryRepository.GetAll();
            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, PostCreateViewModel vm)
        {
            try
            {
                _postRepository.UpdatePost(vm.Post);

                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                return View(vm.Post);
            }
        }

        public IActionResult MyPosts()
            {
                int CurrentUser = GetCurrentUserProfileId();
                List<Post> posts = _postRepository.GetUsersPosts(CurrentUser);
                return View(posts);
            }

        private int GetCurrentUserProfileId()
          {
            string id = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return int.Parse(id);
          }

        // GET: PostController/Create
        public ActionResult CreatePostTag(int id)
        {
            CreatePostTagViewModel ptvm = new CreatePostTagViewModel();

            ptvm.TagList = _tagRepository.GetAllTags();
            ptvm.Post = new Post();
            ptvm.Post.Id = id;

            return View(ptvm);
         
        }

        // POST: PostController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult CreatePostTag(int postId, int tagId)
        {
            try
            {
                _postRepository.AddTagToPost(postId, tagId);

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
            
        }
    }

}
