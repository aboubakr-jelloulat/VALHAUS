using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Stripe.BillingPortal;
using Stripe.Checkout;
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

        [BindProperty]
        public ShoppingCartVM ShoppingCartVM { get; set; }
        /*
            Automatically taking data from the HTTP request and putting it into C# objects.
                
            [BindProperty]
            public ShoppingCartVM ShoppingCartVM { get; set; }
                Then you do NOT need this anymore:
                public IActionResult SummaryPOST(ShoppingCartVM shoppingCartVM)

            ASP.NET Core will automatically take data from the HTTP request
            (form fields, route values, etc.) and put it into ShoppingCartVM.
            
            so you can use :
                [HttpPost]
                public IActionResult SummaryPOST()
                {
                    // Data is already here
                    var cart = ShoppingCartVM;
                }

         */
        public CartController(IUnitOfWork unitOfWork, IEmailSender emailSender)
        {
            _unitOfWork = unitOfWork;
        }

        private static double _calculatePriceBasedOnQuantity(ShoppingCart shoppingCart)
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
                includeProperties: "Product"), OrderHeader = new()
            };



            foreach(var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = _calculatePriceBasedOnQuantity(cart);

                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartVM);


        }

        public IActionResult Summary()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM = new()
            {
                ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == userId,
                    includeProperties: "Product"),
                OrderHeader = new()
            };

            // null check
            var applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

            if (applicationUser == null)
            {
                return RedirectToAction("Index", "Home");
            }

            ShoppingCartVM.OrderHeader.ApplicationUser = applicationUser;
            ShoppingCartVM.OrderHeader.Name = applicationUser.Name;
            ShoppingCartVM.OrderHeader.PhoneNumber = applicationUser.PhoneNumber;
            ShoppingCartVM.OrderHeader.StreetAddress = applicationUser.StreetAddress;
            ShoppingCartVM.OrderHeader.City = applicationUser.City;
            ShoppingCartVM.OrderHeader.State = applicationUser.State;
            ShoppingCartVM.OrderHeader.PostalCode = applicationUser.PostalCode;

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = _calculatePriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            return View(ShoppingCartVM);
        }

        [HttpPost]
        [ActionName("Summary")]
        public IActionResult SummaryPOST()
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var userId = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier).Value;

            ShoppingCartVM.ShoppingCartList = _unitOfWork.ShoppingCart.GetAll(
                u => u.ApplicationUserId == userId,
                includeProperties: "Product"
            );

            ShoppingCartVM.OrderHeader.OrderDate = DateTime.Now;
            ShoppingCartVM.OrderHeader.ApplicationUserId = userId;

            ApplicationUser applicationUser = _unitOfWork.ApplicationUser.Get(u => u.Id == userId);

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                cart.Price = _calculatePriceBasedOnQuantity(cart);
                ShoppingCartVM.OrderHeader.OrderTotal += (cart.Price * cart.Count);
            }

            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                ShoppingCartVM.OrderHeader.PaymentStatus = StaticDetails.PaymentStatusPending;
                ShoppingCartVM.OrderHeader.OrderStatus = StaticDetails.StatusPending;
            }
            else
            {
                ShoppingCartVM.OrderHeader.PaymentStatus = StaticDetails.PaymentStatusDelayedPayment;
                ShoppingCartVM.OrderHeader.OrderStatus = StaticDetails.StatusApproved;
            }

            _unitOfWork.OrderHeader.Add(ShoppingCartVM.OrderHeader);
            _unitOfWork.Save();

            foreach (var cart in ShoppingCartVM.ShoppingCartList)
            {
                OrderDetail orderDetail = new()
                {
                    ProductId = cart.ProductId,
                    OrderHeaderId = ShoppingCartVM.OrderHeader.Id,
                    Price = cart.Price,
                    Count = cart.Count
                };
                _unitOfWork.OrderDetail.Add(orderDetail);
                _unitOfWork.Save();
            }

            if (applicationUser.CompanyId.GetValueOrDefault() == 0)
            {
                // Stripe logic
                var domain = Request.Scheme + "://" + Request.Host.Value + "/";
                var options = new Stripe.Checkout.SessionCreateOptions
                {
                    SuccessUrl = domain + $"customer/cart/OrderConfirmation?id={ShoppingCartVM.OrderHeader.Id}",
                    CancelUrl = domain + "customer/cart/index",
                    LineItems = new List<Stripe.Checkout.SessionLineItemOptions>(),
                    Mode = "payment",
                };

                foreach (var item in ShoppingCartVM.ShoppingCartList)
                {
                    var sessionLineItem = new Stripe.Checkout.SessionLineItemOptions
                    {
                        PriceData = new Stripe.Checkout.SessionLineItemPriceDataOptions
                        {
                            UnitAmount = (long)(item.Price * 100),
                            Currency = "usd",
                            ProductData = new Stripe.Checkout.SessionLineItemPriceDataProductDataOptions
                            {
                                Name = item.Product.Title
                            }
                        },
                        Quantity = item.Count
                    };
                    options.LineItems.Add(sessionLineItem);
                }

                var service = new Stripe.Checkout.SessionService();
                Stripe.Checkout.Session session = service.Create(options);
                _unitOfWork.OrderHeader.UpdateStripePaymentID(
                    ShoppingCartVM.OrderHeader.Id,
                    session.Id,
                    session.PaymentIntentId
                );
                _unitOfWork.Save();

                Response.Headers.Add("Location", session.Url);
                return new StatusCodeResult(303);
            }

            return RedirectToAction(nameof(OrderConfirmation), new { id = ShoppingCartVM.OrderHeader.Id });
        }




        public IActionResult OrderConfirmation(int id)
        {

            return View(id);
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
