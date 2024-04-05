using System.Linq;
using System.Threading.Tasks;
using Classifieds.Data;
using Classifieds.Data.Entities;
using Classifieds.Web.Services.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Classifieds.Web.Pages.Advertisements
{
    // limit access to this page to users who are of minimum age through the IsMinimumAge policy
    [Authorize(Policy = Policies.IsMinimumAge)]
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public CreateModel(ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty] public Advertisement Advertisement { get; set; }

        public IActionResult OnGet()
        {
            // // Claims can be accessed like this
            // var isMinimumAge = User.Claims.FirstOrDefault(q=>q.Type == UserClaims.IsMinimumAge)?.Value;
            // var isMinimumAgeAlternative = User.FindFirst(q=>q.Type == UserClaims.IsMinimumAge)?.Value;
            //
            // // If the user is not of minimum age, redirect to AccessDenied page
            // if (isMinimumAge == null || bool.Parse(isMinimumAge) == false)
            // {
            //     return RedirectToPage("/Identity/Account/AccessDenied");
            // }

            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return Page();
        }

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid) return Page();

            _context.Advertisements.Add(Advertisement);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}