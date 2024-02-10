using System.ComponentModel.DataAnnotations;

namespace PayPalDemo.Models
{
    public class Transaction
    {
        [Display(Name = "Payment ID")]
        [Key] // Define primary key.
        public string TransactionID { get; set; }

        [Display(Name = "Created")]
        public string CreateTime { get; set; }

        [Display(Name = "Name")]
        public string PayerName { get; set; }

        [Display(Name = "Email")]
        public string PayerEmail { get; set; }

        [Display(Name = "Amount")]
        public float Amount { get; set; }

        [Display(Name = "Currency")]
        public string Currency { get; set; }



        [Display(Name = "MOP")]
        public string PaymentMethod { get; set; }
    }
}
