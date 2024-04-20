﻿// <auto-generated />
using System;
using DataAccessWebAPI.DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DataAccessWebAPI.Migrations
{
    [DbContext(typeof(IntegralInvestingAppDbContext))]
    [Migration("20240420160819_AddPortfolioAndPortfolioStockModels")]
    partial class AddPortfolioAndPortfolioStockModels
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.4")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DataAccessWebAPI.Models.BankAccount", b =>
                {
                    b.Property<int>("BankAccountId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("BankAccountId"));

                    b.Property<string>("AccountName")
                        .IsRequired()
                        .HasMaxLength(60)
                        .HasColumnType("nvarchar(60)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("BankAccountId");

                    b.ToTable("BankAccounts");
                });

            modelBuilder.Entity("DataAccessWebAPI.Models.Portfolio", b =>
                {
                    b.Property<int>("PortfolioId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PortfolioId"));

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("PortfolioId");

                    b.ToTable("Portfolios");
                });

            modelBuilder.Entity("DataAccessWebAPI.Models.PortfolioStock", b =>
                {
                    b.Property<int>("PortfolioStockId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PortfolioStockId"));

                    b.Property<decimal>("CurrentPrice")
                        .HasColumnType("decimal(8, 2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("NumberOfShares")
                        .HasColumnType("int");

                    b.Property<int>("PortfolioId")
                        .HasColumnType("int");

                    b.Property<decimal>("PurchasePrice")
                        .HasColumnType("decimal(8, 2)");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("PortfolioStockId");

                    b.HasIndex("PortfolioId");

                    b.ToTable("PortfolioStocks");
                });

            modelBuilder.Entity("DataAccessWebAPI.Models.UserFund", b =>
                {
                    b.Property<int>("UserFundId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserFundId"));

                    b.Property<decimal>("CurrentFunds")
                        .HasColumnType("decimal(8, 2)");

                    b.Property<string>("CurrentTransferAccount")
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<decimal?>("CurrentTransferAmount")
                        .HasColumnType("decimal(8, 2)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("UserFundId");

                    b.ToTable("UserFunds");
                });

            modelBuilder.Entity("DataAccessWebAPI.Models.PortfolioStock", b =>
                {
                    b.HasOne("DataAccessWebAPI.Models.Portfolio", "Portfolio")
                        .WithMany("PortfolioStocks")
                        .HasForeignKey("PortfolioId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Portfolio");
                });

            modelBuilder.Entity("DataAccessWebAPI.Models.Portfolio", b =>
                {
                    b.Navigation("PortfolioStocks");
                });
#pragma warning restore 612, 618
        }
    }
}
