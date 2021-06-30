//controls users interaction with the app (calls on methods from TagRepository)

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class TagsController : Controller
    {
        private readonly ITagRepository _tagRepo;
        // ^^ _tagRepo is instance of ITagRepository
        public TagsController(ITagRepository tagRepository)
        {
            _tagRepo = tagRepository;
        }

        // GET: TagsController
        public ActionResult Index()
        {
            List<Tag> tags = _tagRepo.GetAllTags();

            return View(tags);
        }

       
        // GET: TagsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: TagsController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TagsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Tag tag)
        {
            try
            {
                _tagRepo.AddTag(tag);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(tag);
            }
            //use AddTag method to create tag object, then redirect user to Index view
        }

        // GET: TagsController/Edit/5
        public ActionResult Edit(int id)
        {
            Tag tag = _tagRepo.GetTagById(id);

            if (tag == null)
            {
                return NotFound();
            }

            return View(tag);
        }
        // POST: TagsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Tag tag)
        {
            try
            {
                _tagRepo.UpdateTag(tag);
                //UpdateTag() is imported from TagRepository.cs
                return RedirectToAction("Index");
            }
            catch
            {
                return View(tag);
            }
        }


        // GET: TagsController/Delete/5
        public ActionResult Delete(int id)
        {
            Tag tag = _tagRepo.GetTagById(id);
            return View(tag);
        }

        // POST: TagsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Tag tag)
        {
            try
            {
                _tagRepo.DeleteTag(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(tag);
            }
        }
    }
}