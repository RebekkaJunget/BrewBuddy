using BrewBuddy.Interface;
using BrewBuddy.Models;
using BrewBuddy.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace BrewBuddy.Pages.Users
{
    public class UpdateUserModel : PageModel
    {
        //vi statrer med at injektisere repositoriet i coffiemachinmodel
        private readonly IRepository<User> _repository;
        public UpdateUserModel(IRepository<User> repository)
        {
            _repository = repository;
        }

        [BindProperty]
        public User UpdateUser { get; set; }

        public async Task<IActionResult> OnGetAsync(int Id)
        {
            if (Id == 0)
            {
                return NotFound();
            }

            var user = await _repository.GetByIdAsync(Id);
            if (user == null)
            {
                return NotFound();
            }

            UpdateUser = user;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            if (UpdateUser.BirthDate < new DateTime(1753, 1, 1) || UpdateUser.BirthDate > new DateTime(9999, 12, 31))
            {
                ModelState.AddModelError("BirthDate", "Datoen skal være mellem 01/01/1753 og 31/12/9999.");
                return Page();
            }
            try
            {
                await _repository.UpdateAsync(UpdateUser);

            }
            catch (DbUpdateConcurrencyException)
            {
                var exists = await _repository.GetByIdAsync(UpdateUser.UserId);
                if (exists == null)
                {
                    return NotFound();

                }
                else
                {
                    throw;
                }


            }
            return RedirectToPage("/Users/Users");
        }
    }
}

