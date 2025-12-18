using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Valhaus.Data.Repository.IRepository;
using Valhaus.Models.Models;
using Valhaus.Utils;

namespace VALHAUS.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            return View();
        }

        #region API CALLS

        [HttpGet]
        //  JavaScript (order.js) send the status
        public IActionResult GetAll(string status)
        {
            IEnumerable<OrderHeader> orderHeaders;

            // Get all orders with user information
            orderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser");

            // Filter status
            switch (status)
            {
                case "pending":
                    orderHeaders = orderHeaders.Where(u => u.PaymentStatus == StaticDetails.PaymentStatusDelayedPayment);
                    break;
                case "inprocess":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == StaticDetails.StatusInProcess);
                    break;
                case "completed":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == StaticDetails.StatusShipped);
                    break;
                case "approved":
                    orderHeaders = orderHeaders.Where(u => u.OrderStatus == StaticDetails.StatusApproved);
                    break;
                default:
                    // Return all orders
                    break;
            }

            return Json(new { data = orderHeaders });
        }

        #endregion
    }
}
