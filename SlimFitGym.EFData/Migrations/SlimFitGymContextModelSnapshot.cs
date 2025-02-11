﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SlimFitGym.EFData;

#nullable disable

namespace SlimFitGym.EFData.Migrations
{
    [DbContext(typeof(SlimFitGymContext))]
    partial class SlimFitGymContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.1");

            modelBuilder.Entity("SlimFitGym.Models.Models.Account", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(320)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("TEXT");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(16)
                        .HasColumnType("TEXT");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<bool>("isActive")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Email")
                        .IsUnique();

                    b.ToTable("Accounts", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Email = "admin@gmail.com",
                            Name = "admin",
                            Password = "admin",
                            Phone = "+36123456789",
                            Role = "admin",
                            isActive = true
                        },
                        new
                        {
                            Id = 2,
                            Email = "kazmer@gmail.com",
                            Name = "kazmer",
                            Password = "kazmer",
                            Phone = "+36123456799",
                            Role = "trainer",
                            isActive = true
                        },
                        new
                        {
                            Id = 3,
                            Email = "pista@gmail.com",
                            Name = "pista",
                            Password = "pista",
                            Phone = "+36123456788",
                            Role = "user",
                            isActive = true
                        });
                });

            modelBuilder.Entity("SlimFitGym.Models.Models.Benefit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("BenefitName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("BenefitName")
                        .IsUnique();

                    b.ToTable("Benefits", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BenefitName = "Legjobb"
                        });
                });

            modelBuilder.Entity("SlimFitGym.Models.Models.Machine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Machines", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "Bicikli"
                        },
                        new
                        {
                            Id = 2,
                            Name = "Futópad"
                        });
                });

            modelBuilder.Entity("SlimFitGym.Models.Models.Pass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Days")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsHighlighted")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaxEntries")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Passes", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Days = 30,
                            IsActive = true,
                            IsHighlighted = false,
                            MaxEntries = 10,
                            Name = "Havi",
                            Price = 15000m
                        });
                });

            modelBuilder.Entity("SlimFitGym.Models.Models.PassAndBenefit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("BenefitId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PassId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("BenefitId");

                    b.HasIndex("PassId");

                    b.ToTable("PassesAndBenefits", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            BenefitId = 1,
                            PassId = 1
                        });
                });

            modelBuilder.Entity("SlimFitGym.Models.Models.Purchase", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AccountId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("PassId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("PurchaseDate")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("PassId");

                    b.ToTable("Purchases", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AccountId = 1,
                            PassId = 1,
                            PurchaseDate = new DateTime(2025, 2, 6, 17, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("SlimFitGym.Models.Models.Reservation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AccountId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TrainingId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("AccountId");

                    b.HasIndex("TrainingId");

                    b.ToTable("Reservations", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            AccountId = 3,
                            TrainingId = 1
                        });
                });

            modelBuilder.Entity("SlimFitGym.Models.Models.Room", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasMaxLength(500)
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<int>("RecommendedPeople")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("Rooms", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Description = "",
                            Name = "Futós szoba",
                            RecommendedPeople = 15
                        },
                        new
                        {
                            Id = 2,
                            Description = "",
                            Name = "Biciklizős Szoba",
                            RecommendedPeople = 10
                        });
                });

            modelBuilder.Entity("SlimFitGym.Models.Models.RoomAndMachine", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("MachineCount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MachineId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RoomId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("MachineId");

                    b.HasIndex("RoomId");

                    b.ToTable("RoomsAndMachines", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            MachineCount = 1,
                            MachineId = 1,
                            RoomId = 2
                        },
                        new
                        {
                            Id = 2,
                            MachineCount = 4,
                            MachineId = 2,
                            RoomId = 1
                        });
                });

            modelBuilder.Entity("SlimFitGym.Models.Models.TrainerApplicant", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AccountId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("TrainerApplicants", (string)null);
                });

            modelBuilder.Entity("SlimFitGym.Models.Models.Training", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<bool>("IsActive")
                        .HasColumnType("INTEGER");

                    b.Property<int>("MaxPeople")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<int>("RoomId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TrainerId")
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("TrainingEnd")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("TrainingStart")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Training", (string)null);

                    b.HasData(
                        new
                        {
                            Id = 1,
                            IsActive = true,
                            MaxPeople = 1,
                            Name = "TRX edzés",
                            RoomId = 1,
                            TrainerId = 2,
                            TrainingEnd = new DateTime(2025, 2, 6, 18, 0, 0, 0, DateTimeKind.Unspecified),
                            TrainingStart = new DateTime(2025, 2, 6, 17, 0, 0, 0, DateTimeKind.Unspecified)
                        });
                });

            modelBuilder.Entity("SlimFitGym.Models.Models.PassAndBenefit", b =>
                {
                    b.HasOne("SlimFitGym.Models.Models.Benefit", "Benefit")
                        .WithMany()
                        .HasForeignKey("BenefitId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SlimFitGym.Models.Models.Pass", "Pass")
                        .WithMany()
                        .HasForeignKey("PassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Benefit");

                    b.Navigation("Pass");
                });

            modelBuilder.Entity("SlimFitGym.Models.Models.Purchase", b =>
                {
                    b.HasOne("SlimFitGym.Models.Models.Pass", "Pass")
                        .WithMany()
                        .HasForeignKey("PassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Pass");
                });

            modelBuilder.Entity("SlimFitGym.Models.Models.Reservation", b =>
                {
                    b.HasOne("SlimFitGym.Models.Models.Account", "Account")
                        .WithMany()
                        .HasForeignKey("AccountId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SlimFitGym.Models.Models.Training", "Training")
                        .WithMany()
                        .HasForeignKey("TrainingId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Account");

                    b.Navigation("Training");
                });

            modelBuilder.Entity("SlimFitGym.Models.Models.RoomAndMachine", b =>
                {
                    b.HasOne("SlimFitGym.Models.Models.Machine", "Machine")
                        .WithMany()
                        .HasForeignKey("MachineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("SlimFitGym.Models.Models.Room", "Room")
                        .WithMany()
                        .HasForeignKey("RoomId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Machine");

                    b.Navigation("Room");
                });
#pragma warning restore 612, 618
        }
    }
}
