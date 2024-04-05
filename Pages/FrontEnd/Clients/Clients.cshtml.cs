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
        public int BillId { get; set; }

        [BindProperty]
        public IList<Client> NewClients { get; set; } = new List<Client>();
        public ClientModel(ILogger<ClientModel> logger, HotelManagementContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task Initialize()
        {
            var NewBill = await _context.Bills.Include(b => b.clients).FirstOrDefaultAsync(b => b.bill_id == BillId);
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
            if (clientId == null) //如果不是针对单个顾客的更改（即通过订单修改进入顾客修改）
            {
                People = (int)HttpContext.Session.GetInt32("People");
                BillId = (int)HttpContext.Session.GetInt32("BillId");
                Initialized = true;
                await Initialize();
            }
            else //如果是针对单个顾客的更改
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
                BillId = (int)HttpContext.Session.GetInt32("BillId");
                var NewBill = await _context.Bills.Include(b => b.clients).FirstOrDefaultAsync(b => b.bill_id == BillId);
                foreach (var client in NewBill.clients)
                {
                    _context.Clients.Remove(client); // 标记为删除状态
                }
                NewBill.clients.Clear();

                foreach (var NewClient in NewClients)
                {
                    NewClient.Billbill_id = BillId;
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
            HttpContext.Session.Remove("People");
            HttpContext.Session.Remove("BillId");
            HttpContext.Session.Remove("FormerId");

            return RedirectToPage("/FrontEnd/Manage");
        }
    }
}