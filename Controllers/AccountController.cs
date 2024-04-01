using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Controllers
{

    public class AccountController : Controller
    {
        private readonly UserContext _context;
        private readonly UserManager<HotelStuff> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public AccountController(RoleManager<IdentityRole> roleManager, UserContext context, UserManager<HotelStuff> userManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [Route("Account/Login")]
        public IActionResult Login(string returnUrl = null)
        {
            TempData["ReturnUrl"] = returnUrl;
            return RedirectToPage("/Identity/Login");
        }

        [Route("Account/AccessDenied")]
        public IActionResult AccessDenied(string returnUrl = null)
        {
            TempData["Denied"] = "您没有足够的权限！";
            var path = returnUrl.Trim('/');
            var segments = path.Split('/');
            var firstSegment = segments.FirstOrDefault();
            firstSegment = "/" + firstSegment + "/Manage";

            return Redirect(firstSegment);
        }

    }
}
