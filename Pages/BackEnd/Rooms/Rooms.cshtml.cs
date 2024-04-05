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
        public async Task<IActionResult> OnGetAsync(int? roomId)
        {
            if (roomId != null)
            {
                NewRoom = await _context.Rooms.FindAsync(roomId);
                HttpContext.Session.SetInt32("FormerId", (int)roomId);
            }
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

            int? FormerId = HttpContext.Session.GetInt32("FormerId");
            var existingRoom = await _context.Rooms.FirstOrDefaultAsync(r => r.room_floor == NewRoom.room_floor && r.room_number == NewRoom.room_number);
            if (existingRoom != null && (FormerId == null || existingRoom.room_id != FormerId))
            {
                ModelState.AddModelError("NewRoom.room_number", "房间已存在");
                await Initialize();
                return Page();
            }

            if (FormerId == null)
                _context.Rooms.Add(NewRoom);
            else
            {
                var FormerRoom = await _context.Rooms.FindAsync(FormerId);
                FormerRoom.room_floor = NewRoom.room_floor;
                FormerRoom.room_number = NewRoom.room_number;
                FormerRoom.Classclass_id = NewRoom.Classclass_id;
            }
            await _context.SaveChangesAsync();
            await Initialize();
            TempData["SuccessMessage"] = "提交成功！";
            HttpContext.Session.Remove("FormerId");
            return Redirect("/BackEnd/Manage");

        }
    }
}