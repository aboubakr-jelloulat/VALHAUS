using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Valhaus.Data.Data;
using Valhaus.Data.Repository.IRepository;
using Valhaus.Data.Repository.Repositories;
using Valhaus.Models;

namespace VALHÄUS.Controllers
{
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        // Constructor - ASP.NET Core AUTOMATICALLY injects the db here
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public  IActionResult Index()
        {
            List<Category> categories = _unitOfWork.Categories.GetAll().ToList();
            return View(categories);
        }


        public ActionResult CreateNewCategory()
        {
            return View();
        }

        [HttpPost] // ← Only accept POST requests (form submissions)
        //This method should ONLY run when the user SUBMITS a form
        public ActionResult CreateNewCategory(Category category)
        {
            
            if (ModelState.IsValid)
            {
                _unitOfWork.Categories.Add(category);
                _unitOfWork.Save();
                TempData["success"] = "Category created successfully!";
                return RedirectToAction("Index");
                //if you need to redirect it in another place not in the same Controller
                //return RedirectToAction("Index", "Category");
            }

            return View();
        }




        //Edit

        public ActionResult Edit(int? id)
        {
            if (id is null or 0)
            {
                return NotFound();
            }
            Category? category = _unitOfWork.Categories.Get(item => item.Id == id);
            if (category is null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public ActionResult Edit(Category category)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Categories.Update(category);
                _unitOfWork.Save();
                 TempData["success"] = "Category updated successfully!";

                return RedirectToAction("Index");
            }

            return View();
        }


        //delete

        public ActionResult Delete(int? id)
        {
            if (id is null or 0)
            {
                return NotFound();
            }
            Category? category = _unitOfWork.Categories.Get(item => item.Id == id);
            if (category is null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeletePOST(int? id)
        {
            Category? category_to_delete = _unitOfWork.Categories.Get(item => item.Id == id);

            if (category_to_delete is null)
                return NotFound();

            _unitOfWork.Categories.Remove(category_to_delete);

            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully!";

            return RedirectToAction("Index");
        }

    }
}

