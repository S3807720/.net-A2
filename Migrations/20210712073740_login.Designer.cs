﻿// <auto-generated />
using System;
using MCBA_Web.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace MCBA_Web.Migrations
{
    [DbContext(typeof(McbaContext))]
    [Migration("20210712073740_login")]
    partial class login
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("MCBA_Web.Models.Account", b =>
                {
                    b.Property<int>("AccountNumber")
                        .HasColumnType("int");

                    b.Property<int>("AccountType")
                        .HasMaxLength(50)
                        .HasColumnType("int");

                    b.Property<decimal>("Balance")
                        .HasColumnType("money");

                    b.Property<int>("CustomerID")
                        .HasMaxLength(50)
                        .HasColumnType("int");

                    b.HasKey("AccountNumber");

                    b.HasIndex("CustomerID");

                    b.ToTable("Accounts");

                    b.HasCheckConstraint("CH_Account_Balance", "Balance >= 0");
                });

            modelBuilder.Entity("MCBA_Web.Models.Customer", b =>
                {
                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<string>("Address")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("City")
                        .HasMaxLength(40)
                        .HasColumnType("nvarchar(40)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PostCode")
                        .HasMaxLength(4)
                        .HasColumnType("nvarchar(4)");

                    b.HasKey("CustomerID");

                    b.ToTable("Customers");
                });

            modelBuilder.Entity("MCBA_Web.Models.Login", b =>
                {
                    b.Property<string>("LoginID")
                        .HasMaxLength(8)
                        .HasColumnType("nchar(8)");

                    b.Property<int>("CustomerID")
                        .HasColumnType("int");

                    b.Property<string>("PasswordHash")
                        .IsRequired()
                        .HasMaxLength(64)
                        .HasColumnType("nchar(64)");

                    b.HasKey("LoginID");

                    b.HasIndex("CustomerID");

                    b.ToTable("Logins");

                    b.HasCheckConstraint("CH_Login_LoginID", "len(LoginID) = 8");

                    b.HasCheckConstraint("CH_Login_PasswordHash", "len(PasswordHash) = 64");
                });

            modelBuilder.Entity("MCBA_Web.Models.Transaction", b =>
                {
                    b.Property<int>("TransactionID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AccountNumber")
                        .HasColumnType("int");

                    b.Property<decimal>("Amount")
                        .HasColumnType("money");

                    b.Property<string>("Comment")
                        .HasMaxLength(30)
                        .HasColumnType("nvarchar(30)");

                    b.Property<int?>("DestinationAccountNumber")
                        .HasColumnType("int");

                    b.Property<DateTime>("TransactionTimeUtc")
                        .HasColumnType("datetime2");

                    b.Property<int>("TransactionType")
                        .HasColumnType("int");

                    b.HasKey("TransactionID");

                    b.HasIndex("AccountNumber");

                    b.HasIndex("DestinationAccountNumber");

                    b.ToTable("Transactions");

                    b.HasCheckConstraint("CH_Transaction_Amount", "Amount > 0");
                });

            modelBuilder.Entity("MCBA_Web.Models.Account", b =>
                {
                    b.HasOne("MCBA_Web.Models.Customer", "Customer")
                        .WithMany("Accounts")
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("MCBA_Web.Models.Login", b =>
                {
                    b.HasOne("MCBA_Web.Models.Customer", "Customer")
                        .WithMany()
                        .HasForeignKey("CustomerID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Customer");
                });

            modelBuilder.Entity("MCBA_Web.Models.Transaction", b =>
                {
                    b.HasOne("MCBA_Web.Models.Account", "Account")
                        .WithMany("Transactions")
                        .HasForeignKey("AccountNumber")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("MCBA_Web.Models.Account", "DestinationAccount")
                        .WithMany()
                        .HasForeignKey("DestinationAccountNumber");

                    b.Navigation("Account");

                    b.Navigation("DestinationAccount");
                });

            modelBuilder.Entity("MCBA_Web.Models.Account", b =>
                {
                    b.Navigation("Transactions");
                });

            modelBuilder.Entity("MCBA_Web.Models.Customer", b =>
                {
                    b.Navigation("Accounts");
                });
#pragma warning restore 612, 618
        }
    }
}
