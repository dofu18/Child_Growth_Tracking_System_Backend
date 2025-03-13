﻿// <auto-generated />
using System;
using InfrastructureLayer.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace InfrastructureLayer.Migrations
{
    [DbContext(typeof(ChildGrowthDbContext))]
    [Migration("20250313163959_UserPackageManage")]
    partial class UserPackageManage
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("DomainLayer.Entities.Alert", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("AlertDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<Guid>("ChildrentId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsRead")
                        .HasColumnType("boolean");

                    b.Property<string>("Message")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<Guid>("ReveivedUserId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("ChildrentId");

                    b.HasIndex("ReveivedUserId");

                    b.ToTable("Alerts");
                });

            modelBuilder.Entity("DomainLayer.Entities.BmiCategory", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("BmiBottom")
                        .HasColumnType("numeric");

                    b.Property<decimal>("BmiTop")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.HasKey("Id");

                    b.ToTable("BmiCategories");

                    b.HasData(
                        new
                        {
                            Id = new Guid("01955c45-f781-7835-8d4b-aff20764aca6"),
                            BmiBottom = 0m,
                            BmiTop = 5m,
                            CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Under Weight",
                            UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                        },
                        new
                        {
                            Id = new Guid("01955c46-2df8-74fd-bbb1-c8c1d792ee5b"),
                            BmiBottom = 5m,
                            BmiTop = 85m,
                            CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Healthy Weight",
                            UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                        },
                        new
                        {
                            Id = new Guid("01955c46-5e27-7c17-97ee-a06ded033c32"),
                            BmiBottom = 95m,
                            BmiTop = 85m,
                            CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Over Weight",
                            UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                        },
                        new
                        {
                            Id = new Guid("01955c46-81c0-7e96-9961-3b490a2d1c8f"),
                            BmiBottom = 120m,
                            BmiTop = 95m,
                            CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Obesity",
                            UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                        },
                        new
                        {
                            Id = new Guid("01955c46-a1d5-7fe9-a779-da396903a7a4"),
                            BmiBottom = 140m,
                            BmiTop = 120m,
                            CreatedAt = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Name = "Servere Obesity",
                            UpdatedAt = new DateTime(2025, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc)
                        });
                });

            modelBuilder.Entity("DomainLayer.Entities.Children", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Bmi")
                        .HasColumnType("numeric");

                    b.Property<Guid>("BmiCategoryId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("BmiPercentile")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateOnly>("DoB")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasDefaultValueSql("CURRENT_DATE");

                    b.Property<string>("Gender")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("Male");

                    b.Property<string>("GroupAge")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("From2to19");

                    b.Property<decimal>("Height")
                        .HasColumnType("numeric");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Notes")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<Guid>("ParentId")
                        .HasColumnType("uuid");

                    b.Property<string>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("Active");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<decimal>("Weight")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("BmiCategoryId");

                    b.HasIndex("ParentId");

                    b.ToTable("Children");
                });

            modelBuilder.Entity("DomainLayer.Entities.ConsultationRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Attachments")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<Guid>("ChildrentId")
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<Guid>("DoctorReceiveId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("RequestDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("Pending");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<Guid>("UserRequestId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ChildrentId");

                    b.HasIndex("DoctorReceiveId");

                    b.HasIndex("UserRequestId");

                    b.ToTable("ConsultationRequests");
                });

            modelBuilder.Entity("DomainLayer.Entities.ConsultationResponse", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Attachments")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Content")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("DoctorId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("RequestId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("ResponseDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("DoctorId");

                    b.HasIndex("RequestId");

                    b.ToTable("ConsultationResponses");
                });

            modelBuilder.Entity("DomainLayer.Entities.DoctorLicense", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Biography")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Certificate")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Degrees")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Languages")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("LicenseNumber")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("Metadata")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("ProfileImg")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<int>("RatingAvg")
                        .HasColumnType("integer");

                    b.Property<string>("Research")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<string>("Specialize")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("Pending");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("UserId")
                        .IsUnique();

                    b.ToTable("DoctorLicense");
                });

            modelBuilder.Entity("DomainLayer.Entities.GrowthRecord", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Bmi")
                        .HasColumnType("numeric");

                    b.Property<Guid>("BmiCategory")
                        .HasColumnType("uuid");

                    b.Property<decimal>("BmiPercentile")
                        .HasColumnType("numeric");

                    b.Property<Guid>("ChildrentId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<decimal>("Height")
                        .HasColumnType("numeric");

                    b.Property<string>("Notes")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<decimal>("Weight")
                        .HasColumnType("numeric");

                    b.HasKey("Id");

                    b.HasIndex("ChildrentId");

                    b.HasIndex("CreatedBy");

                    b.ToTable("GrowthRecords");
                });

            modelBuilder.Entity("DomainLayer.Entities.Package", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("BillingCycle")
                        .IsRequired()
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("Monthly");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<Guid>("CreatedBy")
                        .HasColumnType("uuid");

                    b.Property<string>("Description")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<int>("MaxChildrentAllowed")
                        .HasColumnType("integer");

                    b.Property<string>("PackageName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<decimal>("Price")
                        .HasColumnType("numeric");

                    b.Property<string>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("Pending");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.HasKey("Id");

                    b.HasIndex("CreatedBy");

                    b.ToTable("Packages");
                });

            modelBuilder.Entity("DomainLayer.Entities.RatingFeedback", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<Guid?>("DoctorId")
                        .HasColumnType("uuid");

                    b.Property<string>("Feedback")
                        .HasMaxLength(500)
                        .HasColumnType("character varying(500)");

                    b.Property<int>("Rating")
                        .HasColumnType("integer");

                    b.Property<string>("RatingType")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("Doctor");

                    b.Property<string>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("Publish");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("DoctorId");

                    b.HasIndex("UserId");

                    b.ToTable("RatingFeedbacks");
                });

            modelBuilder.Entity("DomainLayer.Entities.Role", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)");

                    b.Property<string>("RoleName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("Pending");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.ToTable("Roles");

                    b.HasData(
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000001"),
                            Description = "This person have fully permission of system",
                            RoleName = "Admin",
                            Status = "Active"
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000002"),
                            Description = "This person have under permission of admin",
                            RoleName = "Staff",
                            Status = "Active"
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000004"),
                            Description = "This person have limited permission of system",
                            RoleName = "User",
                            Status = "Active"
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000003"),
                            Description = "This person have permission to response user",
                            RoleName = "Doctor",
                            Status = "Active"
                        });
                });

            modelBuilder.Entity("DomainLayer.Entities.SharingProfile", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<Guid>("ChildrentId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("ChildrentId");

                    b.HasIndex("UserId");

                    b.ToTable("SharingProfiles");
                });

            modelBuilder.Entity("DomainLayer.Entities.Transaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(300)
                        .HasColumnType("character varying(300)");

                    b.Property<string>("MerchantTransactionId")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<Guid>("PackageId")
                        .HasColumnType("uuid");

                    b.Property<DateTime>("PaymentDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("PaymentMethod")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("PaymentStatus")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("Pending");

                    b.Property<DateTime>("TransactionDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("TransactionType")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uuid");

                    b.HasKey("Id");

                    b.HasIndex("PackageId");

                    b.HasIndex("UserId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("DomainLayer.Entities.User", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<string>("Address")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("AuthType")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("Email");

                    b.Property<string>("Avatar")
                        .HasMaxLength(1000)
                        .HasColumnType("character varying(1000)");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsTrial")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("boolean")
                        .HasDefaultValue(false);

                    b.Property<DateTime>("LastLogin")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("timestamp with time zone")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<string>("Password")
                        .HasMaxLength(200)
                        .HasColumnType("character varying(200)");

                    b.Property<string>("Phone")
                        .HasMaxLength(11)
                        .HasColumnType("character varying(11)");

                    b.Property<Guid>("RoleId")
                        .HasColumnType("uuid");

                    b.Property<string>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("NotVerified");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasMaxLength(40)
                        .HasColumnType("character varying(40)");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DomainLayer.Entities.UserPackage", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid");

                    b.Property<DateTime?>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateOnly>("ExpireDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasDefaultValueSql("CURRENT_DATE");

                    b.Property<Guid>("OwnerId")
                        .HasColumnType("uuid");

                    b.Property<Guid>("PackageId")
                        .HasColumnType("uuid");

                    b.Property<decimal>("PriceAtSubscription")
                        .HasColumnType("numeric");

                    b.Property<DateOnly>("StartDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("date")
                        .HasDefaultValueSql("CURRENT_DATE");

                    b.Property<string>("Status")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("text")
                        .HasDefaultValue("OnGoing");

                    b.Property<DateTime?>("UpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.HasIndex("PackageId");

                    b.ToTable("UserPackages");
                });

            modelBuilder.Entity("DomainLayer.Entities.Alert", b =>
                {
                    b.HasOne("DomainLayer.Entities.Children", "Children")
                        .WithMany()
                        .HasForeignKey("ChildrentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DomainLayer.Entities.User", "ReceivedUser")
                        .WithMany()
                        .HasForeignKey("ReveivedUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Children");

                    b.Navigation("ReceivedUser");
                });

            modelBuilder.Entity("DomainLayer.Entities.Children", b =>
                {
                    b.HasOne("DomainLayer.Entities.BmiCategory", "BmiCategory")
                        .WithMany()
                        .HasForeignKey("BmiCategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DomainLayer.Entities.User", "Parent")
                        .WithMany()
                        .HasForeignKey("ParentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("BmiCategory");

                    b.Navigation("Parent");
                });

            modelBuilder.Entity("DomainLayer.Entities.ConsultationRequest", b =>
                {
                    b.HasOne("DomainLayer.Entities.Children", "Children")
                        .WithMany()
                        .HasForeignKey("ChildrentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DomainLayer.Entities.User", "DoctorReceive")
                        .WithMany()
                        .HasForeignKey("DoctorReceiveId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DomainLayer.Entities.User", "UserRequest")
                        .WithMany()
                        .HasForeignKey("UserRequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Children");

                    b.Navigation("DoctorReceive");

                    b.Navigation("UserRequest");
                });

            modelBuilder.Entity("DomainLayer.Entities.ConsultationResponse", b =>
                {
                    b.HasOne("DomainLayer.Entities.User", "Doctor")
                        .WithMany()
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DomainLayer.Entities.ConsultationRequest", "ConsultationRequest")
                        .WithMany()
                        .HasForeignKey("RequestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ConsultationRequest");

                    b.Navigation("Doctor");
                });

            modelBuilder.Entity("DomainLayer.Entities.DoctorLicense", b =>
                {
                    b.HasOne("DomainLayer.Entities.User", "User")
                        .WithOne()
                        .HasForeignKey("DomainLayer.Entities.DoctorLicense", "UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("DomainLayer.Entities.GrowthRecord", b =>
                {
                    b.HasOne("DomainLayer.Entities.Children", "Children")
                        .WithMany()
                        .HasForeignKey("ChildrentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DomainLayer.Entities.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedBy")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Children");

                    b.Navigation("CreatedUser");
                });

            modelBuilder.Entity("DomainLayer.Entities.Package", b =>
                {
                    b.HasOne("DomainLayer.Entities.User", "CreatedUser")
                        .WithMany()
                        .HasForeignKey("CreatedBy")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("CreatedUser");
                });

            modelBuilder.Entity("DomainLayer.Entities.RatingFeedback", b =>
                {
                    b.HasOne("DomainLayer.Entities.DoctorLicense", "Doctor")
                        .WithMany()
                        .HasForeignKey("DoctorId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("DomainLayer.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Doctor");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DomainLayer.Entities.SharingProfile", b =>
                {
                    b.HasOne("DomainLayer.Entities.Children", "Children")
                        .WithMany()
                        .HasForeignKey("ChildrentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DomainLayer.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Children");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DomainLayer.Entities.Transaction", b =>
                {
                    b.HasOne("DomainLayer.Entities.Package", "Package")
                        .WithMany()
                        .HasForeignKey("PackageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DomainLayer.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Package");

                    b.Navigation("User");
                });

            modelBuilder.Entity("DomainLayer.Entities.User", b =>
                {
                    b.HasOne("DomainLayer.Entities.Role", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Role");
                });

            modelBuilder.Entity("DomainLayer.Entities.UserPackage", b =>
                {
                    b.HasOne("DomainLayer.Entities.User", "Owner")
                        .WithMany()
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("DomainLayer.Entities.Package", "Package")
                        .WithMany()
                        .HasForeignKey("PackageId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");

                    b.Navigation("Package");
                });
#pragma warning restore 612, 618
        }
    }
}
