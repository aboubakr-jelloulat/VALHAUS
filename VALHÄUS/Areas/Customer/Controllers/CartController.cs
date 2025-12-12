using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Valhaus.Data.Repository.IRepository;
using Valhaus.Models.Models;
using Valhaus.Utils;

namespace VALHAUS.Areas.Customer.Controllers
{
    [Area("Customer")]
    [Authorize]
    public class CartController : Controller
    {

        private readonly IUnitOfWork _unitOfWork;

        public ShoppingCartVM ShoppingCartVM { get; set; }
        public CartController(IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
        }

        private double _calculatePriceBasedOnQuantity(ShoppingCart shoppingCart)
        {
            if (shoppingCart.Count <= StaticDetails.PRICE_50)
            {
                return shoppingCart.Product.Price;
            }
            else if (shoppingCart.Count <= StaticDetails.PRICE_100)
            {
                return shoppingCart.Product.Price50;
            }

            return shoppingCart.Product.Price100;
        }

        public IActionResult Index()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                includeProperties: "Product")
            };



            foreach(var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = _calculatePriceBasedOnQuantity(cart);

                ShoppingCartVM.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartVM);


        }

        public IActionResult Summary()
        {
            return View();
        }


        public IActionResult plus(int cartId)
        {
            var DbCart = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            DbCart.Count++;
            _unitOfWork.ShoppingCart.Update(DbCart);
            _unitOfWork.Save();

            return RedirectToAction(nameof(Index));
        }


        public IActionResult minus(int cartId)
        {
            var DbCart = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);
            if (DbCart.Count <= 1)
            {
                // Delete
                _unitOfWork.ShoppingCart.Remove(DbCart);
            }
            else
            {
                DbCart.Count--;
                _unitOfWork.ShoppingCart.Update(DbCart);
            }

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

        public IActionResult remove(int cartId)
        {
            var DbCart = _unitOfWork.ShoppingCart.Get(u => u.Id == cartId);

            _unitOfWork.ShoppingCart.Remove(DbCart);

            _unitOfWork.Save();
            return RedirectToAction(nameof(Index));
        }

    }
}
