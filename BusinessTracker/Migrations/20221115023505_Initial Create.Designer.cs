﻿// <auto-generated />
using System;
using BusinessTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BusinessTracker.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20221115023505_Initial Create")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("BusinessTracker.Models.Food", b =>
                {
                    b.Property<int>("FoodId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("FoodId"), 1L, 1);

                    b.Property<string>("Icon")
                        .IsRequired()
                        .HasColumnType("nvarchar(2)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Type")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("FoodId");

                    b.ToTable("Foods");
                });

            modelBuilder.Entity("BusinessTracker.Models.Transaction", b =>
                {
                    b.Property<int>("TransactionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("TransactionId"), 1L, 1);

                    b.Property<string>("Amount")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FoodId")
                        .IsRequired()
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("TransactionId");

                    b.HasIndex("FoodId");

                    b.ToTable("Transactions");
                });

            modelBuilder.Entity("BusinessTracker.Models.Transaction", b =>
                {
                    b.HasOne("BusinessTracker.Models.Food", "Food")
                        .WithMany()
                        .HasForeignKey("FoodId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Food");
                });
#pragma warning restore 612, 618
        }
    }
}
