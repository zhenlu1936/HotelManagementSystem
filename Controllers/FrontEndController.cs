using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

public class RoomQueryModel
{
    public DateTime CheckInTime { get; set; }
    public DateTime CheckOutTime { get; set; }
}
namespace HotelManagementSystem.Controllers
{

    public class FrontEndController : Controller
    {
        private readonly HotelManagementContext _context;

        public FrontEndController(HotelManagementContext context)
        {
            _context = context;
        }

        [Route("Bills/Delete")]
        [HttpPost]
        public async Task<IActionResult> BillDelete(int billId)
        {
            var bill = await _context.Bills.FindAsync(billId);
            var roomsContainingBill = await _context.Rooms
                .Include(room => room.bills) // 确保加载Bills集合
                .Where(room => room.bills.Any(bill => bill.bill_id == billId)) // 找出包含特定Bill的Room
                .ToListAsync();

            if (bill != null)
            {
                foreach (var room in roomsContainingBill)
                {
                    // 查找并移除特定的Bill
                    var billToRemove = room.bills.FirstOrDefault(bill => bill.bill_id == billId);
                    if (billToRemove != null)
                    {
                        room.bills.Remove(billToRemove);
                        if (bill.bill_checkInTime <= DateTime.Today
                            && DateTime.Today <= bill.bill_checkOutTime
                            && bill.bill_ifChecked == true)
                        {
                            room.room_ifStayIn = false;
                        }
                    }
                    if (room.bills == null)
                    {
                        room.bills = new List<Bill>();
                    }
                }

                _context.Bills.Remove(bill);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/FrontEnd/Manage");
        }

        [Route("Bills/Check")]
        [HttpPost]
        public async Task<IActionResult> BillCheck(int billId)
        {
            var bill = await _context.Bills.FindAsync(billId);
            if (bill != null)
            {
                bill.bill_ifChecked = !bill.bill_ifChecked;

                if (bill.bill_checkInTime <= DateTime.Today && DateTime.Today <= bill.bill_checkOutTime)
                {
                    var roomsContainingBill = await _context.Rooms
                    .Include(room => room.bills) // 确保加载Bills集合
                    .Where(room => room.bills.Any(bill => bill.bill_id == billId)) // 找出包含特定Bill的Room
                    .ToListAsync();
                    foreach (var room in roomsContainingBill)
                    {
                        room.room_ifStayIn = !room.room_ifStayIn;
                    }
                }
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/FrontEnd/Manage");
        }

        [Route("Client/Delete")]
        [HttpPost]
        public async Task<IActionResult> ClientDelete(int clientID)
        {
            var client = await _context.Clients.FindAsync(clientID);
            if (client != null)
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/FrontEnd/Manage");
        }
    }
}
