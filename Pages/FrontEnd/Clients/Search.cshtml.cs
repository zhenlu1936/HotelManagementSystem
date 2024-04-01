using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace HotelManagementSystem.Pages
{
    public class ClientSearchModel : PageModel
    {
        private readonly HotelManagementContext _context;
        public List<Client> Clients { get; set; } = new List<Client>();

        [BindProperty]
        public string ClientName { get; set; }
        public string ReturnUrl { get; set; }
        public ClientSearchModel(HotelManagementContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            ReturnUrl ??= $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";
            if (TempData["ClientName"] != null)
            {
                ClientName = (string)TempData["ClientName"];
                Clients = await _context.Clients
                                        .Where(c => c.client_name.Contains(ClientName))
                                        .ToListAsync();
                if (Clients.Count != 0) TempData["FirstTime"] = null;
                else TempData["FirstTime"] = "无";
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string clientName)
        {
            ReturnUrl ??= $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";
            ClientName = clientName;
            Clients = await _context.Clients
                                    .Where(c => c.client_name.Contains(ClientName))
                                    .ToListAsync();
            if (Clients.Count != 0) TempData["FirstTime"] = null;
            else TempData["FirstTime"] = "无";
            return Page();
        }
    }
}