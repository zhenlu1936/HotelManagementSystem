using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewFeatures;


namespace HotelManagementSystem.Pages
{
    public class BillsModel : PageModel
    {
        private readonly ILogger<BillsModel> _logger;
        private readonly HotelManagementContext _context;
        public bool Wrong { get; set; } = false;
        [BindProperty]
        public Bill NewBill { get; set; } = new Bill();
        [BindProperty]
        public List<int> SelectedRoomIds { get; set; }
        public int MaxCapacity { get; set; } = 0;
        public BillsModel(ILogger<BillsModel> logger, HotelManagementContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<JsonResult> OnGetAvailableRoomsAsync(DateTime checkInTime, DateTime checkOutTime)
        {
            var availableRooms = await _context.Rooms
                .Where(room => !room.bills.Any() || room.bills.All(room_bill => room_bill.bill_checkOutTime <= checkInTime || room_bill.bill_checkInTime >= checkOutTime))
                .Select(room => new { Value = room.room_id, Text = room.room_trueId.ToString() })
                .ToListAsync();
            return new JsonResult(availableRooms);
        }
        public async void OnGet()
        {

        }
        public async Task Check()
        {
            if (NewBill.bill_checkInTime >= NewBill.bill_checkOutTime)
            {
                ModelState.AddModelError("NewBill.bill_checkInTime", "入住时间应早于退房时间");
                Wrong = true;
            }
            if (NewBill.rooms.Count == 0)
            {
                ModelState.AddModelError("SelectedRoomIds", "预订房间不得为空");
                Wrong = true;
            }
            if (MaxCapacity < NewBill.bill_people)
            {
                ModelState.AddModelError("NewBill.bill_people", "人数超出房间总容量");
                Wrong = true;
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var selectedRooms = await _context.Rooms.Where(r => SelectedRoomIds.Contains(r.room_id)).Include(room => room.roomclass).ToListAsync();
            foreach (var room in selectedRooms)
            {
                room.bills.Add(NewBill); // 关联房间到账单
                NewBill.rooms.Add(room);
                NewBill.bill_price += room.roomclass.class_price;
                MaxCapacity += room.roomclass.class_capacity;
            }

            await Check();
            if (Wrong)
            {
                return Page();
            }

            NewBill.bill_bookTime = DateTime.Now;
            _context.Bills.Add(NewBill);
            await _context.SaveChangesAsync();

            TempData["people"] = NewBill.bill_people;
            TempData["id"] = NewBill.bill_id;
            return RedirectToPage("/FrontEnd/Clients/Create");
        }
    }
}