using BrewBuddy.Interface;
using BrewBuddy.Models;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace BrewBuddy.Repositories
{
    public class AssignmentRepository : IRepository<Assignment>
    {
        private readonly BrewBuddyContext _context;

        //vi laver en konstruktør som tager brewbuddycontext som parameter 
        public AssignmentRepository(BrewBuddyContext context)
        {
            _context = context;
        }
        public void Add(Assignment assignment)
        {
            _context.Assignments.Add(assignment);
            _context.SaveChanges(); 
        }

        public void Delete(int Id)
        {
            // Find the coffee machine by its ID
            var assignment = _context.Assignments.Find(Id);

            // If the machine exists, remove it
            if (assignment != null)
            {
                _context.Assignments.Remove(assignment);
                _context.SaveChanges(); // Save changes to commit the deletion
            }
        }
        public CoffieMachine GetMachineById(int machineId)
        {
            return _context.CoffieMachines.FirstOrDefault(m => m.MachineId == machineId);
        }

        public List<Assignment> GetAll()
        {
            return _context.Assignments.ToList(); //henter alle kaffe maskiner
        }

        public Assignment GetAllById(int Id)
        {
            return _context.Assignments.FirstOrDefault(a => a.AssignmentId == Id);
        }

        public Task<Assignment> GetByIdAsync(int Id)
        {
            throw new NotImplementedException();
        }

        public void Update(Assignment assignment)
        {
            Debug.WriteLine($"Updating assignment: {assignment.AssignmentId}");
            _context.Assignments.Update(assignment);
            _context.SaveChanges();
        }

        Task IRepository<Assignment>.UpdateAsync(Assignment entity)
        {
            throw new NotImplementedException();
        }
    }
}
