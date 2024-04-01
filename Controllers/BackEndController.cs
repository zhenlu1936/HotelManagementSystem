using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace HotelManagementSystem.Controllers
{

    public class BackEndController : Controller
    {
        private readonly HotelManagementContext _context;

        public BackEndController(HotelManagementContext context)
        {
            _context = context;
        }


        [Authorize(Policy = "经理或管理员")]
        [HttpPost]
        public async Task<IActionResult> RoomDelete(int roomId)
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

            return RedirectToPage("/BackEnd/Manage");
        }

        [Authorize(Policy = "经理或管理员")]
        [HttpPost]
        public async Task<IActionResult> ClassDelete(int classId)
        {
            var Class = await _context.Classes.FindAsync(classId);
            if (Class != null)
            {
                _context.Classes.Remove(Class);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/BackEnd/Manage");
        }

        [HttpPost]
        public async Task<IActionResult> RoomDetail(int roomId)
        {
            var Room = await _context.Rooms.FindAsync(roomId);
            if (Room != null)
            {
                var DetailedBills = await _context.Bills
                .Include(bill => bill.rooms)
                .Where(bill => bill.rooms
                .Any(room => room.room_id == roomId))
                .ToListAsync();

            }

            return RedirectToPage("/BackEnd/Manage");
        }
    }
}
