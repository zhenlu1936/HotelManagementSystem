﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using HotelManagementSystem.Pages;

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

        [HttpPost]
        public async Task<IActionResult> BillDelete(int billId, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
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
                        if (bill.bill_trueCheckInTime <= DateTime.Today
                            && (bill.bill_trueCheckOutTime == null))
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


            return Redirect(returnUrl);
        }

        [HttpPost]
        public async Task<IActionResult> BillCheck(int billId, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            var bill = await _context.Bills.FindAsync(billId);
            if (bill != null)
            {
                if (bill.bill_trueCheckInTime == null)
                    bill.bill_trueCheckInTime = DateTime.Now;
                else
                    bill.bill_trueCheckInTime = null;

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

            return Redirect(returnUrl);
        }

        [HttpPost]
        public async Task<IActionResult> BillOut(int billId, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            var bill = await _context.Bills.FindAsync(billId);
            if (bill != null)
            {
                if (bill.bill_trueCheckOutTime == null)
                    bill.bill_trueCheckOutTime = DateTime.Now;
                else
                    bill.bill_trueCheckOutTime = null;

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

            return Redirect(returnUrl);
        }

        [HttpPost]
        public async Task<IActionResult> BillPay(int billId, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            var bill = await _context.Bills.FindAsync(billId);
            if (bill != null)
            {
                if (bill.bill_payTime == null)
                    bill.bill_payTime = DateTime.Now;
                else
                    bill.bill_payTime = null;

                await _context.SaveChangesAsync();
            }

            return Redirect(returnUrl);
        }

        [HttpPost]
        public async Task<IActionResult> ClientDelete(int clientID, string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            var client = await _context.Clients.FindAsync(clientID);
            if (client != null)
            {
                _context.Clients.Remove(client);
                await _context.SaveChangesAsync();
            }

            return Redirect(returnUrl);
        }

    }
}
