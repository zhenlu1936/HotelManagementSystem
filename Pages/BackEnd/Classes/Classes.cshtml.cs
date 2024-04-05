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
        public async Task<IActionResult> OnGetAsync(int? classId)
        {
            if (classId != null)
            {
                NewClass = await _context.Classes.FindAsync(classId);
                HttpContext.Session.SetInt32("FormerId", (int)classId);
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            int? FormerId = HttpContext.Session.GetInt32("FormerId");
            if (FormerId == null)
            {
                _context.Classes.Add(NewClass);
            }
            else
            {
                var FormerClass = await _context.Classes.FindAsync(FormerId);
                FormerClass.class_price = NewClass.class_price;
                FormerClass.class_name = NewClass.class_name;
                FormerClass.class_capacity = NewClass.class_capacity;
            }
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "提交成功！";
            HttpContext.Session.Remove("FormerId");
            return Redirect("/BackEnd/Manage");

        }
    }
}