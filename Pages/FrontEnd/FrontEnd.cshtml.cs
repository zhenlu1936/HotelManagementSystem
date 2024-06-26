﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Pages
{
    public class FrontEndModel : PageModel
    {

        private readonly HotelManagementContext _context;
        public string ReturnUrl { get; set; }
        public IList<Bill> Bills { get; set; } = new List<Bill>();
        public IList<Client> Clients { get; set; } = new List<Client>();

        [BindProperty]
        public BillSearchInputModel BillSearchInput { get; set; } = new BillSearchInputModel();

        [BindProperty]
        public ClientSearchInputModel ClientSearchInput { get; set; } = new ClientSearchInputModel();

        public async Task Clear()
        {
            var billsWithoutClients = _context.Bills
                .Where(b => !b.clients.Any())
                .ToList();

            foreach (var bill in billsWithoutClients)
            {
                var billId = bill.bill_id;
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
                        }
                    }

                    _context.Bills.Remove(bill);
                }
            }

            await _context.SaveChangesAsync();
        }

        [ActivatorUtilitiesConstructor]
        public FrontEndModel(HotelManagementContext context)
        {
            _context = context;
        }

        virtual public async Task<IActionResult> OnGetAsync()
        {
            ReturnUrl ??= $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";
            await Clear();
            Bills = await _context.Bills.ToListAsync();
            Bills = Bills.OrderBy(b => b.bill_id).ToList();

            Clients = await _context.Clients.ToListAsync();
            Clients = Clients.OrderBy(c => c.Billbill_id).ToList();

            return Page();
        }

    }
}