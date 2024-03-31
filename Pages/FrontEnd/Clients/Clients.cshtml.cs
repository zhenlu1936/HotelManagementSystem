using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelManagementSystem.Pages
{
    public class ClientModel : PageModel
    {
        private readonly ILogger<ClientModel> _logger;
        private readonly HotelManagementContext _context;
        public bool Initialized { get; set; } = false;
        public int People { get; set; }
        public int id { get; set; }
        [BindProperty]
        public IList<Client> NewClients { get; set; } = new List<Client>();
        public ClientModel(ILogger<ClientModel> logger, HotelManagementContext context)
        {
            _logger = logger;
            _context = context;
        }

        public Task Initialize()
        {
            for (int i = 0; i < People; i++)
            {
                NewClients.Add(new Client());
            }
            return Task.CompletedTask;
        }
        public async Task<IActionResult> OnGetAsync()
        {

            People = (int)TempData["people"];
            id = (int)TempData["id"];
            TempData["people"] = People;
            TempData["id"] = id;
            Initialized = true;
            await Initialize();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            People = (int)TempData["people"];
            id = (int)TempData["id"];
            TempData["people"] = People;
            TempData["id"] = id;
            if (!ModelState.IsValid)
            {
                await Initialize();
                return Page();
            }

            foreach (var NewClient in NewClients)
            {
                NewClient.Billbill_id = id;
                _context.Clients.Add(NewClient);
                var NewBill = await _context.Bills.FindAsync(id);
                NewBill.clients.Add(NewClient);
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "提交成功！";
            return RedirectToPage("/FrontEnd/Manage");
        }
    }
}