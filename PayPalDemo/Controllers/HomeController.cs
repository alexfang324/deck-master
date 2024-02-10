using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PayPalDemo.Data;
using PayPalDemo.Models;
using PayPalDemo.Repositories;
using System.Diagnostics;

namespace PayPalDemo.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger, IConfiguration configuration)
        {
            _context = context;
            _logger = logger;
            _configuration = configuration;

        }


        public IActionResult Index()
        {
            string email = User.Identity.Name;
            MyRegisteredUserRepo myRegisteredUserRepo = new MyRegisteredUserRepo(_context);

            if (email != null)
            {
                string firstName = myRegisteredUserRepo.GetUsernameFromEmail(email);
                HttpContext.Session.SetString("userName"
                                    , firstName);
            }

            return View("Index");
        }

        [Authorize(Roles = "Admin,Manager")]
        public IActionResult Transactions()
        {
            DbSet<Transaction> items = _context.Transactions;

            return View(items);
        }

        //handles successful paypal transaction infromation and
        //save them to database
        [HttpPost]
        [Authorize]
        public IActionResult PaySuccess([FromBody] Transaction transaction)
        {
            try
            {   //save transaction record to database
                _context.Transactions.Add(transaction);
                _context.SaveChanges();

                // Construct and return the redirect URL
                var redirectUrl = Url.Action("PayPalConfirmation", "Home", new { transactionId = transaction.TransactionID });
                return Json(new { redirectUrl });
            }
            catch (Exception ex)
            {

                return StatusCode(500, new { error = "An error occured while preocessing this payment" });
            }
        }


        //public IActionResult Confirmation(string confirmationId)
        //{
        //    IPN transaction =
        //    _context.IPNs.FirstOrDefault(t => t.paymentID == confirmationId);

        //    return View("Confirmation", transaction);
        //}

        //shows user info of the current logged in user
        public IActionResult SecureArea()
        {
            string userName = User.Identity.Name;
            var registeredUser = _context.MyRegisteredUsers.FirstOrDefault(ru => ru.Email == userName);

            return View(registeredUser);
        }

        //page to display after a successful paypal payment
        public IActionResult PayPalConfirmation(string transactionId)
        {
            TransactionRepo transactionRepo = new TransactionRepo(_context);
            Transaction transaction = transactionRepo.GetTransactionById(transactionId);
            return View("PayPalConfirmation", transaction);
        }


        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}