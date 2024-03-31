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
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }
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
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "确认密码")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }

        public async Task Initialize()
        {
            RoleOptions = new SelectList(await _context.StuffRoles.ToListAsync(), "role_id", "role_name");
        }
        public async Task OnGetAsync(string returnUrl = null)
        {
            await Initialize();
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = CreateUser();
                user.stuff_number = Input.stuff_number;
                user.stuff_name = Input.stuff_name;
                user.Rolerole_id = Input.stuff_role;

                await _userStore.SetUserNameAsync(user, Input.stuff_number, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    var claims = await _userManager.GetClaimsAsync(user);
                    var role = await _context.StuffRoles.FindAsync(Input.stuff_role);

                    var stuffNameClaim = claims.FirstOrDefault(c => c.Type == "stuff_name");
                    var stuffRoleClaim = claims.FirstOrDefault(c => c.Type == "stuff_role");

                    if (stuffNameClaim == null)
                    {
                        await _userManager.AddClaimAsync(user, new Claim("stuff_name", user.stuff_name));
                    }
                    else
                    {
                        await _userManager.ReplaceClaimAsync(user, stuffNameClaim, new Claim(stuffNameClaim.Type, user.stuff_name));
                    }
                    if (stuffRoleClaim == null)
                    {
                        await _userManager.AddClaimAsync(user, new Claim("stuff_role", role.role_name));
                    }
                    else
                    {
                        await _userManager.ReplaceClaimAsync(user, stuffRoleClaim, new Claim(stuffRoleClaim.Type, role.role_name));
                    }

                    await _signInManager.SignOutAsync();
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    //再次登出登入，保证claim正确刷新

                    return Redirect("/Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
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
