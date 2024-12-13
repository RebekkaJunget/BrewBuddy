using BrewBuddy.Interface;
using BrewBuddy.Models;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Reflection.Metadata;
using System.Text.RegularExpressions;

namespace BrewBuddy.Repositories
{
    public class UserRepository : IRepository<User>
    {
        private readonly BrewBuddyContext _context;

        public UserRepository(BrewBuddyContext context)
        {
            _context = context;
        }

        private void ValidateUser(User user)
        {
            if (string.IsNullOrWhiteSpace(user.FirstName) || user.FirstName.Length < 1 || user.FirstName.Length > 50)
            {
                Debug.WriteLine($"Fejl i fornavn: '{user.FirstName}'");
                throw new UserValidationException("Fornavn skal være mellem 1 og 50 karakterer");
            }

            if (string.IsNullOrWhiteSpace(user.LastName) || user.LastName.Length < 1 || user.LastName.Length > 50)
            {
                Debug.WriteLine($"Fejl i efternavn: '{user.LastName}'");
                throw new UserValidationException("Efternavn skal være mellem 1 og 50 karakterer");
            }
            
            // Valider telefonnummer
            if (!Regex.IsMatch(user.PhoneNumber, @"^\d{8}$"))
                throw new UserValidationException("Mobilnummer skal være præcis 8 cifre.");

            // Tilføj +45, hvis det mangler
            if (!user.PhoneNumber.StartsWith("+45"))
                user.PhoneNumber = "+45" + user.PhoneNumber;


            if (!Regex.IsMatch(user.Email, @"^.+@.+\..+$"))
            {
                Debug.WriteLine($"Fejl i email: '{user.Email}'");

                throw new UserValidationException("Ugyldigt format.");
            }

            if (string.IsNullOrWhiteSpace(user.Password) || user.Password.Length < 8)
            {
                Debug.WriteLine($"Fejl i Password: '{user.Password}'");

                throw new UserValidationException("Password skal minimum være 8 karakterer lang.");
            }
            if (user.BirthDate >= DateTime.Now)
            {
                throw new UserValidationException("Fødesldato skal være før dags dato.");
            }
            if (user.RegistrationDate == DateTime.MinValue)
            {
                user.RegistrationDate = DateTime.Now; // Eller en anden passende dato
            }
            user.RegistrationDate = user.RegistrationDate < new DateTime(1753, 1, 1)
            ? DateTime.Now
            : user.RegistrationDate;


        }

        public void Add(User user)
        {
            try
            {
                ValidateUser(user);
                using (var context = new BrewBuddyContext())
                {
                    _context.Users.Add(user);
                    _context.SaveChanges();
                }
            }
            catch (UserValidationException ex)
            {
                Debug.WriteLine($"Validation error: {ex.Message}");
                throw; // Genkast undtagelsen, så den kan fanges i OnPost
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ukendt fejl: {ex.Message}");
                throw; // Genkast andre undtagelser
            }
        }
        public void Delete(int Id)
        {
            var user = _context.Users.Find(Id);

            // hvis brugeren eksistere, bliver det slettet 
            if (user != null)
            {
                _context.Users.Remove(user);
                _context.SaveChanges();
            }
        }

        public List<User> GetAll()
        {
            return _context.Users.ToList();
        }

        public User GetAllById(int Id)
        {
            return _context.Users.FirstOrDefault(a => a.UserId == Id);
        }

     

        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(m => m.UserId == id);
        }

        public void Update(User entity)
        {

            throw new NotImplementedException();
        }

        public async Task UpdateAsync(User updatedUser)
        {
            
            var existingUser = await _context.Users.FindAsync(updatedUser.UserId);

            if (existingUser != null)
            {
                updatedUser.UserId =existingUser.UserId;
                existingUser.FirstName = updatedUser.FirstName;
                existingUser.LastName = updatedUser.LastName;
                existingUser.PhoneNumber = updatedUser.PhoneNumber;
                existingUser.Role = updatedUser.Role;
                existingUser.BirthDate = updatedUser.BirthDate;
                existingUser.Email = updatedUser.Email;
                updatedUser.RegistrationDate = existingUser.RegistrationDate;


                // Ignorer RegistrationDate - det bevares som det er
            }

            _context.Entry(existingUser).CurrentValues.SetValues(updatedUser);

           
            Console.WriteLine($"BirthDate: {updatedUser.BirthDate}");

            Debug.WriteLine("******************************************"+updatedUser.BirthDate.Year);
            Debug.WriteLine($"UpdateUser: {JsonConvert.SerializeObject(updatedUser)}");
            _context.Update(existingUser);
           
            await _context.SaveChangesAsync();
        }
    }
}