namespace Build_School_Project_No_4.DataModels
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Members
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Members()
        {
            Chatlist = new HashSet<Chatlist>();
            Chatlist1 = new HashSet<Chatlist>();
            CommentDetails = new HashSet<CommentDetails>();
            Followings = new HashSet<Followings>();
            Followings1 = new HashSet<Followings>();
            LiveChat = new HashSet<LiveChat>();
            LiveChat1 = new HashSet<LiveChat>();
            MeetLikes = new HashSet<MeetLikes>();
            MeetLikes1 = new HashSet<MeetLikes>();
            Orders = new HashSet<Orders>();
            Products = new HashSet<Products>();
            RecentVistors = new HashSet<RecentVistors>();
            RecentVistors1 = new HashSet<RecentVistors>();
        }

        [Key]
        public int MemberId { get; set; }

        [StringLength(50)]
        public string MemberName { get; set; }

        public DateTime? RegistrationDate { get; set; }

        [Required]
        [StringLength(50)]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        [StringLength(50)]
        public string Phone { get; set; }

        [StringLength(50)]
        public string Country { get; set; }

        public int? CityId { get; set; }

        public int? Gender { get; set; }

        [Column(TypeName = "date")]
        public DateTime? BirthDay { get; set; }

        public int? TimeZone { get; set; }

        public int? LanguageId { get; set; }

        public string Bio { get; set; }

        public string ProfilePicture { get; set; }

        public int? LineStatusId { get; set; }

        [StringLength(10)]
        public string AuthCode { get; set; }

        public bool? IsAdmin { get; set; }

        public string MeetPicture { get; set; }

        public int? LoginMethod { get; set; }

        public string GoogleId { get; set; }

        public string FBId { get; set; }

        public string LineId { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Chatlist> Chatlist { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Chatlist> Chatlist1 { get; set; }

        public virtual Cities Cities { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<CommentDetails> CommentDetails { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Followings> Followings { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Followings> Followings1 { get; set; }

        public virtual Language Language { get; set; }

        public virtual LineStatus LineStatus { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LiveChat> LiveChat { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<LiveChat> LiveChat1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MeetLikes> MeetLikes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MeetLikes> MeetLikes1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Orders> Orders { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Products> Products { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RecentVistors> RecentVistors { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<RecentVistors> RecentVistors1 { get; set; }
    }
}
