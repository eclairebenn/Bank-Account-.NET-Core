using System;
using System.ComponentModel.DataAnnotations;

namespace bank_account.Models
{
    public class Record : BaseEntity
    {
        public int RecordId { get; set; }

        [Required]
        [Display(Name = "Deposit(+) / Withdrawal(-)")]
        [RegularExpression(@"^-?[0-9]{1,12}(?:\.[0-9]{1,4})?$")]
        public float Amount { get; set; }
        public DateTime CreatedAt { get; set; }

        public int UserId {get;set;}
        public User User {get;set;}
        public Record()
        {
            CreatedAt = DateTime.Now;
        }
    }
}