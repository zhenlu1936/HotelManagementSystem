using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HotelManagementSystem.Controllers
{
    public class ClientsController : Controller
    {
        private readonly HotelManagementContext _context;

        public ClientsController(HotelManagementContext context)
        {
            _context = context;
        }

        [Route("Clients/Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(int clientID)
        {
            var client = await _context.Clients.FindAsync(clientID);
            if (client != null)
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/FrontEnd");
        }

    }
}
