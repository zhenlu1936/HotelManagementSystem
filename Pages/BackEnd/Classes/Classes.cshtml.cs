using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace HotelManagementSystem.Pages
{
    public class ClassModel : PageModel
    {
        private readonly ILogger<ClassModel> _logger;
        private readonly HotelManagementContext _context;
        [BindProperty]
        public Class NewClass { get; set; } = new Class();
        public ClassModel(ILogger<ClassModel> logger, HotelManagementContext context)
        {
            _logger = logger;
            _context = context;
        }
        public async Task<IActionResult> OnGetAsync()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Classes.Add(NewClass);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "提交成功！";
            return Page();
        }
    }
}