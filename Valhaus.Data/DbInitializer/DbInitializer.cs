using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Valhaus.Data.Data;
using Valhaus.Models.Models;
using Valhaus.Utils;

namespace Valhaus.Data.DbInitializer;
 public class DbInitializer : IDbInitializer
 {

    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly ApplicationDbContext _db;

    public DbInitializer(
        UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager,
        ApplicationDbContext db)
    {
        _roleManager = roleManager;
        _userManager = userManager;
        _db = db;
    }


    public void Initialize()
    {


        //migrations if they are not applied
        try
        {
            if (_db.Database.GetPendingMigrations().Count() > 0)
            {
                _db.Database.Migrate();  // Apply all missing migrations now
            }
        }
        catch (Exception ex) { }



        //create roles if they are not created
        if (!_roleManager.RoleExistsAsync(StaticDetails.Role_Customer).GetAwaiter().GetResult())
        {
            _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Customer)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Employee)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Admin)).GetAwaiter().GetResult();
            _roleManager.CreateAsync(new IdentityRole(StaticDetails.Role_Company)).GetAwaiter().GetResult();


            //if roles are not created, then we will create admin user as well
            var adminEmail = "adminvalhaus@gmail.com";

            // Check if admin user already exists
            var adminUser = _userManager.FindByEmailAsync(adminEmail).GetAwaiter().GetResult();

            if (adminUser is null)
            {
                //Create admin user
                var createResult = _userManager.CreateAsync(new ApplicationUser
                {
                    EmailConfirmed = true,
                    UserName = adminEmail,
                    Email = adminEmail,
                    Name = "Aboubakr Jelloulat",
                    PhoneNumber = "+47 912 34 567",
                    StreetAddress = "Karl Johans gate 15",
                    City = "Oslo",
                    State = "Oslo",
                    PostalCode = "0154"
                }, "Valhaus@Admin2026").GetAwaiter().GetResult();

                //If creation succeeded, assign Admin role
                if (createResult.Succeeded)
                {
                    _userManager.AddToRoleAsync(
                        _userManager.FindByEmailAsync(adminEmail).GetAwaiter().GetResult(),
                        StaticDetails.Role_Admin
                    ).GetAwaiter().GetResult();
                }
            }


        }

        return;
    }
 }

