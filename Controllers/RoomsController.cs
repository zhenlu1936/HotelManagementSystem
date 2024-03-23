using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Controllers
{

    public class RoomsController : Controller
    {
        private readonly HotelManagementContext _context;

        public RoomsController(HotelManagementContext context)
        {
            _context = context;
        }

        [Route("Rooms/Delete")]
        [HttpPost]
        public async Task<IActionResult> Delete(int roomId)
        {
            var room = await _context.Rooms.FindAsync(roomId);
            var billsContainingRoom = await _context.Bills
                .Include(bill => bill.rooms)
                .Where(bill => bill.rooms.Any(room => room.room_id == roomId))
                .ToListAsync();

            if (room != null)
            {
                foreach (var bill in billsContainingRoom)
                {
                    // 查找并移除特定的Bill
                    var roomToRemove = bill.rooms.FirstOrDefault(room => room.room_id == roomId);
                    if (roomToRemove != null)
                    {
                        bill.rooms.Remove(roomToRemove);
                    }
                    if (bill.rooms == null)
                    {
                        bill.rooms = new List<Room>();
                    }
                }

                _context.Rooms.Remove(room);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/BackEnd");
        }

    }
}
