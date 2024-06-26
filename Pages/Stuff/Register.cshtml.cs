// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace HotelManagementSystem.Areas.Identity.Pages.Account
{
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<HotelStuff> _signInManager;
        private readonly UserManager<HotelStuff> _userManager;
        private readonly IUserStore<HotelStuff> _userStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly UserContext _context;
        public SelectList RoleOptions { get; set; }
        public RegisterModel(
            UserManager<HotelStuff> userManager,
            IUserStore<HotelStuff> userStore,
            SignInManager<HotelStuff> signInManager,
            ILogger<RegisterModel> logger,
            UserContext context
           )
        {
            _userManager = userManager;
            _userStore = userStore;
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        [BindProperty]
        public InputModel Input { get; set; } = new InputModel();
        public string ReturnUrl { get; set; }
        public HotelStuff NewUser { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public class InputModel
        {
            [Required]
            [Display(Name = "工号")]
            public string stuff_number { get; set; }

            [Required]
            [Display(Name = "姓名")]
            public string stuff_name { get; set; }

            [Required]
            [Display(Name = "权限")]
            public int stuff_role { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "密码")]
            public string Password { get; set; } = "123456aA!";

            [DataType(DataType.Password)]
            [Display(Name = "确认密码")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; } = "123456aA!";
        }

        public async Task Initialize()
        {
            var Roles = await _context.StuffRoles.ToListAsync();
            var user = await _userManager.GetUserAsync(User);
            var claims = await _userManager.GetClaimsAsync(user);
            var stuffRoleClaim = claims.FirstOrDefault(c => c.Type == "stuff_role");
            if (stuffRoleClaim.Value == "管理员")
            {
                Roles = Roles.Where(r => r.role_id != 1 && r.role_id != 2).ToList();
            }
            if (stuffRoleClaim.Value == "经理")
            {
                Roles = Roles.Where(r => r.role_id != 1).ToList();
            }
            RoleOptions = new SelectList(Roles, "role_id", "role_name");
        }
        public async Task OnGetAsync(string stuffId, string returnUrl = null)
        {
            if (stuffId != null)
            {
                NewUser = await _context.Users.FindAsync(stuffId);
                Input.stuff_number = NewUser.stuff_number;
                Input.stuff_name = NewUser.stuff_name;
                Input.stuff_role = NewUser.Rolerole_id;

                HttpContext.Session.SetString("FormerId", stuffId);
                ViewData["Title"] = "编辑";
                ViewData["Text"] = "为管理人员编辑账户。";
                ViewData["Submit"] = "编辑";
            }
            else
            {
                ViewData["Title"] = "注册";
                ViewData["Text"] = "为管理人员创建新账户。";
                ViewData["Submit"] = "注册";
            }

            await Initialize();
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            string FormerId = HttpContext.Session.GetString("FormerId");
            if (FormerId != null)
            {
                NewUser = await _context.Users.FindAsync(FormerId);
                Input.stuff_role = NewUser.Rolerole_id;

            }

            if (ModelState.IsValid)
            {

                if (FormerId == null) NewUser = CreateUser();
                NewUser.stuff_number = Input.stuff_number;
                NewUser.Rolerole_id = Input.stuff_role;
                NewUser.stuff_name = Input.stuff_name;

                await _userStore.SetUserNameAsync(NewUser, Input.stuff_number, CancellationToken.None);

                IdentityResult result;
                if (FormerId == null) //如果不是编辑已有员工
                {
                    result = await _userManager.CreateAsync(NewUser, Input.Password);

                    if (result.Succeeded)
                    {
                        TempData["SuccessMessage"] = "创建成功！";
                        return Redirect("/Stuff/Manage");
                    }
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
                else
                {
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = "编辑成功！";
                    HttpContext.Session.Remove("FormerId");
                    return Redirect("/Stuff/Manage");
                }
            }

            await Initialize();
            return Page();
        }

        private HotelStuff CreateUser()
        {
            try
            {
                return Activator.CreateInstance<HotelStuff>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(HotelStuff)}'. " +
                    $"Ensure that '{nameof(HotelStuff)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
    }
}
