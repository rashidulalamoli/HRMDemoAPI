using Microsoft.EntityFrameworkCore;

namespace DataAccess.Models
{
    public partial class HRMContext: DbContext
    {
        public HRMContext(DbContextOptions<HRMContext> options) : base(options)
        {

        }
        public  DbSet<User> User { get; set; }
        public  DbSet<Task> Task { get; set; }
        public  DbSet<LeaveRequest> LeaveRequest { get; set; }
        public  DbSet<Notification> Notification { get; set; }
        public  DbSet<NotificationManager> NotificationManager { get; set; }
        public  DbSet<Relation> Relation { get; set; }
        public  DbSet<Role> Role { get; set; }
        public  DbSet<UserRelationMap> UserRelationMap { get; set; }

        ///model validation will go here via fluent validation api.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("User", "dbo");
                entity.HasIndex(e => e.Email)
                    .HasName("UQ__User__Email")
                    .IsUnique();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.UserGuid)
                   .IsRequired()
                   .HasMaxLength(50);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.FullName).HasMaxLength(150);

                entity.Property(e => e.Initials).HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PasswordHash).HasMaxLength(4000);

                entity.Property(e => e.Phone).HasMaxLength(50);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(150);
            });
        }
    }
}
