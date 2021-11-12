namespace Build_School_Project_No_4.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Products
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Products()
        {
            CommentDetails = new HashSet<CommentDetails>();
            Orders = new HashSet<Orders>();
            ProductPlans = new HashSet<ProductPlans>();
            ProductPosition = new HashSet<ProductPosition>();
            ProductServer = new HashSet<ProductServer>();
            ProductStyle = new HashSet<ProductStyle>();
        }

        [Key]
        public int ProductId { get; set; }

        public int GameCategoryId { get; set; }

        public int CreatorId { get; set; }

        public decimal UnitPrice { get; set; }

        [Required]
        public string ProductImg { get; set; }

        [Required]
        public string RecommendationVoice { get; set; }

        [Required]
        public string Introduction { get; set; }

        public int? RankId { get; set; }

        [Required]
        public string CreatorImg { get; set; }

        public bool? ProductStatus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommentDetails> CommentDetails { get; set; }

        public virtual GameCategories GameCategories { get; set; }

        public virtual Members Members { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orders> Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductPlans> ProductPlans { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductPosition> ProductPosition { get; set; }

        public virtual Rank Rank { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductServer> ProductServer { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ProductStyle> ProductStyle { get; set; }
    }
}
