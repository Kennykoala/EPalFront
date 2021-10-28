namespace Build_School_Project_No_4.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Orders
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Orders()
        {
            Payments = new HashSet<Payments>();
        }

        [Key]
        public int OrderId { get; set; }

        public int CustomerId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        public decimal UnitPrice { get; set; }

        public decimal? Discount { get; set; }

        public DateTime OrderDate { get; set; }

        public DateTime DesiredStartTime { get; set; }

        public DateTime? GameEndDateTime { get; set; }

        public int? OrderStatusId { get; set; }

        [Required]
        [StringLength(50)]
        public string OrderConfirmation { get; set; }

        public DateTime? GameStartTime { get; set; }

        public int? OrderStatusIdCreator { get; set; }

        public virtual Members Members { get; set; }

        public virtual OrderStatus OrderStatus { get; set; }

        public virtual Products Products { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Payments> Payments { get; set; }
    }
}
