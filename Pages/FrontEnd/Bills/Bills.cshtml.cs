using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using NuGet.Common;
using Microsoft.CodeAnalysis.FlowAnalysis.DataFlow;


namespace HotelManagementSystem.Pages
{
    public class BillsModel : PageModel
    {
        private readonly HotelManagementContext _context;
        public bool Wrong { get; set; } = false;
        public int BillId { get; set; }

        [BindProperty]
        public Bill NewBill { get; set; }
        public Bill FormerBill { get; set; }

        [BindProperty]
        public List<int> SelectedRoomIds { get; set; } = new List<int>();
        public List<Room> AvailableRooms { get; set; } = new List<Room>();
        public int MaxCapacity { get; set; } = 0;
        public BillsModel(HotelManagementContext context)
        {
            _context = context;
        }

        public async Task<JsonResult> OnGetAvailableRoomsAsync(DateTime checkInTime, DateTime checkOutTime)
        {

            AvailableRooms = await _context.Rooms
                .Where(room => (!room.bills.Any())
                || (room.bills.All(room_bill => room_bill.bill_checkOutTime <= checkInTime || room_bill.bill_checkInTime >= checkOutTime))
                || (NewBill.rooms.Contains(room)))
                .ToListAsync();
            AvailableRooms = AvailableRooms.OrderBy(r => r.room_trueId).ToList();

            var availableRooms = AvailableRooms.Select(room => new { Value = room.room_id, Text = room.room_trueId.ToString() });
            return new JsonResult(availableRooms);
        }

        public async Task IfFormerBill(int? billId)
        {
            if (billId != null)
            {
                HttpContext.Session.SetInt32("FormerBillId", (int)billId);
                NewBill = await _context.Bills
                .Include(r => r.rooms)
                .FirstOrDefaultAsync(r => r.bill_id == billId);

                foreach (var room in NewBill.rooms)
                {
                    SelectedRoomIds.Add(room.room_id);
                }
            }

            else
                NewBill = new Bill { bill_people = 1, bill_checkInTime = DateTime.Today, bill_checkOutTime = DateTime.Today.AddDays(1) };
        }
        public async Task<IActionResult> OnGetAsync(int? billId)
        {
            await IfFormerBill(billId);

            await OnGetAvailableRoomsAsync(NewBill.bill_checkInTime, NewBill.bill_checkOutTime);

            return Page();
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

            int billId = (int)HttpContext.Session.GetInt32("FormerBillId");
            bool exists = await _context.Bills.AnyAsync(b => b.bill_id == billId);
            var selectedRooms = await _context.Rooms.Where(r => SelectedRoomIds.Contains(r.room_id)).Include(room => room.roomclass).ToListAsync();

            if (exists)
            {
                FormerBill = await _context.Bills.Include(r => r.rooms).FirstOrDefaultAsync(b => b.bill_id == billId);
                FormerBill.bill_people = NewBill.bill_people;
                FormerBill.bill_checkInTime = NewBill.bill_checkInTime;
                FormerBill.bill_checkOutTime = NewBill.bill_checkOutTime;
                FormerBill.bill_price = 0;
                FormerBill.rooms.Clear();

                foreach (var room in selectedRooms)
                {
                    room.bills.Add(NewBill); // 关联房间到账单
                    FormerBill.rooms.Add(room);
                    FormerBill.bill_price += room.roomclass.class_price;
                    MaxCapacity += room.roomclass.class_capacity;
                }

                NewBill = FormerBill;
            }
            else
            {
                foreach (var room in selectedRooms)
                {
                    room.bills.Add(NewBill); // 关联房间到账单
                    NewBill.rooms.Add(room);
                    NewBill.bill_price += room.roomclass.class_price;
                    MaxCapacity += room.roomclass.class_capacity;
                }
                NewBill.bill_bookTime = DateTime.Now;
            }

            await Check();

            if (Wrong)
            {
                await OnGetAvailableRoomsAsync(NewBill.bill_checkInTime, NewBill.bill_checkOutTime);
                await IfFormerBill(billId);
                return Page();
            }

            if (!exists)
            {
                _context.Bills.Add(NewBill);
            }

            await _context.SaveChangesAsync();
            HttpContext.Session.SetInt32("People", NewBill.bill_people);
            HttpContext.Session.SetInt32("Id", NewBill.bill_id);
            HttpContext.Session.SetInt32("FormerBillId", 0);

            return RedirectToPage("/FrontEnd/Clients/Create");
        }
    }
}