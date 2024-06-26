﻿using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Pages
{
    public class BackEndModel : PageModel
    {
        private readonly ILogger<BackEndModel> _logger;
        private readonly HotelManagementContext _context;
        public IList<Room> Rooms { get; set; } = new List<Room>();
        public IList<Class> Classes { get; set; } = new List<Class>();
        public BackEndModel(ILogger<BackEndModel> logger, HotelManagementContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task OnGetAsync()
        {
            Classes = await _context.Classes.ToListAsync();
            Classes = Classes.OrderBy(c => c.class_price).ToList();

            Rooms = await _context.Rooms
            .Include(room => room.roomclass)
            .Include(room => room.bills)
            .ToListAsync();
            Rooms = Rooms.OrderBy(r => r.room_floor * 100 + r.room_number).ToList();
        }

    }
}