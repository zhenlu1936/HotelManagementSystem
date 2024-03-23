using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Pages
{
    public class StuffModel : PageModel
    {
        private readonly ILogger<StuffModel> _logger;
        private readonly UserContext _context;
        public IList<HotelStuff> Stuffs { get; set; } = new List<HotelStuff>();

        public StuffModel(ILogger<StuffModel> logger, UserContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task OnGetAsync()
        {
            Stuffs = await _context.Users.ToListAsync();
        }

    }
}