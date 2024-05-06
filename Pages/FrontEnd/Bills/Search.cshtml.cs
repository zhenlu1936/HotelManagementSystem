using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Packaging;

namespace HotelManagementSystem.Pages
{
    public class BillSearchModel : FrontEndModel
    {
        public void BillSearchRender(BillSearchInputModel input)
        {
            if (input.RoomTrueId != null)
            {
                TempData["BillSearchRoomId"] = input.RoomTrueId;
            }
            if (input.CheckInTime != null)
            {
                TempData["BillSearchCheckInTime"] = input.CheckInTime;
            }
            if (input.CheckOutTime != null)
            {
                TempData["BillSearchCheckOutTime"] = input.CheckOutTime;
            }
            if (input.IfPaid.HasValue)
            {
                TempData["BillSearchIfPaid"] = input.IfPaid.Value;
            }
            if (input.IfChecked.HasValue)
            {
                TempData["BillSearchIfChecked"] = input.IfChecked.Value;
            }
            if (input.IfOut.HasValue)
            {
                TempData["BillSearchIfOut"] = input.IfOut.Value;
            }
            TempData.Keep("BillSearchRoomId");
            TempData.Keep("BillSearchCheckInTime");
            TempData.Keep("BillSearchCheckOutTime");
            TempData.Keep("BillSearchIfPaid");
            TempData.Keep("BillSearchIfChecked");
            TempData.Keep("BillSearchIfOut");
        }
        private readonly HotelManagementContext _context;

        [BindProperty]
        public Boolean test { get; set; } = false;

        public BillSearchModel(HotelManagementContext context) : base(context)
        {
            _context = context;
        }
        private async Task BillsSearch(BillSearchInputModel input)
        {
            BillSearchInput = input;
            TempData["BillSearchNoInput"] = null;
            if (BillSearchInput.GetType().GetProperties().All(p => p.GetValue(BillSearchInput) == null))
            {
                TempData["BillSearchNoInput"] = "Yes";
            }

            else Bills = await _context.Bills
                                .Where(b => (BillSearchInput.RoomTrueId == null || (b.rooms.Any(r => r.room_floor * 100 + r.room_number == BillSearchInput.RoomTrueId)))
                                && (BillSearchInput.CheckInTime == null || b.bill_checkInTime == BillSearchInput.CheckInTime)
                                && (BillSearchInput.CheckOutTime == null || b.bill_checkOutTime == BillSearchInput.CheckOutTime)
                                && (BillSearchInput.IfPaid == null || (b.bill_payTime == null && BillSearchInput.IfPaid == false) || (b.bill_payTime != null && BillSearchInput.IfPaid == true))
                                && (BillSearchInput.IfOut == null || (b.bill_trueCheckOutTime == null && BillSearchInput.IfOut == false) || (b.bill_trueCheckOutTime != null && BillSearchInput.IfOut == true))
                                && (BillSearchInput.IfChecked == null || (b.bill_trueCheckInTime == null && BillSearchInput.IfChecked == false) || (b.bill_trueCheckInTime != null && BillSearchInput.IfChecked == true)))
                                .ToListAsync();
        }
        override public async Task<IActionResult> OnGetAsync()
        {
            ReturnUrl ??= $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";

            if (TempData["BillSearchRoomId"] != null)
            {
                BillSearchInput.RoomTrueId = Convert.ToInt32(TempData["BillSearchRoomId"]);
            }
            if (TempData["BillSearchCheckInTime"] != null)
            {
                BillSearchInput.CheckInTime = Convert.ToDateTime(TempData["BillSearchCheckInTime"]);
            }
            if (TempData["BillSearchCheckOutTime"] != null)
            {
                BillSearchInput.CheckOutTime = Convert.ToDateTime(TempData["BillSearchCheckOutTime"]);
            }
            if (TempData["BillSearchIfPaid"] != null)
            {
                BillSearchInput.IfPaid = Convert.ToBoolean(TempData["BillSearchIfPaid"]);
            }
            if (TempData["BillSearchIfChecked"] != null)
            {
                BillSearchInput.IfChecked = Convert.ToBoolean(TempData["BillSearchIfChecked"]);
            }
            if (TempData["BillSearchIfOut"] != null)
            {
                BillSearchInput.IfOut = Convert.ToBoolean(TempData["BillSearchIfOut"]);
            }

            TempData["BillSearchFirstTime"] = "Yes";
            TempData.Keep("BillSearchFirstTime");

            if (BillSearchInput.GetType().GetProperties().Any(p => p.GetValue(BillSearchInput) != null))
            {
                await BillsSearch(BillSearchInput);
                BillSearchRender(BillSearchInput);
            }

            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            ReturnUrl ??= $"{Request.Scheme}://{Request.Host}{Request.Path}{Request.QueryString}";

            await BillsSearch(BillSearchInput);
            if (TempData["BillSearchFirstTime"] != null)
            {
                if ((string)TempData["BillSearchFirstTime"] == "Yes")
                {
                    TempData["BillSearchFirstTime"] = "No";
                }
            }
            else TempData["BillSearchFirstTime"] = "Yes";
            TempData.Keep("BillSearchFirstTime");

            BillSearchRender(BillSearchInput);

            return Page();
        }
    }
}