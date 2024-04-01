using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace HotelManagementSystem.Pages
{
    public class ClientSearchModel : FrontEndModel
    {
        private readonly HotelManagementContext _context;
        public ClientSearchModel(HotelManagementContext context) : base(context)
        {
            _context = context;
            ClientName = "张三";
        }
        override public async Task<IActionResult> OnGetAsync()
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