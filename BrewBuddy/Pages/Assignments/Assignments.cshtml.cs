using BrewBuddy.Interface;
using BrewBuddy.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using System.Diagnostics;
using System.Security.Claims;

namespace BrewBuddy.Pages.Assignments
{
    public class AssignmentsModel : PageModel
    {
        private readonly IRepository<Assignment> _repository;

        //denne her laver vi for at holde maskinerne i en liste 
        public List<Assignment> Assignments { get; set; }
        public List<Assignment> IncompleteAssignments { get; set; }
        public List<Assignment> TodaysCompletedAssignments { get; set; }
        public List<Assignment> YesterdaysCompletedAssignments { get; set; }

        [BindProperty]
        public Assignment NewAssignment { get; set; }

        //derefter laver vi en konstrukt�r med repositori
        public AssignmentsModel(IRepository<Assignment> repository)
        {
            _repository = repository;
        }


        public void OnGet()
        {
            // Get all assignments from the repository
            var allAssignments = _repository.GetAll();
            PopulateAssignmentLists(allAssignments);
        }

        public IActionResult OnPost()
        {
            Debug.WriteLine($"MachineId received: {NewAssignment.MachineId}");

            if (!ModelState.IsValid)
            {
                Debug.WriteLine("ModelState is invalid");
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Debug.WriteLine($"Error: {error.ErrorMessage}");
                }
                Assignments = _repository.GetAll();
                return Page();
            }
            NewAssignment.UserId = null;
            NewAssignment.FinishedDateAndTime = null;
            _repository.Add(NewAssignment);
            return RedirectToPage();
        }

        //public IActionResult OnPostComplete(int AssignmentId)
        //{
        //    //Debug.WriteLine("hallo");
        //    //Debug.WriteLine(AssignmentId);
        //    var assignment = _repository.GetAllById(AssignmentId);
        //    if (assignment == null)
        //    {
        //        return Page();
        //    }
        //    assignment.IsComplete = true;
        //    assignment.FinishedDateAndTime = DateTime.Now;
        //    assignment.UserId = ;


        //    _repository.Update(assignment);

        //    Assignments = _repository.GetAll();

        //    return Page();



        public IActionResult OnPostComplete(int AssignmentId, decimal? Amount)
        {
            // Check if Amount is valid
            if (Amount < 0)
            {
                ModelState.AddModelError("Amount", "Bel�bet skal v�re st�rre end 0.");
                IncompleteAssignments = GetIncompleteAssignments(_repository.GetAll()); // Refresh assignments list
                return Page(); // Return to the page with the error message
            }
            // Hent opgaven fra databasen
            var assignment = _repository.GetAllById(AssignmentId);
            if (assignment == null)
            {
                ModelState.AddModelError("", "Opgaven findes ikke.");
                Assignments = _repository.GetAll();
                return Page();
            }

            // Hent UserId fra claims
            var userIdString = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userIdString) || !int.TryParse(userIdString, out int userId))
            {
                ModelState.AddModelError("", "Du skal v�re logget ind for at fuldf�re en opgave.");
                Assignments = _repository.GetAll();
                return Page();
            }

            // Marker opgaven som fuldf�rt og tildel UserId
            assignment.IsComplete = true;
            assignment.FinishedDateAndTime = DateTime.Now;
            assignment.UserId = userId;
            assignment.Amount = Amount;


            // Opdater opgaven i databasen
            _repository.Update(assignment);

            // Opdater listen over opgaver
            PopulateAssignmentLists(_repository.GetAll());

            return Page();
        }


        // Metode til at opdatere listerne
        private void PopulateAssignmentLists(IEnumerable<Assignment> allAssignments)
        {
            IncompleteAssignments = GetIncompleteAssignments(allAssignments);
            TodaysCompletedAssignments = GetTodaysCompletedAssignments(allAssignments);
            YesterdaysCompletedAssignments = GetYesterdaysCompletedAssignments(allAssignments);
        }

        // Metoder til filtrering
        private List<Assignment> GetIncompleteAssignments(IEnumerable<Assignment> allAssignments)
        {
            return allAssignments
                .Where(a => !a.IsComplete)
                .ToList();
        }

        private List<Assignment> GetTodaysCompletedAssignments(IEnumerable<Assignment> allAssignments)
        {
            var today = DateTime.Today;
            return allAssignments
                .Where(a => a.IsComplete && a.FinishedDateAndTime?.Date == today)
                .OrderBy(a => a.FinishedDateAndTime)
                .ToList();
        }

        private List<Assignment> GetYesterdaysCompletedAssignments(IEnumerable<Assignment> allAssignments)
        {
            var yesterday = DateTime.Today.AddDays(-1);
            return allAssignments
                .Where(a => a.IsComplete && a.FinishedDateAndTime?.Date == yesterday)
                .OrderBy(a => a.FinishedDateAndTime)
                .ToList();
        }

    }
}