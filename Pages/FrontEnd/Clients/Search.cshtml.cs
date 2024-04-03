using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using NuGet.Packaging;

namespace HotelManagementSystem.Pages
{
    public class ClientSearchModel : FrontEndModel
    {
        private readonly HotelManagementContext _context;

        [BindProperty]
        public string? ClientName { get; set; }

        [BindProperty]
        public string? ClientTel { get; set; }

        [BindProperty]
        public string? ClientTrueId { get; set; }

        public ClientSearchModel(HotelManagementContext context) : base(context)
        {
            _context = context;
        }
        private async Task ClientsSearch(string clientName = null, string clientTel = null, string clientTrueId = null)
        {
            ClientName = clientName;
            ClientTel = clientTel;
            ClientTrueId = clientTrueId;

            Clients = await _context.Clients
                                .Where(c => (string.IsNullOrEmpty(clientName) || c.client_name.Contains(clientName))
                                && (string.IsNullOrEmpty(clientTel) || (c.client_tel != null && c.client_tel.Contains(clientTel)))
                                && (string.IsNullOrEmpty(clientTrueId) || c.client_trueId.Contains(clientTrueId)))
                                .ToListAsync();

        }
        override public async Task<IActionResult> OnGetAsync()
        {
            ReturnUrl ??= $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";

            if (TempData["ClientName"] != null)
            {
                ClientName = (string)TempData["ClientName"];
            }
            if (TempData["ClientTel"] != null)
            {
                ClientTel = (string)TempData["ClientTel"];
            }
            if (TempData["ClientTrueId"] != null)
            {
                ClientTrueId = (string)TempData["ClientTrueId"];
            }

            await ClientsSearch(ClientName, ClientTel, ClientTrueId);

            return Page();
        }
        public async Task<IActionResult> OnPostAsync(string clientName = null, string clientTel = null, string clientTrueId = null)
        {
            ReturnUrl ??= $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";

            await ClientsSearch(clientName, clientTel, clientTrueId);
            if (Clients.Count != 0) TempData["FirstTime"] = null;
            else TempData["FirstTime"] = "无";

            return Page();
        }
    }
}