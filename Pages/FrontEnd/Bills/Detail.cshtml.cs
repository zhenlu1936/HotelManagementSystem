using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace HotelManagementSystem.Pages
{
    public class BillDetailModel : PageModel
    {
        private readonly ILogger<BackEndModel> _logger;
        private readonly HotelManagementContext _context;
        public IList<Room> DetailRooms { get; set; } = new List<Room>();
        public Bill Bill { get; set; } = new Bill();
        public IList<Client> DetailClients { get; set; } = new List<Client>();
        public BillDetailModel(ILogger<BackEndModel> logger, HotelManagementContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task OnGetAsync(int billId)
        {
            Bill = await _context.Bills
            .Include(r => r.rooms)
                .ThenInclude(room => room.roomclass)
            .Include(r => r.clients)
            .FirstOrDefaultAsync(r => r.bill_id == billId);
            DetailRooms = (IList<Room>)Bill.rooms;
            DetailClients = (IList<Client>)Bill.clients;

        }

    }
}