using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace HotelManagementSystem.Pages
{
    public class BillSearchModel : PageModel
    {
        private readonly HotelManagementContext _context;
        public List<Bill> Bills { get; set; } = new List<Bill>();

        [BindProperty]
        public int RoomTrueId { get; set; } = 0;
        public string ReturnUrl { get; set; }
        public BillSearchModel(HotelManagementContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            ReturnUrl ??= $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";
            if (TempData["RoomId"] != null)
            {
                RoomTrueId = Convert.ToInt32(TempData["RoomId"]);
                Bills = await _context.Bills
                                        .Where(x => x.rooms.Any(r => r.room_floor * 100 + r.room_number == RoomTrueId))
                                        .ToListAsync();
                if (Bills.Count != 0) TempData["FirstTime"] = null;
                else TempData["FirstTime"] = "无";
                TempData["RoomId"] = null;
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int roomTrueId)
        {
            ReturnUrl ??= $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";
            RoomTrueId = roomTrueId;
            Bills = await _context.Bills
                                    .Where(x => x.rooms.Any(r => r.room_floor * 100 + r.room_number == RoomTrueId))
                                    .ToListAsync();
            if (Bills.Count != 0) TempData["FirstTime"] = null;
            else TempData["FirstTime"] = "无";
            return Page();
        }
    }
}