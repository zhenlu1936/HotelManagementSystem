using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace HotelManagementSystem.Pages
{
    public class BillSearchModel : FrontEndModel
    {
        private readonly HotelManagementContext _context;

        [BindProperty]
        public int? RoomTrueId { get; set; }

        [BindProperty]
        public DateTime? CheckInTime { get; set; }

        [BindProperty]
        public DateTime? CheckOutTime { get; set; }
        public BillSearchModel(HotelManagementContext context) : base(context)
        {
            _context = context;
        }
        private async Task ClientsSearch(int? roomId, DateTime? checkInTime, DateTime? checkOutTime)
        {
            RoomTrueId = roomId;
            CheckInTime = checkInTime;
            CheckOutTime = CheckOutTime;

            Bills = await _context.Bills
                                .Where(b => (RoomTrueId == null || (b.rooms.Any(r => r.room_floor * 100 + r.room_number == RoomTrueId)))
                                && (CheckInTime == null || b.bill_checkInTime == CheckInTime)
                                && (CheckOutTime == null || b.bill_checkOutTime == CheckOutTime))
                                .ToListAsync();
        }
        override public async Task<IActionResult> OnGetAsync()
        {
            ReturnUrl ??= $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";

            if (TempData["RoomId"] != null)
            {
                RoomTrueId = Convert.ToInt32(TempData["RoomId"]);
            }
            if (TempData["CheckInTime"] != null)
            {
                CheckInTime = Convert.ToDateTime(TempData["CheckInTime"]);
            }
            if (TempData["CheckOutTime"] != null)
            {
                CheckOutTime = Convert.ToDateTime(TempData["CheckOutTime"]);
            }

            await ClientsSearch(RoomTrueId, CheckInTime, CheckOutTime);
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(int? roomTrueId, DateTime? checkInTime, DateTime? checkOutTime)
        {
            ReturnUrl ??= $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";
            await ClientsSearch(roomTrueId, checkInTime, checkOutTime);

            if (Bills.Count != 0) TempData["FirstTime"] = null;
            else TempData["FirstTime"] = "无";
            return Page();
        }
    }
}