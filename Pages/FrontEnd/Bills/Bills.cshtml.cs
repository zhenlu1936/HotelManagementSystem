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

        [BindProperty]
        public int? SpecificPrice { get; set; }
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
                || (NewBill != null && NewBill.rooms.Contains(room)))
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

            if (billId != null)
            {
                ViewData["Title"] = "编辑订单";
                ViewData["Text"] = "这里是订单编辑系统。";
                ViewData["Submit"] = "编辑";
            }
            else
            {
                ViewData["Title"] = "创建订单";
                ViewData["Text"] = "这里是订单创建系统。";
                ViewData["Submit"] = "创建";
            }

            TempData.Clear();

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
                ModelState.AddModelError("NewBill.bill_people", "人数超出所选房间总容量");
                Wrong = true;
            }
        }
        public async Task<IActionResult> OnPostAsync()
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            int? billId = HttpContext.Session.GetInt32("FormerBillId");
            var selectedRooms = await _context.Rooms.Where(r => SelectedRoomIds.Contains(r.room_id)).Include(room => room.roomclass).ToListAsync();

            if (billId != null) //编辑已有账单
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

                FormerBill.bill_price *= FormerBill.bill_checkOutTime.Day - FormerBill.bill_checkInTime.Day;
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

                NewBill.bill_price *= NewBill.bill_checkOutTime.Day - NewBill.bill_checkInTime.Day;
                NewBill.bill_bookTime = DateTime.Now;
            }

            await Check();

            if (Wrong)
            {
                await OnGetAvailableRoomsAsync(NewBill.bill_checkInTime, NewBill.bill_checkOutTime);
                await IfFormerBill(billId);
                return Page();
            }

            if (SpecificPrice != null) //若指定价格不为空则向订单填充制定价格
            {
                NewBill.bill_price = (int)SpecificPrice;
            }

            if (billId == null)
            {
                _context.Bills.Add(NewBill);
            }

            await _context.SaveChangesAsync();
            HttpContext.Session.SetInt32("People", NewBill.bill_people);
            HttpContext.Session.SetInt32("BillId", NewBill.bill_id);
            HttpContext.Session.Remove("FormerBillId");

            return RedirectToPage("/FrontEnd/Clients/Clients");
        }
    }
}