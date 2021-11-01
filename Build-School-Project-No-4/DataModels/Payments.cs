namespace Build_School_Project_No_4.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Payments
    {
        [Key]
        public int PaymentId { get; set; }

        [Required]
        public string TransactionUID { get; set; }

        public DateTime TransationDateTime { get; set; }

        public string ConfirmationId { get; set; }

        public int PayMethod { get; set; }

        public int OrderId { get; set; }

        public virtual Orders Orders { get; set; }
    }
}
