using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

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

        [Authorize(Policy = "经理或管理员")]
        [Route("Stuff/Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(string Id)
        {
            var user = await _userManager.GetUserAsync(User);
            var claims = await _userManager.GetClaimsAsync(user);
            var stuffRoleClaim = claims.FirstOrDefault(c => c.Type == "stuff_role");

            var Stuff = await _context.Users.FindAsync(Id);
            if (Stuff != null && (stuffRoleClaim.Value == "管理员" && Stuff.Rolerole_id > 2))
            {
                _context.Users.Remove(Stuff);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Stuff/Manage");
        }

        [Authorize(Policy = "经理或管理员")]
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
