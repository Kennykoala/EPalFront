using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Linq;

namespace Build_School_Project_No_4.DataModels
{
    public partial class EPalContext : DbContext
    {
        public EPalContext()
            : base("name=EPalContext")
        {
        }

        public virtual DbSet<Chatlist> Chatlist { get; set; }
        public virtual DbSet<Cities> Cities { get; set; }
        public virtual DbSet<Comment> Comment { get; set; }
        public virtual DbSet<CommentDetails> CommentDetails { get; set; }
        public virtual DbSet<Followings> Followings { get; set; }
        public virtual DbSet<GameCategories> GameCategories { get; set; }
        public virtual DbSet<Language> Language { get; set; }
        public virtual DbSet<LineStatus> LineStatus { get; set; }
        public virtual DbSet<LiveChat> LiveChat { get; set; }
        public virtual DbSet<MeetLikes> MeetLikes { get; set; }
        public virtual DbSet<Members> Members { get; set; }
        public virtual DbSet<MessageType> MessageType { get; set; }
        public virtual DbSet<Orders> Orders { get; set; }
        public virtual DbSet<OrderStatus> OrderStatus { get; set; }
        public virtual DbSet<Payments> Payments { get; set; }
        public virtual DbSet<Position> Position { get; set; }
        public virtual DbSet<ProductPlans> ProductPlans { get; set; }
        public virtual DbSet<ProductPosition> ProductPosition { get; set; }
        public virtual DbSet<Products> Products { get; set; }
        public virtual DbSet<ProductServer> ProductServer { get; set; }
        public virtual DbSet<ProductStyle> ProductStyle { get; set; }
        public virtual DbSet<Rank> Rank { get; set; }
        public virtual DbSet<RecentVistors> RecentVistors { get; set; }
        public virtual DbSet<SatisfyTag> SatisfyTag { get; set; }
        public virtual DbSet<Server> Server { get; set; }
        public virtual DbSet<Style> Style { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CommentDetails>()
                .HasMany(e => e.Comment)
                .WithRequired(e => e.CommentDetails)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GameCategories>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.GameCategories)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<LineStatus>()
                .Property(e => e.LineStatusName)
                .IsFixedLength();

            modelBuilder.Entity<Members>()
                .Property(e => e.AuthCode)
                .IsFixedLength();

            modelBuilder.Entity<Members>()
                .HasMany(e => e.Chatlist)
                .WithOptional(e => e.Members)
                .HasForeignKey(e => e.ReceiverId);

            modelBuilder.Entity<Members>()
                .HasMany(e => e.Chatlist1)
                .WithRequired(e => e.Members1)
                .HasForeignKey(e => e.SenderId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Members>()
                .HasMany(e => e.CommentDetails)
                .WithRequired(e => e.Members)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Members>()
                .HasMany(e => e.Followings)
                .WithRequired(e => e.Members)
                .HasForeignKey(e => e.FollowingId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Members>()
                .HasMany(e => e.Followings1)
                .WithRequired(e => e.Members1)
                .HasForeignKey(e => e.MemberId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Members>()
                .HasMany(e => e.LiveChat)
                .WithRequired(e => e.Members)
                .HasForeignKey(e => e.LiveReceiverId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Members>()
                .HasMany(e => e.LiveChat1)
                .WithRequired(e => e.Members1)
                .HasForeignKey(e => e.LiveSenderId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Members>()
                .HasMany(e => e.MeetLikes)
                .WithRequired(e => e.Members)
                .HasForeignKey(e => e.LikeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Members>()
                .HasMany(e => e.MeetLikes1)
                .WithRequired(e => e.Members1)
                .HasForeignKey(e => e.MemberId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Members>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Members)
                .HasForeignKey(e => e.CustomerId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Members>()
                .HasMany(e => e.Products)
                .WithRequired(e => e.Members)
                .HasForeignKey(e => e.CreatorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Members>()
                .HasMany(e => e.RecentVistors)
                .WithRequired(e => e.Members)
                .HasForeignKey(e => e.MemberId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Members>()
                .HasMany(e => e.RecentVistors1)
                .WithRequired(e => e.Members1)
                .HasForeignKey(e => e.RecentVistorId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MessageType>()
                .HasMany(e => e.Chatlist)
                .WithRequired(e => e.MessageType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<MessageType>()
                .HasMany(e => e.LiveChat)
                .WithRequired(e => e.MessageType)
                .HasForeignKey(e => e.LiveMessageTypeId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Orders>()
                .Property(e => e.UnitPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Orders>()
                .Property(e => e.Discount)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Orders>()
                .HasMany(e => e.Payments)
                .WithRequired(e => e.Orders)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Position>()
                .HasMany(e => e.ProductPosition)
                .WithRequired(e => e.Position)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Products>()
                .Property(e => e.UnitPrice)
                .HasPrecision(18, 0);

            modelBuilder.Entity<Products>()
                .HasMany(e => e.CommentDetails)
                .WithRequired(e => e.Products)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Products>()
                .HasMany(e => e.Orders)
                .WithRequired(e => e.Products)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Products>()
                .HasMany(e => e.ProductPlans)
                .WithRequired(e => e.Products)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Products>()
                .HasMany(e => e.ProductPosition)
                .WithRequired(e => e.Products)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Products>()
                .HasMany(e => e.ProductServer)
                .WithRequired(e => e.Products)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Products>()
                .HasMany(e => e.ProductStyle)
                .WithRequired(e => e.Products)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<SatisfyTag>()
                .HasMany(e => e.Comment)
                .WithRequired(e => e.SatisfyTag)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Server>()
                .HasMany(e => e.ProductServer)
                .WithRequired(e => e.Server)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Style>()
                .HasMany(e => e.ProductStyle)
                .WithRequired(e => e.Style)
                .WillCascadeOnDelete(false);
        }
    }
}
