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
        public ClientSearchModel(HotelManagementContext context) : base(context)
        {
            _context = context;
        }
        public void ClientSearchRender(ClientSearchInputModel input)
        {
            if (TempData["ClientName"] != null)
                TempData["ClientName"] = input.ClientName;
            if (TempData["ClientTel"] != null)
                TempData["ClientTel"] = input.ClientTel;
            if (TempData["ClientTrueId"] != null)
                TempData["ClientTrueId"] = input.ClientTrueId;
            TempData.Keep("ClientName");
            TempData.Keep("ClientTel");
            TempData.Keep("ClientTrueId");
        }
        private async Task ClientsSearch(ClientSearchInputModel input)
        {
            ClientSearchInput = input;
            TempData.Remove("ClientSearchNoInput");
            if (ClientSearchInput.GetType().GetProperties().All(p => p.GetValue(ClientSearchInput) == null))
            {
                TempData["ClientSearchNoInput"] = "Yes";
            }

            else Clients = await _context.Clients
                                       .Where(c => (string.IsNullOrEmpty(ClientSearchInput.ClientName) || c.client_name.Contains(ClientSearchInput.ClientName))
                                       && (string.IsNullOrEmpty(ClientSearchInput.ClientTel) || (c.client_tel != null && c.client_tel.Contains(ClientSearchInput.ClientTel)))
                                       && (string.IsNullOrEmpty(ClientSearchInput.ClientTrueId) || c.client_trueId.Contains(ClientSearchInput.ClientTrueId)))
                                       .ToListAsync();

        }
        override public async Task<IActionResult> OnGetAsync()
        {
            ReturnUrl ??= $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";

            if (TempData["ClientName"] != null)
            {
                ClientSearchInput.ClientName = (string)TempData["ClientName"];
            }
            if (TempData["ClientTel"] != null)
            {
                ClientSearchInput.ClientTel = (string)TempData["ClientTel"];
            }
            if (TempData["ClientTrueId"] != null)
            {
                ClientSearchInput.ClientTrueId = (string)TempData["ClientTrueId"];
            }

            TempData["ClientSearchFirstTime"] = "Yes";
            TempData.Keep("ClientSearchFirstTime");

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            ReturnUrl ??= $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";

            await ClientsSearch(ClientSearchInput);
            if (TempData["ClientSearchFirstTime"] != null)
            {
                if ((string)TempData["ClientSearchFirstTime"] == "Yes")
                {
                    TempData["ClientSearchFirstTime"] = "No";
                }
            }
            else TempData["ClientSearchFirstTime"] = "Yes";
            TempData.Keep("ClientSearchFirstTime");

            ClientSearchRender(ClientSearchInput);

            return Page();
        }
    }
}