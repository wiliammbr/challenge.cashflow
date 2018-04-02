using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Challenge.CashFlow.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        [Required(ErrorMessage = "The description of the transaction is mandatory")]
        [Display(Name = "Description")]
        public string Description { get; set; }
        [Required(ErrorMessage = "The amount of the transaction is mandatory")]
        [Display(Name = "Amount")]
        public decimal Amount { get; set; }
        [Required(ErrorMessage = "The payment type of the transaction is mandatory")]
        [Display(Name = "Payment Type")]
        public string PaymentType { get; set; }
        [Required]
        public DateTime Date { get; set; }

        public string PaymentTypeText
        {
            get 
            {
                return this.PaymentType == "C" ? "Credit Card" : this.PaymentType == "M" ? "Money" : "";
            }
        }
    }
}