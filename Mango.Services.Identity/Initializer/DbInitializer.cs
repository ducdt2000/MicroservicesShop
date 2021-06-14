using IdentityModel;
using Mango.Services.Identity.DbContext;
using Mango.Services.Identity.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Mango.Services.Identity.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly AppDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DbInitializer(AppDbContext context, UserManager<User> user, RoleManager<IdentityRole> role)
        {
            _context = context;
            _userManager = user;
            _roleManager = role;
        }

        public void Initialize()
        {
            if(_roleManager.FindByNameAsync(SD.Admin).Result == null)
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Customer)).GetAwaiter().GetResult();
            }
            else 
            {
                return; 
            }

            User admin = new User()
            {
                UserName = "ducdt@gmail.com",
                Email = "ducdt@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "096969696",
                FirstName="Duc",
                LastName="Do"
            };

            _userManager.CreateAsync(admin, "@Alo123").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(admin, SD.Admin).GetAwaiter().GetResult();

            var temp1 = _userManager.AddClaimsAsync(admin, new Claim[] {
                new Claim(JwtClaimTypes.Name, admin.FirstName + " " + admin.LastName),
                new Claim(JwtClaimTypes.GivenName, admin.FirstName),
                new Claim(JwtClaimTypes.FamilyName, admin.LastName),
                new Claim(JwtClaimTypes.Role, SD.Admin)
            }).Result;

            User customer = new User()
            {
                UserName = "customer@gmail.com",
                Email = "customer@gmail.com",
                EmailConfirmed = true,
                PhoneNumber = "0339696696",
                FirstName = "Duck",
                LastName = "Tran"
            };

            _userManager.CreateAsync(customer, "@Alo123").GetAwaiter().GetResult();
            _userManager.AddToRoleAsync(customer, SD.Customer).GetAwaiter().GetResult();

            var temp2 = _userManager.AddClaimsAsync(customer, new Claim[] {
                new Claim(JwtClaimTypes.Name, customer.FirstName + " " + customer.LastName),
                new Claim(JwtClaimTypes.GivenName, customer.FirstName),
                new Claim(JwtClaimTypes.FamilyName, customer.LastName),
                new Claim(JwtClaimTypes.Role, SD.Customer)
            }).Result;
        }
    }
}
