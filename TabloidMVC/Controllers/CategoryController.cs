using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TabloidMVC.Models;
using TabloidMVC.Repositories;

namespace TabloidMVC.Controllers
{
    public class CategoryController : Controller
    {

        private readonly ICategoryRepository _categoryRepository;
        private object _categoryRepositiory;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        // GET: category controller
        public IActionResult Index()
        {
            var categories = _categoryRepository.GetAll();
            return View(categories);
        }

        // GET: categoryController/Details/5
        public IActionResult Details(int id)
        {
            return View();
        }

        //GET category create

        public IActionResult Create()
        {
            return View();
        }

        //POST category create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category category)

        {
            try
            {
                _categoryRepository.AddCategory(category);

                return RedirectToAction("Index");
            }
            catch
            {
                return View(category);
            }
        }

        // GET: CategoryController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CategoryController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CategoryController/Delete/
        public ActionResult Delete(int id)
        {
            Category category = _categoryRepositiory.GetCategoryById(id);
            return View(category);
        }

        // POST: CategoryController/Delete/
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Category category)
        {
            try
            {
                _categoryRepository.DeleteCategory(id);
                return RedirectToAction("Index");
            }
            catch
            {
                return View(category);
            }
        }
    }


}
