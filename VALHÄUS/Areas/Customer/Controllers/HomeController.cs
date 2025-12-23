using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Diagnostics;
using System.Diagnostics;
using System.Security.Claims;
using Valhaus.Data.Repository.IRepository;
using Valhaus.Models;
using Valhaus.Models.Models;
using Valhaus.Utils;

namespace VALHAUS.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            
            IEnumerable<Product> Products = _unitOfWork.Products.GetAll(includeProperties: "Categories");

            return View(Products);
        }

        public IActionResult Details(int Productid)
        {
            ShoppingCart cart = new()
            {
                Product = _unitOfWork.Products.Get(u => u.Id == Productid, includeProperties: "Categories"),
                Count = 1,
                ProductId = Productid
            };

            return View(cart);
        }



        [HttpPost]
        [Authorize]
        //It protects the action so only logged-in users can access it.
        public IActionResult Details(ShoppingCart shoppingCart)
        {

            var productFromDb = _unitOfWork.Products.Get(u => u.Id == shoppingCart.ProductId);

            if (productFromDb == null)
            {
                return NotFound();
            }

            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            shoppingCart.ApplicationUserId = userId;
            
            ShoppingCart cartFromDb = _unitOfWork.ShoppingCart.Get(u => u.ApplicationUserId == userId && u.ProductId == shoppingCart.ProductId);
            
            if (cartFromDb != null)
            {
                // update
                TempData["success"] = "Product quantity updated successfully!";
                cartFromDb.Count += shoppingCart.Count;
                _unitOfWork.ShoppingCart.Update(cartFromDb);
                _unitOfWork.Save();
            }
            else
            {
                // add to cart
                TempData["success"] = "Product added to cart successfully!";
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Save();

                // session 
                HttpContext.Session.SetInt32(StaticDetails.SessionCart, _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId).Count());
            }
            
            
            

            return RedirectToAction(nameof(Index));
        }




        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
