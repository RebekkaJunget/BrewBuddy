
using BrewBuddy.Interface;
using BrewBuddy.Models;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BrewBuddy.Pages
{
    //[Authorize(Policy = "AdminOnly")]
    public class UsersModel : PageModel
    {
        private readonly IRepository<User> _repository; //vi statrer med at injektisere repositoriet i coffiemachinmodel

        public List<User> users { get; set; } //denne her laver vi for at holde maskinerne i en liste 


        [BindProperty]
        public User NewUser { get; set; } //og den her laver vi for at kunne oprette en ny maskine 

       
        //Her genere vi en Bcrypt salt - ellers kunne vi skrive /*workFactor: 12*/ i stedet 
        string salt = BCrypt.Net.BCrypt.GenerateSalt(12);

        public UsersModel(IRepository<User> repository) //derefter laver vi en konstruktør med repositori
        {
            _repository = repository;
        }


        public void OnGet()
        {
            users = _repository.GetAll();

        }


        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                Debug.WriteLine("Modelstate invalid");
                users = _repository.GetAll();
                return Page();
            }
            try
            {

                //Hash password før indsættelse
                NewUser.Password = BCrypt.Net.BCrypt.HashPassword(NewUser.Password, salt);

                Debug.WriteLine($"User data: {JsonConvert.SerializeObject(NewUser)}");
                //Tilføj brugeren via repositoriet
                _repository.Add(NewUser);

                //Redirect ved succes
                return RedirectToPage();
            }
            catch (UserValidationException ex)
            {
                Debug.WriteLine($"Validation error: {ex.Message}");
                throw;
            }
            catch (DbUpdateException ex)
            {
                Debug.WriteLine($"DbUpdateException: {ex.InnerException?.Message}");
                throw;
            }
            catch (Exception ex)
            {
                //Debug.WriteLine($"Valideringsfejl i OnPost: {ex.Message}");
                //ModelState.AddModelError(string.Empty, ex.Message); // Tilføj fejlbesked til ModelState
                users = _repository.GetAll();
                return Page();
            }

        }

        public IActionResult OnPostDelete(int UserId)
        {
            _repository.Delete(UserId);
            return RedirectToPage();
        }

    }
}
