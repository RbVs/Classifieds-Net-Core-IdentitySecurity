using System.Collections.Generic;
using System.Threading.Tasks;
using Classifieds.Data;
using Classifieds.Data.Constants;
using Classifieds.Data.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Classifieds.Web.Pages.Categories
{
    [Authorize(Roles = Roles.Administrator)]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Category> Category { get; set; }

        public async Task OnGetAsync()
        {
            Category = await _context.Categories.ToListAsync();
        }
    }
}