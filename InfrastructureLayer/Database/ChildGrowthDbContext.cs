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
        public DbSet<GrowthRecord> GrowthRecords { get; set; }
        public DbSet<Package> Packages { get; set; }
        public DbSet<RatingFeedback> RatingFeedbacks { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SharingProfile> SharingProfiles { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserPackage> UserPackages { get; set; }
        public DbSet<WhoData> WhoData { get; set; }

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

            modelBuilder.Entity<WhoData>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.AgeMonth).IsRequired();
                e.Property(x => x.BmiPercentile).IsRequired();
                e.Property(x => x.Bmi).IsRequired();
                e.Property(x => x.Gender).HasConversion<string>().HasDefaultValue(GenderEnum.Male);
                e.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                e.Property(x => x.UpdatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            modelBuilder.Entity<Alert>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.Children).WithMany().HasForeignKey(x => x.ChildrentId).OnDelete(DeleteBehavior.Cascade);
                e.Property(x => x.AlertDate).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                e.Property(x => x.Message).IsRequired(false).HasMaxLength(500);
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
            modelBuilder.Entity<BmiCategory>().HasData(new BmiCategory
            {
                Id = Guid.Parse("01955c45-f781-7835-8d4b-aff20764aca6"),
                Name = "Under Weight",
                BmiTop = 5,
                BmiBottom = 0,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }, new BmiCategory
            {
                Id = Guid.Parse("01955c46-2df8-74fd-bbb1-c8c1d792ee5b"),
                Name = "Healthy Weight",
                BmiTop = 85,
                BmiBottom = 5,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }, new BmiCategory
            {
                Id = Guid.Parse("01955c46-5e27-7c17-97ee-a06ded033c32"),
                Name = "Over Weight",
                BmiTop = 85,
                BmiBottom = 95,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }, new BmiCategory
            {
                Id = Guid.Parse("01955c46-81c0-7e96-9961-3b490a2d1c8f"),
                Name = "Obesity",
                BmiTop = 95,
                BmiBottom = 120,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }, new BmiCategory
            {
                Id = Guid.Parse("01955c46-a1d5-7fe9-a779-da396903a7a4"),
                Name = "Servere Obesity",
                BmiTop = 120,
                BmiBottom = 140,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            });
            modelBuilder.Entity<Children>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.Parent).WithMany().HasForeignKey(x => x.ParentId).OnDelete(DeleteBehavior.Cascade);
                e.Property(x => x.Name).IsRequired().HasMaxLength(50);
                e.Property(x => x.DoB).IsRequired().HasDefaultValueSql("CURRENT_DATE");
                e.Property(x => x.Status).HasConversion<string>().HasDefaultValue(ChildrentStatusEnum.Active);
                e.Property(x => x.Gender).HasConversion<string>().HasDefaultValue(GenderEnum.Male);
                e.Property(x => x.Weight).IsRequired();
                e.Property(x => x.Height).IsRequired();
                e.Property(x => x.Bmi).IsRequired();
                e.HasOne(x => x.BmiCategory).WithMany().HasForeignKey(x => x.BmiCategoryId).OnDelete(DeleteBehavior.Cascade);
                e.Property(x => x.BmiPercentile).IsRequired();
                e.Property(x => x.Notes).IsRequired(false).HasMaxLength(500);
                e.Property(x => x.GroupAge).HasConversion<string>().HasDefaultValue(GroupAgeEnum.From2to19);
                e.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                e.Property(x => x.UpdatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");

            });
            modelBuilder.Entity<ConsultationRequest>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.RequestDate).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                e.Property(x => x.Title).IsRequired().HasMaxLength(100);
                e.Property(x => x.Description).IsRequired(false).HasMaxLength(500);
                e.Property(x => x.Status).HasConversion<string>().HasDefaultValue(ConsultationRequestStatusEnum.Pending);
                e.Property(x => x.Attachments).IsRequired(false).HasMaxLength(500);
                e.HasOne(x => x.UserRequest).WithMany().HasForeignKey(x => x.UserRequestId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(x => x.DoctorReceive).WithMany().HasForeignKey(x => x.DoctorReceiveId).OnDelete(DeleteBehavior.Cascade);
                e.Property(x => x.UpdatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");

            });
            modelBuilder.Entity<ConsultationResponse>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.ConsultationRequest).WithMany().HasForeignKey(x => x.RequestId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(x => x.Doctor).WithMany().HasForeignKey(x => x.DoctorId).OnDelete(DeleteBehavior.Cascade);
                e.Property(x => x.ResponseDate).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                e.Property(x => x.Title).IsRequired().HasMaxLength(100);
                e.Property(x => x.Content).IsRequired(false).HasMaxLength(200);
                e.Property(x => x.Attachments).IsRequired(false).HasMaxLength(500);

            });
            modelBuilder.Entity<DoctorLicense>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Certificate).IsRequired(false).HasMaxLength(200);
                e.Property(x => x.LicenseNumber).IsRequired().HasMaxLength(100);
                e.Property(x => x.Biography).IsRequired(false).HasMaxLength(100);
                e.Property(x => x.Metadata).IsRequired(false).HasMaxLength(500);
                e.Property(x => x.ProfileImg).IsRequired(false).HasMaxLength(500);
                e.Property(x => x.Specialize).IsRequired(false).HasMaxLength(200);
                e.Property(x => x.Status).HasConversion<string>().HasDefaultValue(DoctorLicenseStatusEnum.Pending);
                e.HasOne(x => x.User).WithOne().HasForeignKey<DoctorLicense>(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                e.Property(x => x.RatingAvg).IsRequired();
                e.Property(x => x.Degrees).IsRequired(false).HasMaxLength(500);
                e.Property(x => x.Research).IsRequired(false).HasMaxLength(500);
                e.Property(x => x.Languages).IsRequired(false).HasMaxLength(500);
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
                e.Property(x => x.Notes).IsRequired(false).HasMaxLength(500);
                e.HasOne(x => x.CreatedUser).WithMany().HasForeignKey(x => x.CreatedBy).OnDelete(DeleteBehavior.Cascade);
                e.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");

            });
            modelBuilder.Entity<Package>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.PackageName).IsRequired().HasMaxLength(100);
                e.Property(x => x.Description).IsRequired(false).HasMaxLength(200);
                e.Property(x => x.Price).IsRequired();
                e.Property(x => x.BillingCycle).HasConversion<string>().HasDefaultValue(BillingCycleEnum.Monthly);
                e.Property(x => x.MaxChildrentAllowed).IsRequired();
                e.Property(x => x.Status).HasConversion<string>().HasDefaultValue(PackageStatusEnum.Pending);
                e.HasOne(x => x.CreatedUser).WithMany().HasForeignKey(x => x.CreatedBy).OnDelete(DeleteBehavior.Cascade);
                e.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                e.Property(x => x.UpdatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");

            });
            modelBuilder.Entity<RatingFeedback>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                e.Property(x => x.Rating).IsRequired();
                e.Property(x => x.Feedback).IsRequired(false).HasMaxLength(500);
                e.Property(x => x.Status).HasConversion<string>().HasDefaultValue(RatingFeedbackStatusEnum.Publish);
                e.Property(x => x.RatingType).HasConversion<string>().HasDefaultValue(RatingTypeEnum.Doctor);
                e.HasOne(x => x.Doctor).WithMany().HasForeignKey(x => x.DoctorId).OnDelete(DeleteBehavior.Cascade);
                e.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                e.Property(x => x.UpdatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            modelBuilder.Entity<Role>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.RoleName).IsRequired().HasMaxLength(20);
                e.Property(x => x.Description).IsRequired(false).HasMaxLength(100);
                e.Property(x => x.Status).HasConversion<string>().HasDefaultValue(RoleStatusEnum.Pending);

            });
            modelBuilder.Entity<Role>().HasData(new Role
            {
                Id = Guid.Parse(GeneralConst.ADMIN_GUID),
                RoleName = "Admin",
                Description = "This person have fully permission of system",
                Status = RoleStatusEnum.Active,
            }, new Role
            {
                Id = Guid.Parse(GeneralConst.ROLE_STAFF_GUID),
                RoleName = "Staff",
                Description = "This person have under permission of admin",
                Status = RoleStatusEnum.Active,
            }, new Role
            {
                Id = Guid.Parse(GeneralConst.ROLE_USER_GUID),
                RoleName = "User",
                Description = "This person have limited permission of system",
                Status = RoleStatusEnum.Active,
            }, new Role
            {
                Id = Guid.Parse(GeneralConst.ROLE_DOCTOR_GUID),
                RoleName = "Doctor",
                Description = "This person have permission to response user",
                Status = RoleStatusEnum.Active,
            });
            modelBuilder.Entity<SharingProfile>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(x => x.Children).WithMany().HasForeignKey(x => x.ChildrentId).OnDelete(DeleteBehavior.Cascade);
                e.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
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
                e.Property(x => x.IsTrial).IsRequired().HasDefaultValue(false);
                e.HasOne(x => x.Role).WithMany().HasForeignKey(x => x.RoleId).OnDelete(DeleteBehavior.Cascade);
                e.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            modelBuilder.Entity<UserPackage>(e =>
            {
                e.HasKey(x => x.Id);
                e.HasOne(x => x.Owner).WithMany().HasForeignKey(x => x.OwnerId).OnDelete(DeleteBehavior.Cascade);
                e.HasOne(x => x.Package).WithMany().HasForeignKey(x => x.PackageId).OnDelete(DeleteBehavior.Cascade);
                e.Property(x => x.PriceAtSubscription).IsRequired();
                e.Property(x => x.MaxChildrentAllowed).IsRequired();
                e.Property(x => x.Status).HasConversion<string>().HasDefaultValue(UserPackageStatusEnum.OnGoing);
                e.Property(x => x.StartDate).IsRequired().HasDefaultValueSql("CURRENT_DATE");
                e.Property(x => x.ExpireDate).IsRequired().HasDefaultValueSql("CURRENT_DATE");
            });
        }
    }
}
