// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using IntegralInvesting.Areas.Identity.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IntegralInvesting.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<IntegralInvestingUser> _userManager;
        private readonly SignInManager<IntegralInvestingUser> _signInManager;

        public IndexModel(
            UserManager<IntegralInvestingUser> userManager,
            SignInManager<IntegralInvestingUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            // New custom fields added for First Name and Last Name
            [DataType(DataType.Text)]
            [Display(Name = "First Name")]
            public string FirstName { get; set; }

            [DataType(DataType.Text)]
            [Display(Name = "Last Name")]
            public string LastName { get; set; }
        }

        private async Task LoadAsync(IntegralInvestingUser user)
        {
            var userName = await _userManager.GetUserNameAsync(user);

            Username = userName;

            Input = new InputModel
            {
                FirstName = user.FirstName, 
                LastName = user.LastName
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            bool isFirstNameChanged = HandleFirstNameChange(user);
            bool isLastNameChanged = HandleLastNameChange(user);

            var result = await _userManager.UpdateAsync(user);

            if (result.Succeeded)
            {
                await _signInManager.RefreshSignInAsync(user);
                StatusMessage = UpdateStatusMessage(isFirstNameChanged, isLastNameChanged);
                return RedirectToPage();
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private bool HandleFirstNameChange(IntegralInvestingUser user)
        {
            if (user.FirstName != Input.FirstName)
            {
                user.FirstName = Input.FirstName;
                return true;
            }
            else
            {
                return false;
            }
        }

        private bool HandleLastNameChange(IntegralInvestingUser user)
        {
            if (user.LastName != Input.LastName)
            {
                user.LastName = Input.LastName;
                return true;
            }
            else
            {
                return false;
            }
        }

        private string UpdateStatusMessage(bool isFirstNameChanged, bool isLastNameChanged)
        {
            if (isFirstNameChanged && !isLastNameChanged)
                return "First Name updated successfully";

            if (!isFirstNameChanged && isLastNameChanged)
                return "Last Name updated successfully";

            return "First Name and Last Name updated successfully";
        }
    }
}
