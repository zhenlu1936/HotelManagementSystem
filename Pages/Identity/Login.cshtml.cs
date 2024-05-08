// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System.Security.Claims;

namespace HotelManagementSystem.Areas.Identity.Pages.Account
{
    public class LoginModel : PageModel
    {
        private readonly SignInManager<HotelStuff> _signInManager;
        private readonly UserManager<HotelStuff> _userManager;
        private readonly ILogger<LoginModel> _logger;
        private readonly UserContext _context;
        public LoginModel(SignInManager<HotelStuff> signInManager, UserContext context, UserManager<HotelStuff> userManager, ILogger<LoginModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _context = context;
            _logger = logger;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public string ReturnUrl { get; set; }

        [TempData]
        public string ErrorMessage { get; set; }
        public class InputModel
        {
            [Required]
            public string Number { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            public bool RememberMe { get; set; }
        }

        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!string.IsNullOrEmpty(ErrorMessage))
            {
                ModelState.AddModelError(string.Empty, ErrorMessage);
            }

            returnUrl ??= Url.Content("~/");

            // Clear the existing external cookie to ensure a clean login process
            await HttpContext.SignOutAsync(IdentityConstants.ExternalScheme);

            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();

            if (ModelState.IsValid)
            {
                // This doesn't count login failures towards account lockout
                // To enable password failures to trigger account lockout, set lockoutOnFailure: true
                var result = await _signInManager.PasswordSignInAsync(Input.Number, Input.Password, Input.RememberMe, lockoutOnFailure: false);
                var user = await _userManager.GetUserAsync(User);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User logged in.");
                    var claims = await _userManager.GetClaimsAsync(user);
                    var stuffNameClaim = claims.FirstOrDefault(c => c.Type == "stuff_name");
                    var stuffRoleClaim = claims.FirstOrDefault(c => c.Type == "stuff_role");

                    if (stuffNameClaim == null)
                    {
                        // 用户没有stuff_name的Claim，添加它
                        await _userManager.AddClaimAsync(user, new Claim("stuff_name", user.stuff_name));
                    }
                    else
                    {
                        // 用户已经有了stuff_name的Claim，可以选择更新它
                        await _userManager.ReplaceClaimAsync(user, stuffNameClaim, new Claim(stuffNameClaim.Type, user.stuff_name));
                    }
                    if (stuffRoleClaim == null)
                    {
                        var role = await _context.StuffRoles.FindAsync(user.Rolerole_id);
                        await _userManager.AddClaimAsync(user, new Claim("stuff_role", role.role_name));
                    }
                    else
                    {
                        var role = await _context.StuffRoles.FindAsync(user.Rolerole_id);
                        await _userManager.ReplaceClaimAsync(user, stuffRoleClaim, new Claim(stuffRoleClaim.Type, role.role_name));
                    }

                    return LocalRedirect(returnUrl);
                }
                if (result.RequiresTwoFactor)
                {
                    return RedirectToPage("./LoginWith2fa", new { ReturnUrl = returnUrl, RememberMe = Input.RememberMe });
                }
                if (result.IsLockedOut)
                {
                    _logger.LogWarning("User account locked out.");
                    return RedirectToPage("./Lockout");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                    return Page();
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }
    }
}
