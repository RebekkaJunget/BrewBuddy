using BrewBuddy.Interface;
using BrewBuddy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Diagnostics.Metrics;

namespace BrewBuddy.Pages
{
    public class CoffieMachinesModel : PageModel
    {
        //vi statrer med at injektisere repositoriet i coffiemachinmodel
        private readonly IRepository<CoffieMachine> _repository;

        //denne her laver vi for at holde maskinerne i en liste 
        public List<CoffieMachine> coffieMachines { get; set; }

        //og den her laver vi for at kunne oprette en ny maskine 
        [BindProperty]
        public CoffieMachine NewMachine { get; set; }

        [BindProperty]
        public CoffieMachine UpdatedMachine { get; set; }

        //derefter laver vi en konstruktÝr med repositori
        public CoffieMachinesModel(IRepository<CoffieMachine> repository)
        {
            _repository = repository;
        }


        public void OnGet()
        {
            coffieMachines = _repository.GetAll();

        }

        public IActionResult OnPost()
        {
           
            if (!ModelState.IsValid)
            {
                coffieMachines = _repository.GetAll();
                return Page();
            }
           
            _repository.Add(NewMachine);
            coffieMachines = _repository.GetAll();
            return RedirectToPage();
        }

        public IActionResult OnPostDelete(int MachineId)
        {
            _repository.Delete(MachineId);
            return RedirectToPage();
        }


        public IActionResult OnPostUpdate()
        {
            if (!ModelState.IsValid) {
                coffieMachines = _repository.GetAll();
                return Page();
            }
            _repository.Update(UpdatedMachine);
            return RedirectToPage();
        }
    }
}
