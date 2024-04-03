using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

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

        public async Task Initialize()
        {
            var NewBill = await _context.Bills.Include(b => b.clients).FirstOrDefaultAsync(b => b.bill_id == id);
            foreach (var client in NewBill.clients)
            {
                if (NewClients.Count < People)
                    NewClients.Add(client);
                else break;
            }

            for (int i = NewClients.Count; i < People; i++)
            {
                NewClients.Add(new Client());
            }

        }
        public async Task<IActionResult> OnGetAsync(int? clientId)
        {
            if (clientId == null)
            {
                People = (int)HttpContext.Session.GetInt32("People");
                id = (int)HttpContext.Session.GetInt32("Id");
                Initialized = true;
                await Initialize();
            }
            else
            {
                People = 1;
                var waitClient = await _context.Clients.FindAsync(clientId);
                NewClients.Add(waitClient);
                HttpContext.Session.SetInt32("FormerId", (int)clientId);
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await Initialize();
                return Page();
            }

            int? FormerId = HttpContext.Session.GetInt32("FormerId");
            if (FormerId == null)
            {
                id = (int)HttpContext.Session.GetInt32("Id");
                var NewBill = await _context.Bills.Include(b => b.clients).FirstOrDefaultAsync(b => b.bill_id == id);
                foreach (var client in NewBill.clients)
                {
                    _context.Clients.Remove(client); // 标记为删除状态
                }
                NewBill.clients.Clear();

                foreach (var NewClient in NewClients)
                {
                    NewClient.Billbill_id = id;
                    NewBill.clients.Add(NewClient);
                }
            }
            else
            {
                var FormerClient = await _context.Clients.FindAsync(FormerId);
                foreach (var NewClient in NewClients)
                {
                    FormerClient.client_tel = NewClient.client_tel;
                    FormerClient.client_name = NewClient.client_name;
                    FormerClient.client_trueId = NewClient.client_trueId;
                }
            }

            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "提交成功！";
            HttpContext.Session.SetInt32("People", 0);
            HttpContext.Session.SetInt32("Id", 0);
            HttpContext.Session.SetInt32("FormerId", 0);

            return RedirectToPage("/FrontEnd/Manage");
        }
    }
}