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
        public int click = 0;
        public List<IdentityRole> AllRoles { get; set; }
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

    }
}
