namespace Build_School_Project_No_4.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class GameCategories
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public GameCategories()
        {
            Products = new HashSet<Products>();
        }

        [Key]
        public int GameCategoryId { get; set; }

        [Required]
        [StringLength(100)]
        public string GameName { get; set; }

        [Required]
        public string GameCoverImg { get; set; }

        public string GameCoverImgMini { get; set; }

        public string GameAlias { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Products> Products { get; set; }
    }
}
