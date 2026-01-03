using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Valhaus.Data.Data;
using Valhaus.Data.Repository.IRepository;
using Valhaus.Models.Models;
using Valhaus.Utils;

namespace VALHAUS.Areas.Admin.Controllers;

[Area("Admin")]
[Authorize(Roles = StaticDetails.Role_Admin)]
public class UserController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly IUnitOfWork _unitOfWork;
    public UserController(UserManager<IdentityUser> userManager, IUnitOfWork unitOfWork, RoleManager<IdentityRole> roleManager)
    {
        _unitOfWork = unitOfWork;
        _roleManager = roleManager;
        _userManager = userManager;
    }

    public IActionResult Index()
    {
        
        return View();
    }

    #region API CALLS
    [HttpGet]
    public IActionResult GetAll()
    {
        List<ApplicationUser> users = _unitOfWork.ApplicationUser.GetAll(includeProperties: "company").ToList();

       
        foreach (var usr in users)
        {
            usr.Role = _userManager.GetRolesAsync(usr).GetAwaiter().GetResult().FirstOrDefault();

            if (usr.company is null)
            {
                usr.company = new Company { Name = string.Empty };
            }
        }

        return Json(new { data = users });
    }


    [HttpPost]
    public IActionResult LockUnlock([FromBody] string id)
    {

        var objFromDb = _unitOfWork.ApplicationUser.Get(u => u.Id == id);
        if (objFromDb is null)
        {
            return Json(new { success = false, message = "Error while Locking/Unlocking" });
        }

        // Check if the user is currently locked
        if (objFromDb.LockoutEnd is not null && objFromDb.LockoutEnd > DateTime.Now)
        {
            //user is currently locked and we need to unlock them
            objFromDb.LockoutEnd = DateTime.Now;
        }
        else
        {
            objFromDb.LockoutEnd = DateTime.Now.AddYears(1000);
        }
        _unitOfWork.ApplicationUser.Update(objFromDb);
        _unitOfWork.Save();
        return Json(new { success = true, message = "Operation Successful" });
    }


    public IActionResult RoleManagment(string userId)
    {

        return View();
    }


    [HttpPost]
    public IActionResult Delete(string id)
    {
        try
        {
            var user = _unitOfWork.ApplicationUser.Get(u => u.Id == id);

            if (user == null)
            {
                return Json(new
                {
                    success = false,
                    message = "User not found"
                });
            }

            // Prevent deletion of admin users or current user
            if (User.FindFirstValue(ClaimTypes.NameIdentifier) == id)
            {
                return Json(new
                {
                    success = false,
                    message = "You cannot delete your own account"
                });
            }

            // Check if user is an admin (if you want to protect admins)
            var userRoles = _userManager.GetRolesAsync(user).Result;
            if (userRoles.Contains("Admin"))
            {
                return Json(new
                {
                    success = false,
                    message = "Admin users cannot be deleted"
                });
            }

            // Remove user from database
            _unitOfWork.ApplicationUser.Remove(user);
            _unitOfWork.Save();

            return Json(new
            {
                success = true,
                message = "User deleted successfully"
            });
        }
        catch (Exception ex)
        {

            return Json(new
            {
                success = false,
                message = "An error occurred while deleting the user"
            });
        }
    }


    #endregion

}
