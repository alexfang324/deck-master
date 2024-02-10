using PayPalDemo.Data;
using PayPalDemo.Models;

namespace PayPalDemo.Repositories
{
    public class MyRegisteredUserRepo
    {
        private readonly ApplicationDbContext _context;

        public MyRegisteredUserRepo(ApplicationDbContext context)
        {
            this._context = context;
        }

        public void AddRegisteredUser(string email, string firstName, string lastName)
        {
            MyRegisteredUser registerUser = new MyRegisteredUser()
            {
                Email = email,
                FirstName = firstName,
                LastName = lastName
            };
            _context.MyRegisteredUsers.Add(registerUser);
            _context.SaveChanges();
        }

        public string GetUsernameFromEmail(string email)
        {
            MyRegisteredUser registeredUser = _context.MyRegisteredUsers.Where((u) => u.Email == email).FirstOrDefault();
            return registeredUser.FirstName;
        }
    }
}
