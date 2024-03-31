using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Mysqlx;

namespace HotelManagementSystem.Pages
{
    public class RoomModel : PageModel
    {
        private readonly ILogger<RoomModel> _logger;
        private readonly HotelManagementContext _context;
        public int People { get; set; }
        public int id { get; set; }
        [BindProperty]
        public Room NewRoom { get; set; } = new Room();
        public SelectList ClassOptions { get; set; }
        public RoomModel(ILogger<RoomModel> logger, HotelManagementContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task Initialize()
        {
            ClassOptions = new SelectList(await _context.Classes.ToListAsync(), "class_id", "class_name");
        }
        public async Task<IActionResult> OnGetAsync()
        {
            await Initialize();
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await Initialize();
                return Page();
            }

            var existingFloor = await _context.Rooms.FirstOrDefaultAsync(r => r.room_floor == NewRoom.room_floor);
            var existingNumber = await _context.Rooms.FirstOrDefaultAsync(r => r.room_number == NewRoom.room_number);
            if (existingFloor != null && existingNumber != null)
            {
                ModelState.AddModelError("NewRoom.room_number", "房间编号已存在");
                await Initialize();
                return Page();
            }

            _context.Rooms.Add(NewRoom);
            await _context.SaveChangesAsync();
            await Initialize();
            TempData["SuccessMessage"] = "提交成功！";
            return Page();

        }
    }
}