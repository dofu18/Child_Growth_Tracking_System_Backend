using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainLayer.Constants;
using DomainLayer.Entities;
using DomainLayer.Enum;
using Microsoft.AspNetCore.Mvc.Razor.Internal;
using Microsoft.EntityFrameworkCore;
using static DomainLayer.Enum.GeneralEnum;

namespace InfrastructureLayer.Database
{
    public class ChildGrowthDbContext : DbContext
    {
        public DbSet<Alert> Alerts { get; set; }
        public DbSet<BmiCategory> BmiCategories { get; set; }
        public DbSet<Children> Children { get; set; }
        public DbSet<ConsultationRequest> ConsultationRequests { get; set; }
        public DbSet<ConsultationResponse> ConsultationResponses { get; set; }
        public DbSet<DoctorLicense> DoctorLicense { get; set; }
        public DbSet<DoctorSpecialization> DoctorSpecialization { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<GrowthRecord> GrowthRecords { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<PackageFeature> PackageFeatures { get; set; }
        public DbSet<RatingFeedback> RatingFeedbacks { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SharingProfile> SharingProfiles { get; set; }
        public DbSet<Specialization> Specializations { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserPackage> UserPackages { get; set; }

        public ChildGrowthDbContext(DbContextOptions<ChildGrowthDbContext> options) : base(options) { }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
            foreach (var entry in entries)
            {
                if (entry.State == EntityState.Added)
                {
                    ((BaseEntity)entry.Entity).CreatedAt = DateTime.UtcNow;
                }
              ((BaseEntity)entry.Entity).UpdatedAt = DateTime.UtcNow;
            }
            return base.SaveChangesAsync(cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Alert>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.Children).WithMany().HasForeignKey(x => x.ChildrentId).OnDelete(DeleteBehavior.Cascade);
                e.Property(x => x.AlertDate).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                e.Property(x => x.Message).IsRequired().HasMaxLength(500);
                e.HasOne(x => x.ReceivedUser).WithMany().HasForeignKey(x => x.ReveivedUserId).OnDelete(DeleteBehavior.Cascade);
                e.Property(x => x.IsRead).IsRequired();
            });
            modelBuilder.Entity<BmiCategory>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Name).IsRequired().HasMaxLength(100);
                e.Property(x => x.BmiTop).IsRequired();
                e.Property(x => x.BmiBottom).IsRequired();
                e.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                e.Property(x => x.UpdatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");

            });
            modelBuilder.Entity<Children>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.Parent).WithMany().HasForeignKey(x => x.ParentId).OnDelete(DeleteBehavior.Cascade);
                e.Property(x => x.Name).IsRequired().HasMaxLength(50);
                e.Property(x => x.DoB).IsRequired().HasDefaultValueSql("CURRENT_DATE");
                e.Property(x => x.Gender).HasConversion<string>().HasDefaultValue(GenderEnum.Male);
                e.Property(x => x.Weight).IsRequired();
                e.Property(x => x.Height).IsRequired();
                e.Property(x => x.Bmi).IsRequired();
                e.HasOne(x => x.BmiCategory).WithMany().HasForeignKey(x => x.BmiCategoryId).OnDelete(DeleteBehavior.Cascade);
                e.Property(x => x.BmiPercentile).IsRequired();
                e.Property(x => x.Notes).IsRequired().HasMaxLength(500);
                e.Property(x => x.GroupAge).HasConversion<string>().HasDefaultValue(GroupAgeEnum.From2to19);
                e.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                e.Property(x => x.UpdatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");

            });
            modelBuilder.Entity<ConsultationRequest>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.Children).WithMany().HasForeignKey(x => x.ChildrentId).OnDelete(DeleteBehavior.Cascade);
                e.Property(x => x.RequestDate).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                e.Property(x => x.Description).IsRequired().HasMaxLength(500);
                e.Property(x => x.Status).HasConversion<string>().HasDefaultValue(ConsultationRequestStatusEnum.Pending);
                e.Property(x => x.Attachments).IsRequired().HasMaxLength(500);
                e.HasOne(x => x.UserRequest).WithMany().HasForeignKey(x => x.UserRequestId).OnDelete(DeleteBehavior.Cascade);
                e.Property(x => x.UpdatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");

            });
            modelBuilder.Entity<ConsultationResponse>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.ConsultationRequest).WithMany().HasForeignKey(x => x.RequestId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(x => x.Doctor).WithMany().HasForeignKey(x => x.DoctorId).OnDelete(DeleteBehavior.Cascade);
                e.Property(x => x.ResponseDate).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                e.Property(x => x.Title).IsRequired().HasMaxLength(100);
                e.Property(x => x.Content).IsRequired().HasMaxLength(200);
                e.Property(x => x.Attachments).IsRequired().HasMaxLength(500);

            });
            modelBuilder.Entity<DoctorLicense>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Certificate).IsRequired().HasMaxLength(200);
                e.Property(x => x.LicenseNumber).IsRequired().HasMaxLength(100);
                e.Property(x => x.Biography).IsRequired().HasMaxLength(100);
                e.Property(x => x.Metadata).IsRequired().HasMaxLength(500);
                e.Property(x => x.ProfileImg).IsRequired().HasMaxLength(500);
                e.Property(x => x.Specialize).IsRequired().HasMaxLength(200);
                e.Property(x => x.Status).HasConversion<string>().HasDefaultValue(DoctorLicenseStatusEnum.Pending);
                e.HasOne(x => x.User).WithOne().HasForeignKey<DoctorLicense>(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                e.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                e.Property(x => x.UpdatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");

            });
            modelBuilder.Entity<DoctorSpecialization>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.DoctorLicense).WithMany().HasForeignKey(x => x.DoctorLicenseId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(x => x.Specialization).WithMany().HasForeignKey(x => x.SpecializtionId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<Feature>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.FeatureName).IsRequired().HasMaxLength(50);
                e.Property(x => x.Description).IsRequired().HasMaxLength(300);
                e.HasOne(x => x.CreatedUser).WithMany().HasForeignKey(x => x.CreatedBy).OnDelete(DeleteBehavior.Cascade);
                e.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                e.Property(x => x.UpdatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");

            });
            modelBuilder.Entity<GrowthRecord>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.Children).WithMany().HasForeignKey(x => x.ChildrentId).OnDelete(DeleteBehavior.Cascade);
                e.Property(x => x.Weight).IsRequired();
                e.Property(x => x.Height).IsRequired();
                e.Property(x => x.Bmi).IsRequired();
                //e.HasOne(x => x.BmiCategory).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                e.Property(x => x.BmiPercentile).IsRequired();
                e.Property(x => x.Notes).IsRequired().HasMaxLength(500);
                e.HasOne(x => x.CreatedUser).WithMany().HasForeignKey(x => x.CreatedBy).OnDelete(DeleteBehavior.Cascade);
                e.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");

            });
            modelBuilder.Entity<Package>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.PackageName).IsRequired().HasMaxLength(100);
                e.Property(x => x.Description).IsRequired().HasMaxLength(200);
                e.Property(x => x.Price).IsRequired();
                e.Property(x => x.DurationMonths).IsRequired();
                e.Property(x => x.TrialPeriodDays).IsRequired();
                e.Property(x => x.MaxChildrentAllowed).IsRequired();
                e.HasOne(x => x.CreatedUser).WithMany().HasForeignKey(x => x.CreatedBy).OnDelete(DeleteBehavior.Cascade);
                e.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                e.Property(x => x.UpdatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");

            });
            modelBuilder.Entity<PackageFeature>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.Package).WithMany().HasForeignKey(x => x.PackageId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(x => x.Feature).WithMany().HasForeignKey(x => x.FeatureId).OnDelete(DeleteBehavior.Cascade);
            });
            modelBuilder.Entity<RatingFeedback>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                e.Property(x => x.Rating).IsRequired();
                e.Property(x => x.Commnet).IsRequired(false).HasMaxLength(500);
                e.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                e.Property(x => x.UpdatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            modelBuilder.Entity<Role>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.RoleName).IsRequired().HasMaxLength(20);
                e.Property(x => x.Description).IsRequired().HasMaxLength(100);
                e.Property(x => x.Status).HasConversion<string>().HasDefaultValue(RoleStatusEnum.Pending);

            });
            //modelBuilder.Entity<Role>().HasData(new Role
            //{
            //    Id = Guid.Parse(GeneralConst.DEFAULT_GUID),
            //    RoleName = "Admin",
            //    Description = "This person have fully permission of system",
            //    Status = RoleStatusEnum.Active,
            //}, new Role
            //{
            //    Id = Guid.Parse(GeneralConst.ROLE_STAFF_GUID),
            //    RoleName = "Staff",
            //    Description = "This person have under permission of admin",
            //    Status = RoleStatusEnum.Active,
            //}, new Role
            //{
            //    Id = Guid.Parse(GeneralConst.ROLE_USER_GUID),
            //    RoleName = "User",
            //    Description = "This person have limited permission of system",
            //    Status = RoleStatusEnum.Active,
            //}, new Role
            //{
            //    Id = Guid.Parse(GeneralConst.ROLE_DOCTOR_GUID),
            //    RoleName = "Doctor",
            //    Description = "This person have permission to response user",
            //    Status = RoleStatusEnum.Active,
            //});
            modelBuilder.Entity<SharingProfile>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(x => x.Children).WithMany().HasForeignKey(x => x.ChildrentId).OnDelete(DeleteBehavior.Cascade);
                e.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            modelBuilder.Entity<Specialization>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Name).IsRequired().HasMaxLength(50);
                e.Property(x => x.Description).IsRequired().HasMaxLength(200);
                e.HasOne(x => x.CreatedUser).WithMany().HasForeignKey(x => x.CreatedBy).OnDelete(DeleteBehavior.Cascade);
                e.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                e.Property(x => x.UpdatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            modelBuilder.Entity<Transaction>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(x => x.Package).WithMany().HasForeignKey(x => x.PackageId).OnDelete(DeleteBehavior.Cascade);
                e.Property(x => x.Amount).IsRequired();
                e.Property(x => x.Currency).IsRequired().HasMaxLength(20);
                e.Property(x => x.TransactionType).IsRequired().HasMaxLength(20);
                e.Property(x => x.PaymentMethod).IsRequired().HasMaxLength(30);
                e.Property(x => x.TransactionDate).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                e.Property(x => x.PaymentDate).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                e.Property(x => x.MerchantTransactionId).IsRequired().HasMaxLength(50);
                e.Property(x => x.PaymentStatus).HasConversion<string>().HasDefaultValue(PaymentStatusEnum.Pending);
                e.Property(x => x.Description).IsRequired().HasMaxLength(300);

            });
            modelBuilder.Entity<User>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Name).IsRequired().HasMaxLength(50);
                e.Property(x => x.UserName).IsRequired().HasMaxLength(40);
                e.Property(x => x.Password).IsRequired(false).HasMaxLength(200);
                e.Property(x => x.Phone).IsRequired(false).HasMaxLength(11);
                e.HasIndex(x => x.Email).IsUnique();
                e.Property(x => x.LastLogin).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                e.Property(x => x.Address).IsRequired(false).HasMaxLength(200);
                e.Property(x => x.Avatar).IsRequired(false).HasMaxLength(1000);
                e.Property(x => x.Status).HasConversion<string>().HasDefaultValue(UserStatusEnum.NotVerified);
                e.Property(x => x.AuthType).HasConversion<string>().HasDefaultValue(AuthTypeEnum.Email);
                e.HasOne(x => x.Role).WithMany().HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Cascade);
                e.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            modelBuilder.Entity<UserPackage>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.Owner).WithMany().HasForeignKey(x => x.OwnerId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(x => x.Package).WithMany().HasForeignKey(x => x.PackageId).OnDelete(DeleteBehavior.Cascade);
                e.Property(x => x.Status).HasConversion<string>().HasDefaultValue(UserPackageStatusEnum.OnGoing);
                e.Property(x => x.StartDate).IsRequired().HasDefaultValueSql("CURRENT_DATE");
                e.Property(x => x.ExpireDate).IsRequired().HasDefaultValueSql("CURRENT_DATE");
            });
        }
    }
}
