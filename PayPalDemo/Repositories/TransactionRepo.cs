using PayPalDemo.Data;
using PayPalDemo.Models;

namespace PayPalDemo.Repositories
{
    public class TransactionRepo
    {
        private readonly ApplicationDbContext _context;

        public TransactionRepo(ApplicationDbContext context)
        {
            this._context = context;
        }

        public Transaction GetTransactionById(string transactionId)
        {
            return _context.Transactions.FirstOrDefault((t => t.TransactionID == transactionId));
        }
    }
}
