using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Valhaus.Data.Repository.IRepository;
using Valhaus.Models.Models;

namespace VALHAUS.Areas.Admin.Controllers
{
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
        public IActionResult GetAll()
        {
            List<OrderHeader> orderHeaders = _unitOfWork.OrderHeader.GetAll(includeProperties: "ApplicationUser").ToList();

            return Json(new { data = orderHeaders });
        }
        
        #endregion

    }
}
