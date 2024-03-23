using Microsoft.AspNetCore.Mvc;

namespace HotelManagementSystem.Controllers
{

    public class ClassesController : Controller
    {
        private readonly HotelManagementContext _context;

        public ClassesController(HotelManagementContext context)
        {
            _context = context;
        }

        [Route("Classes/Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(int classId)
        {
            var Class = await _context.Classes.FindAsync(classId);
            if (Class != null)
            {
                _context.Classes.Remove(Class);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/BackEnd");
        }

    }
}
