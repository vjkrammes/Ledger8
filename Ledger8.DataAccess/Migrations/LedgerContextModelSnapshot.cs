﻿// <auto-generated />
using System;

using Ledger8.DataAccess.EFCore;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Ledger8.DataAccess.Migrations
{
    [DbContext(typeof(LedgerContext))]
    partial class LedgerContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("Ledger8.DataAccess.Entities.AccountEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int?>("AccountNumberId")
                        .HasColumnType("int");

                    b.Property<int>("AccountTypeId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ClosedDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Comments")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<int>("Day")
                        .HasColumnType("int");

                    b.Property<int>("DueDateType")
                        .HasColumnType("int");

                    b.Property<bool>("IsClosed")
                        .HasColumnType("bit");

                    b.Property<bool>("IsPayable")
                        .HasColumnType("bit");

                    b.Property<int>("Month")
                        .HasColumnType("int");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("AccountNumberId");

                    b.HasIndex("AccountTypeId");

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("AccountTypeId"), false);

                    b.HasIndex("CompanyId");

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("CompanyId"), false);

                    b.HasIndex("Tag");

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("Tag"), false);

                    b.ToTable("Accounts");
                });

            modelBuilder.Entity("Ledger8.DataAccess.Entities.AccountNumberEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<string>("Number")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Salt")
                        .IsRequired()
                        .HasColumnType("varbinary(64)");

                    b.Property<DateTime>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("StopDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("AccountId"), false);

                    b.HasIndex("StartDate");

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("StartDate"), false);

                    b.HasIndex("StopDate");

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("StopDate"), false);

                    b.ToTable("AccountNumbers");
                });

            modelBuilder.Entity("Ledger8.DataAccess.Entities.AccountTypeEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Description")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("Description"), false);

                    b.ToTable("AccountTypes");
                });

            modelBuilder.Entity("Ledger8.DataAccess.Entities.AllotmentEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(12,2)");

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PoolId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("CompanyId"), false);

                    b.HasIndex("PoolId");

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("PoolId"), false);

                    b.ToTable("Allotments");
                });

            modelBuilder.Entity("Ledger8.DataAccess.Entities.CompanyEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("Address1")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Address2")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Comments")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsPayee")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("URL")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("Name"), false);

                    b.ToTable("Companies");
                });

            modelBuilder.Entity("Ledger8.DataAccess.Entities.IdentityEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("CompanyId")
                        .HasColumnType("int");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(64)");

                    b.Property<string>("Tag")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("URL")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("UserSalt")
                        .IsRequired()
                        .HasColumnType("varbinary(64)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("CompanyId"), false);

                    b.HasIndex("Tag");

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("Tag"), false);

                    b.ToTable("Identities");
                });

            modelBuilder.Entity("Ledger8.DataAccess.Entities.PoolEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<decimal>("Amount")
                        .HasColumnType("decimal(12,2)");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(12,2)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("Name"), false);

                    b.ToTable("Pools");
                });

            modelBuilder.Entity("Ledger8.DataAccess.Entities.SettingsEntity", b =>
                {
                    b.Property<int>("Lock")
                        .HasColumnType("int");

                    b.Property<Guid>("SystemId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("BackupDirectory")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("Hash")
                        .HasColumnType("varbinary(64)");

                    b.Property<byte[]>("Salt")
                        .HasColumnType("varbinary(64)");

                    b.Property<string>("Theme")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Lock", "SystemId");

                    b.HasIndex("Lock")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("Lock"), false);

                    b.HasIndex("SystemId")
                        .IsUnique();

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("SystemId"), false);

                    b.ToTable("Settings");

                    b.HasCheckConstraint("SettingsCheck", "[Lock] = 1");
                });

            modelBuilder.Entity("Ledger8.DataAccess.Entities.TransactionEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("AccountId")
                        .HasColumnType("int");

                    b.Property<decimal>("Balance")
                        .HasColumnType("decimal(12,2)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("date");

                    b.Property<decimal>("Payment")
                        .HasColumnType("decimal(12,2)");

                    b.Property<string>("Reference")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    SqlServerIndexBuilderExtensions.IsClustered(b.HasIndex("AccountId"), false);

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("Ledger8.DataAccess.Entities.AccountEntity", b =>
                {
                    b.HasOne("Ledger8.DataAccess.Entities.AccountNumberEntity", "AccountNumber")
                        .WithMany()
                        .HasForeignKey("AccountNumberId");

                    b.HasOne("Ledger8.DataAccess.Entities.AccountTypeEntity", "AccountType")
                        .WithMany()
                        .HasForeignKey("AccountTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AccountNumber");

                    b.Navigation("AccountType");
                });

            modelBuilder.Entity("Ledger8.DataAccess.Entities.AllotmentEntity", b =>
                {
                    b.HasOne("Ledger8.DataAccess.Entities.CompanyEntity", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Ledger8.DataAccess.Entities.IdentityEntity", b =>
                {
                    b.HasOne("Ledger8.DataAccess.Entities.CompanyEntity", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });
#pragma warning restore 612, 618
        }
    }
}
