using BrewBuddy.Interface;
using BrewBuddy.Models;

namespace BrewBuddy.Repositories
{
    public class TaskRepository : IRepository<Tasks>
    {
        private readonly BrewBuddyContext _context;

        public TaskRepository(BrewBuddyContext context)
        {
            _context = context;
        }

        public void Add(Models.Tasks task)
        {
            _context.Tasks.Add(task); //her kan vi tilføje en ny task
            _context.SaveChanges(); //denne er vigtig for det er den der sørger for at gemme den nye task 
        }

        public void Delete(int Id)
        {
            // Find the task by its ID
            var task = _context.Tasks.Find(Id);

            // If the task exists, remove i
            if (task != null)
            {
                _context.Tasks.Remove(task);
                _context.SaveChanges(); // Save changes to commit the deletion
            }
        }

        public List<Models.Tasks> GetAll()
        {
            throw new NotImplementedException();
        }

        public void Update(Models.Tasks entity)
        {
            throw new NotImplementedException();
        }
    }
    
    
}
