using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Controllers
{

    public class StuffController : Controller
    {
        private readonly UserContext _context;
        private readonly UserManager<HotelStuff> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public int click = 0;
        public List<IdentityRole> AllRoles { get; set; }
        public StuffController(RoleManager<IdentityRole> roleManager, UserContext context, UserManager<HotelStuff> userManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }

        [Route("Stuff/Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(string Id)
        {
            var Stuff = await _context.Users.FindAsync(Id);
            if (Stuff != null)
            {
                _context.Users.Remove(Stuff);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Stuff/Manage");
        }

        [Route("Stuff/ChangeRole")]
        [HttpPost]
        public async Task<IActionResult> ChangeRole(string Id)
        {
            var user = await _context.Users.FindAsync(Id);
            int num = _context.StuffRoles.Count();
            user.Rolerole_id = (user.Rolerole_id + 1) % (num + 1);
            if (user.Rolerole_id == 0) user.Rolerole_id++;
            if (user.Rolerole_id == 1) user.Rolerole_id++;
            await _context.SaveChangesAsync();

            return RedirectToPage("/Stuff/Manage");
        }
    }
}
