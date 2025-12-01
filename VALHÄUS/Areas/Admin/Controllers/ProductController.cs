
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.VisualStudio.TextTemplating;
using System;
using System.IO;
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
        private readonly IWebHostEnvironment _webHostEnvironment;

        /* IWebHostEnvironment is a builtvin interface in ASP.NET Core
            It provides information about the web hosting environment your app is running in, provide :
                ContentRootPath → The root path of your application (where.csproj is)
                WebRootPath → Path to wwwroot(where static files live)
                EnvironmentName → Development, Production, or Staging  
        */

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            this._unitOfWork = unitOfWork;
            this._webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.Products.GetAll(includeProperties: "Categories").ToList();

            // Uses includeProperties: "Categories" to fetch related categories.
            //Tell EF When fetching products also fetch the related categories.

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
                // update
                productVM.Product = _unitOfWork.Products.Get(u => u.Id == id);

                return View(productVM);
            }

        }



        [HttpPost]
        [RequestSizeLimit(104857600)]
        public ActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            
            
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string uploadPath = Path.Combine(wwwRootPath, @"images/product");

                    // CREATE DIRECTORY IF IT DOES NOT EXIST
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    // DELETE OLD IMAGE 
                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {

                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\', '/'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    // SAVE NEW FILE
                    using (var fileStream = new FileStream(Path.Combine(uploadPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }


                // CREATE
                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Products.Add(productVM.Product);
                    TempData["success"] = "Product created successfully!";
                }
                else
                {
                    // UPDATE
                    _unitOfWork.Products.Update(productVM.Product);
                    TempData["success"] = "Product updated successfully!";
                }

                _unitOfWork.Save();
                return RedirectToAction("Index");
            }

            // Reload categories on error
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
