using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Valhaus.Data.Data;
using Valhaus.Models;

namespace VALHÄUS.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        // Constructor - ASP.NET Core AUTOMATICALLY injects the db here
        public CategoryController(ApplicationDbContext db)
        {
            this._db = db;
        }
        public async Task<IActionResult> Index()
        {
            List<Category> categories = await _db.Categories.ToListAsync();
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
                _db.Categories.Add(category);
                _db.SaveChanges();
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
            Category? category = _db.Categories.Find(id);
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
                _db.Categories.Update(category);
                _db.SaveChanges();
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
            Category? category = _db.Categories.Find(id);
            if (category is null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeletePOST(int? id)
        {
            Category? category_to_delete = _db.Categories.Find(id);

            if (category_to_delete is null)
                return NotFound();

            _db.Categories.Remove(category_to_delete);

            _db.SaveChanges();
            TempData["success"] = "Category deleted successfully!";

            return RedirectToAction("Index");
        }

    }
}

