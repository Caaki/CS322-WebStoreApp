using Microsoft.AspNetCore.Mvc;
using System.Text.RegularExpressions;
using WebApp.DataAccess.Data;
using WebApp.DataAccess.Repository.IRepository;
using WebApp.Models;

namespace WebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.CategoryRepository.GetAll().ToList();
            return View(objCategoryList); ;
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {

            Regex rx = new Regex(".*[0-9].*");
            if (category.Name == null || rx.Matches(category.Name).Count >= 1)
            {
                ModelState.AddModelError("", "The Name mustn't contain numbers or be empty");
            }
            else if (ModelState.IsValid)
            {
                _unitOfWork.CategoryRepository.Add(category);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully";
                return RedirectToAction("Index", "Category");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Delete(int? id)
        {
            Category? category = _unitOfWork.CategoryRepository.Get(u => u.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            _unitOfWork.CategoryRepository.Delete(category);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted";
            return RedirectToAction("Index", "Category");
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Category? category = _unitOfWork.CategoryRepository.Get(u => u.Id == id);
            //Category? category = _db.Categories.FindOrDefault(u => u.Id==id);
            //Category? category = _db.Categories.Where(u =>u.Id == id).FirstOrDeafult();
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }


        [HttpPost]
        public IActionResult Edit(Category category)
        {

            Regex rx = new Regex(".*[0-9].*");
            if (category.Name == null || rx.Matches(category.Name).Count >= 1)
            {
                ModelState.AddModelError("", "The Name mustn't contain numbers or be empty");
            }
            else if (ModelState.IsValid)
            {
                _unitOfWork.CategoryRepository.Update(category);
                _unitOfWork.Save();
                TempData["success"] = "Category updated successfully";
                return RedirectToAction("Index", "Category");
            }


            return View();
        }


    }
}
