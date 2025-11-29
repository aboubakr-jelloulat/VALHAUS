
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.TextTemplating;
using System.Runtime.CompilerServices;
using Valhaus.Data.Data;
using Valhaus.Data.Repository.IRepository;
using Valhaus.Data.Repository.Repositories;
using Valhaus.Models;
using Valhaus.Models.Models;
using Valhaus.Models.ViewModels;
namespace VALHÄUS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductController(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.Products.GetAll().ToList();

            return View(products);
        }

        public ActionResult Upsert(int? id)
        {
            /*
          A projection in EF Core is when you select only specific columns(properties) from a database table instead of loading the entire entity.
                  You are telling EF Core:

              “I do NOT want the full Category entity.
              I only want two things: Name and Id.”
          */


            IEnumerable<SelectListItem> CategoriesList = _unitOfWork.Categories.GetAll()
                .Select(cat => new SelectListItem
                {
                    Text = cat.Name,
                    Value = cat.Id.ToString()

                });

            //ViewBag.CategoriesList = CategoryList;
            //ViewData["CategoriesList"]  = CategoryList;


            ProductVM productVM = new()
            {
                /* 
                 * This creates a new instance of your ViewModel. 
                 
                 * Why new Product()
                        Pass an EMPTY product object to the form
                        If you do NOT create it Product would be null → and the view will crash.
                 
                 */
                Product = new Product(),

                CategoryList = CategoriesList
            };

            if (id is null || id == 0)
            {
                //create
                return View(productVM);
            }
            else
            {
                productVM.Product = _unitOfWork.Products.Get(u => u.Id == id);

                return View(productVM);
            }

        }

        

        [HttpPost]
        public ActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Products.Add(productVM.Product);
                _unitOfWork.Save();
                TempData["success"] = "product created successfully!";
                return RedirectToAction("Index");
            }

            // If ModelState is invalid, repopulate CategoryList
            productVM.CategoryList = _unitOfWork.Categories.GetAll()
                .Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });

            return View(productVM);
        }

        

        public ActionResult Delete(int? id)
        {
            if (id is null or 0)
            {
                return NotFound();
            }
            Product? product = _unitOfWork.Products.Get(item => item.Id == id);
            if (product is null)
            {
                return NotFound();
            }
            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        public ActionResult DeletePOST(int? id)
        {
            Product? product_to_delete = _unitOfWork.Products.Get(item => item.Id == id);

            if (product_to_delete is null)
                return NotFound();

            _unitOfWork.Products.Remove(product_to_delete);

            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully!";

            return RedirectToAction("Index");
        }

    }
}
