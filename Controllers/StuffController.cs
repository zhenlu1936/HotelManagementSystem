using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.Controllers
{

    public class StuffController : Controller
    {
        private readonly UserContext _context;

        public StuffController(UserContext context)
        {
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

            return RedirectToPage("/Stuff");
        }

    }
}
