using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace HotelManagementSystem.Pages
{
    public class StuffModel : PageModel
    {
        private readonly UserManager<HotelStuff> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<StuffModel> _logger;
        private readonly UserContext _context;
        public IList<Labor> Stuff { get; set; } = new List<Labor>();
        public List<IdentityRole> AllRoles { get; set; }
        public StuffModel(RoleManager<IdentityRole> roleManager, ILogger<StuffModel> logger, UserContext context, UserManager<HotelStuff> userManager)
        {
            _logger = logger;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task OnGetAsync()
        {
            var users = await _context.Users.ToListAsync();
            foreach (var user in users)
            {
                var role = await _context.StuffRoles.FindAsync(user.Rolerole_id);
                Stuff.Add(new Labor
                {
                    StuffId = user.Id,
                    StuffNumber = user.stuff_number,
                    StuffName = user.stuff_name,
                    StuffRole = role.role_name
                });
            }

            Stuff = Stuff.OrderBy(s => _context.StuffRoles.FirstOrDefault(r => r.role_name == s.StuffRole).role_id).ToList();

        }

    }
}