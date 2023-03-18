﻿using CMSWebsite.Models;
using CMSWebsite.RepositoriesInterfaces;
using CMSWebsite.ServiceInterfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CMSWebsite.Controllers
{
    [Authorize(Roles = "Admin")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public IActionResult Index()
        {
            List<Category> categories = _categoryService.GetAllCategories().ToList();

            return View(categories);
        }

        [HttpGet]
        public IActionResult AddCategory()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCategory(Category model) 
        {
            if (ModelState.IsValid)
            {
                _categoryService.AddCategory(model);
                
                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult CategoryDetails(int id) 
        {
            var category = _categoryService.GetCategoryById(id);

            if(category == null)
            {
                return RedirectToAction("NotFound404", "Home");
            }
            else
            {
                category.Albums = _categoryService.GetAlbumsByCategoryId(category.CategoryId).ToList();
                return View(category);
            }
        }        
        
        [HttpGet]
        public IActionResult EditCategory(int id) 
        {
            var category = _categoryService.GetCategoryById(id);

            if(category == null)
            {
                return RedirectToAction("NotFound404", "Home");
            }
            else
            {
                return View(category);
            }
        }

        [HttpPost]
        public IActionResult EditCategory(Category model)
        {
            if (ModelState.IsValid)
            {
                _categoryService.EditCategory(model);

                return RedirectToAction("Index");
            }
            else
            {
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult DeleteCategory(int id)
        {
            var category = _categoryService.GetCategoryById(id);

            if (category == null)
            {
                return RedirectToAction("NotFound404", "Home");
            }
            else
            {
                return View(category);
            }
        }

        [HttpPost]
        public IActionResult DeleteCategory(Category model)
        {
            if (ModelState.IsValid)
            {
                _categoryService.DeleteCategory(model.CategoryId);

                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("NotFound404", "Home");
            }
        }
    }
}
