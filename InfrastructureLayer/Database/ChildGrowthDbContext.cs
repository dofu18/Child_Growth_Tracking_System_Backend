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

        //public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        //{
        //    var entries = ChangeTracker.Entries().Where(x => x.Entity is BaseEntity && (x.State == EntityState.Added || x.State == EntityState.Modified));
        //    foreach (var entry in entries)
        //    {
        //        if (entry.State == EntityState.Added)
        //        {
        //            ((BaseEntity)entry.Entity).CreatedAt = DateTime.UtcNow;
        //        }
        //      ((BaseEntity)entry.Entity).UpdatedAt = DateTime.UtcNow;
        //    }
        //    return base.SaveChangesAsync(cancellationToken);
        //}

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
                e.Property(x => x.FromAge).IsRequired();
                e.Property(x => x.ToAge).IsRequired();
                e.Property(x => x.CreatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
                e.Property(x => x.UpdatedAt).IsRequired().HasDefaultValueSql("CURRENT_TIMESTAMP");
            });
            modelBuilder.Entity<BmiCategory>().HasData(new BmiCategory
            {
                Id = Guid.Parse("1e567c0b-62f1-4cf7-a528-f82f623ff9ee"),
                Name = "Under Weight",
                BmiTop = 14,
                BmiBottom = 0,
                FromAge = 0,
                ToAge = 2,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }, new BmiCategory
            {
                Id = Guid.Parse("f6ed1a64-0a0e-4217-bcb4-c1179c519a79"),
                Name = "Under Weight",
                BmiTop = 13,
                BmiBottom = 0,
                FromAge = 3,
                ToAge = 5,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }, new BmiCategory
            {
                Id = Guid.Parse("3a9b3c93-e64e-4f0e-9345-2c5cccb20e82"),
                Name = "Under Weight",
                BmiTop = 14,
                BmiBottom = 0,
                FromAge = 6,
                ToAge = 10,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }, new BmiCategory
            {
                Id = Guid.Parse("2ff769f8-c61b-4e0d-9e62-c1685e1a79c2"),
                Name = "Under Weight",
                BmiTop = 14,
                BmiBottom = 0,
                FromAge = 11,
                ToAge = 15,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }, new BmiCategory
            {
                Id = Guid.Parse("61f4d3d2-6f6a-408a-9d4f-1ad84a82e150"),
                Name = "Under Weight",
                BmiTop = 13,
                BmiBottom = 0,
                FromAge = 16,
                ToAge = 100,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }, new BmiCategory
            {
                Id = Guid.Parse("01955c46-2df8-74fd-bbb1-c8c1d792ee5b"),
                Name = "Healthy Weight",
                BmiTop = 17,
                BmiBottom = 14,
                FromAge = 0,
                ToAge = 2,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }, new BmiCategory
            {
                Id = Guid.Parse("efc73434-1e42-43ae-8d14-14878e5b13b1"),
                Name = "Healthy Weight",
                BmiTop = 17,
                BmiBottom = 14,
                FromAge = 3,
                ToAge = 5,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }, new BmiCategory
            {
                Id = Guid.Parse("1bc993ae-e0a8-4eec-bf2a-c0576d62bc25"),
                Name = "Healthy Weight",
                BmiTop = 17,
                BmiBottom = 14,
                FromAge = 6,
                ToAge = 10,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }, new BmiCategory
            {
                Id = Guid.Parse("5c3fa0f5-f60b-4c4f-b2d0-0d0a4c03db43"),
                Name = "Healthy Weight",
                BmiTop = 17,
                BmiBottom = 14,
                FromAge = 11,
                ToAge = 15,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }, new BmiCategory
            {
                Id = Guid.Parse("54812287-f31f-4c8c-b90f-5e1d52782810"),
                Name = "Healthy Weight",
                BmiTop = 17,
                BmiBottom = 14,
                FromAge = 16,
                ToAge = 100,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }, new BmiCategory
            {
                Id = Guid.Parse("01955c46-5e27-7c17-97ee-a06ded033c32"),
                Name = "Over Weight",
                BmiTop = 85,
                BmiBottom = 95,
                FromAge = 0,
                ToAge = 2,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }, new BmiCategory
            {
                Id = Guid.Parse("337a2951-d570-4fa7-8a3a-264cbfe1ea0a"),
                Name = "Over Weight",
                BmiTop = 85,
                BmiBottom = 95,
                FromAge = 3,
                ToAge = 5,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }, new BmiCategory
            {
                Id = Guid.Parse("2928f86d-44c9-46ac-8c26-13a77d1bc9a2"),
                Name = "Over Weight",
                BmiTop = 85,
                BmiBottom = 95,
                FromAge = 6,
                ToAge = 10,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }, new BmiCategory
            {
                Id = Guid.Parse("2b4a6c93-7be8-41c4-b173-242b37ae3f15"),
                Name = "Over Weight",
                BmiTop = 85,
                BmiBottom = 95,
                FromAge = 11,
                ToAge = 15,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }, new BmiCategory
            {
                Id = Guid.Parse("10cf12cd-9e5f-456e-a073-1b359028123e"),
                Name = "Over Weight",
                BmiTop = 85,
                BmiBottom = 95,
                FromAge = 16,
                ToAge = 100,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }, new BmiCategory
            {
                Id = Guid.Parse("01955c46-81c0-7e96-9961-3b490a2d1c8f"),
                Name = "Obesity",
                BmiTop = 95,
                BmiBottom = 120,
                FromAge = 0,
                ToAge = 2,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }, new BmiCategory
            {
                Id = Guid.Parse("b4c1f9b4-544e-45d5-936f-83e90e9ac0a6"),
                Name = "Obesity",
                BmiTop = 95,
                BmiBottom = 120,
                FromAge = 3,
                ToAge = 5,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }, new BmiCategory
            {
                Id = Guid.Parse("a25596e3-6c7f-4e59-bac3-19f155b8a7dc"),
                Name = "Obesity",
                BmiTop = 95,
                BmiBottom = 120,
                FromAge = 6,
                ToAge = 10,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }, new BmiCategory
            {
                Id = Guid.Parse("c8fbecc3-857a-4427-a626-d38c56a147cc"),
                Name = "Obesity",
                BmiTop = 95,
                BmiBottom = 120,
                FromAge = 11,
                ToAge = 15,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }, new BmiCategory
            {
                Id = Guid.Parse("889fd61d-679f-4191-878e-f52bb3d5ec65"),
                Name = "Obesity",
                BmiTop = 95,
                BmiBottom = 120,
                FromAge = 16,
                ToAge = 100,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }, new BmiCategory
            {
                Id = Guid.Parse("01955c46-a1d5-7fe9-a779-da396903a7a4"),
                Name = "Servere Obesity",
                BmiTop = 120,
                BmiBottom = 140,
                FromAge = 0,
                ToAge = 2,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }, new BmiCategory
            {
                Id = Guid.Parse("92c0c29c-978d-43d5-b58a-3f7fd5ea1d60"),
                Name = "Servere Obesity",
                BmiTop = 120,
                BmiBottom = 140,
                FromAge = 3,
                ToAge = 5,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }, new BmiCategory
            {
                Id = Guid.Parse("4aa18090-73ec-4c41-bcfc-3f8b2f144e7e"),
                Name = "Servere Obesity",
                BmiTop = 120,
                BmiBottom = 140,
                FromAge = 6,
                ToAge = 10,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }, new BmiCategory
            {
                Id = Guid.Parse("dbbe82b4-ef1e-48b5-a6d5-d479905228d8"),
                Name = "Servere Obesity",
                BmiTop = 120,
                BmiBottom = 140,
                FromAge = 11,
                ToAge = 15,
                CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
                UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, DateTimeKind.Utc),
            }, new BmiCategory
            {
                Id = Guid.Parse("88e091a4-02ef-404e-9522-0cb7f933f03d"),
                Name = "Servere Obesity",
                BmiTop = 120,
                BmiBottom = 140,
                FromAge = 16,
                ToAge = 100,
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
                e.Property(x => x.CreatedAt).IsRequired();

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
