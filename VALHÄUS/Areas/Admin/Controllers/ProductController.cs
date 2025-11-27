
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Valhaus.Data.Data;
using Valhaus.Data.Repository.IRepository;
using Valhaus.Data.Repository.Repositories;
using Valhaus.Models;
using Valhaus.Models.Models;
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
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Products.Add(product);
                _unitOfWork.Save();
                TempData["success"] = "product created successfully!";
                return RedirectToAction("Index");
            }

            return View();
        }

        public ActionResult Edit(int? id)
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


        [HttpPost]
        public ActionResult Edit(Product product)
        {

            if (ModelState.IsValid)
            {
                _unitOfWork.Products.Update(product);
                _unitOfWork.Save();
                TempData["success"] = "product updated successfully!";

                return RedirectToAction("Index");
            }

            return View();
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
 