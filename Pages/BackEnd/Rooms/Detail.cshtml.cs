using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace HotelManagementSystem.Pages
{
    public class RoomDetailModel : PageModel
    {
        private readonly ILogger<BackEndModel> _logger;
        private readonly HotelManagementContext _context;
        public Room Room { get; set; } = new Room();
        public Class DetailClass { get; set; } = new Class();
        public IList<Bill> DetailBills { get; set; } = new List<Bill>();
        public IList<Client> DetailClients { get; set; } = new List<Client>();
        public RoomDetailModel(ILogger<BackEndModel> logger, HotelManagementContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task OnGetAsync(int roomId)
        {
            Room = await _context.Rooms
            .Include(r => r.roomclass)
            .Include(r => r.bills)
            .FirstOrDefaultAsync(r => r.room_id == roomId);
            DetailClass = Room.roomclass;
            DetailBills = (IList<Bill>)Room.bills;

            foreach (var bill in DetailBills)
            {
                var clients = await _context.Clients
                .Where(c => c.Billbill_id == bill.bill_id)
                .ToListAsync();

                DetailClients.AddRange(clients);
            }
        }

    }
}