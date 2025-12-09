using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Messaging;
using Valhaus.Data.Repository.IRepository;
using Valhaus.Models.Models;
using Valhaus.Utils;

namespace VALHAUS.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = StaticDetails.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Company> companiesList = _unitOfWork.Companies.GetAll().ToList();

            return View(companiesList);
        }

        public ActionResult Upsert(int? id)
        {
            if (id is null or 0)
            {
                return View(new Company()); // Create
            }
            Company companyobj = _unitOfWork.Companies.Get(u => u.Id == id); // update
            return View(companyobj);
        }

        [HttpPost]
        public IActionResult Upsert(Company CompanyObj)
        {
            if (!ModelState.IsValid)
                return View(CompanyObj);
            
            if (CompanyObj.Id == 0)
            {
                _unitOfWork.Companies.Add(CompanyObj);
                TempData["success"] = "Company created successfully!";
            }
            else
            {
                _unitOfWork.Companies.Update(CompanyObj);
                TempData["success"] = "Company updated successfully!";
            }

            _unitOfWork.Save();
            return RedirectToAction("Index");
        }

        #region API CALLS
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> companies = _unitOfWork.Companies.GetAll().ToList();

            return Json(new {data = companies});
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var companyToBeDeleted = _unitOfWork.Companies.Get(u => u.Id == id);
            
            if(companyToBeDeleted is null)
            {
                return Json(new {success = false, message = "Error while deleting Company" });
            }

            _unitOfWork.Companies.Remove(companyToBeDeleted);
            _unitOfWork.Save();
            return Json(new {success = true, message = "Company deleted successfully!" });
        }

        #endregion

    }
}
