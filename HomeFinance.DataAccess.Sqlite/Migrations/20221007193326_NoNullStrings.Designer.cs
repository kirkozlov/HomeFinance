﻿// <auto-generated />
using System;
using HomeFinance.DataAccess.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HomeFinance.DataAccess.Sqlite.Migrations
{
    [DbContext(typeof(HomeFinanceContext))]
    [Migration("20221007193326_NoNullStrings")]
    partial class NoNullStrings
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "6.0.7");

            modelBuilder.Entity("HomeFinanace.DataAccess.Core.DBModels.Operation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<double>("Amount")
                        .HasColumnType("REAL");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("TEXT");

                    b.Property<string>("HomeFinanceUserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("OperationType")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("WalletId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("WalletIdTo")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("WalletToId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("HomeFinanceUserId");

                    b.HasIndex("WalletId");

                    b.HasIndex("WalletToId");

                    b.ToTable("Operations");
                });

            modelBuilder.Entity("HomeFinanace.DataAccess.Core.DBModels.RepeatableOperation", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<double>("Amount")
                        .HasColumnType("REAL");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("HomeFinanceUserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("NextExecution")
                        .HasColumnType("TEXT");

                    b.Property<int>("OperationType")
                        .HasColumnType("INTEGER");

                    b.Property<int>("RepeatableType")
                        .HasColumnType("INTEGER");

                    b.Property<Guid>("WalletId")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("WalletIdTo")
                        .HasColumnType("TEXT");

                    b.Property<Guid?>("WalletToId")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("HomeFinanceUserId");

                    b.HasIndex("WalletId");

                    b.HasIndex("WalletToId");

                    b.ToTable("RepeatableOperations");
                });

            modelBuilder.Entity("HomeFinanace.DataAccess.Core.DBModels.Tag", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("HomeFinanceUserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Name");

                    b.HasIndex("HomeFinanceUserId");

                    b.ToTable("Tags");
                });

            modelBuilder.Entity("HomeFinanace.DataAccess.Core.DBModels.Wallet", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("TEXT");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("HomeFinanceUserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("HomeFinanceUserId");

                    b.ToTable("Wallets");
                });

            modelBuilder.Entity("HomeFinance.Domain.Models.HomeFinanceUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("INTEGER");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("TEXT");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("TEXT");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("INTEGER");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("TEXT");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("INTEGER");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("ClaimType")
                        .HasColumnType("TEXT");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("TEXT");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("TEXT");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("RoleId")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("TEXT");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .HasColumnType("TEXT");

                    b.Property<string>("Value")
                        .HasColumnType("TEXT");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("OperationTag", b =>
                {
                    b.Property<Guid>("OperationsId")
                        .HasColumnType("TEXT");

                    b.Property<string>("TagsName")
                        .HasColumnType("TEXT");

                    b.HasKey("OperationsId", "TagsName");

                    b.HasIndex("TagsName");

                    b.ToTable("OperationTag");
                });

            modelBuilder.Entity("RepeatableOperationTag", b =>
                {
                    b.Property<Guid>("RepeatableOperationId")
                        .HasColumnType("TEXT");

                    b.Property<string>("TagsName")
                        .HasColumnType("TEXT");

                    b.HasKey("RepeatableOperationId", "TagsName");

                    b.HasIndex("TagsName");

                    b.ToTable("RepeatableOperationTag");
                });

            modelBuilder.Entity("HomeFinanace.DataAccess.Core.DBModels.Operation", b =>
                {
                    b.HasOne("HomeFinance.Domain.Models.HomeFinanceUser", "HomeFinanceUser")
                        .WithMany()
                        .HasForeignKey("HomeFinanceUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HomeFinanace.DataAccess.Core.DBModels.Wallet", "Wallet")
                        .WithMany()
                        .HasForeignKey("WalletId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("HomeFinanace.DataAccess.Core.DBModels.Wallet", "WalletTo")
                        .WithMany()
                        .HasForeignKey("WalletToId");

                    b.Navigation("HomeFinanceUser");

                    b.Navigation("Wallet");

                    b.Navigation("WalletTo");
                });

            modelBuilder.Entity("HomeFinanace.DataAccess.Core.DBModels.RepeatableOperation", b =>
                {
                    b.HasOne("HomeFinance.Domain.Models.HomeFinanceUser", "HomeFinanceUser")
                        .WithMany()
                        .HasForeignKey("HomeFinanceUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HomeFinanace.DataAccess.Core.DBModels.Wallet", "Wallet")
                        .WithMany()
                        .HasForeignKey("WalletId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired();

                    b.HasOne("HomeFinanace.DataAccess.Core.DBModels.Wallet", "WalletTo")
                        .WithMany()
                        .HasForeignKey("WalletToId");

                    b.Navigation("HomeFinanceUser");

                    b.Navigation("Wallet");

                    b.Navigation("WalletTo");
                });

            modelBuilder.Entity("HomeFinanace.DataAccess.Core.DBModels.Tag", b =>
                {
                    b.HasOne("HomeFinance.Domain.Models.HomeFinanceUser", "HomeFinanceUser")
                        .WithMany()
                        .HasForeignKey("HomeFinanceUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HomeFinanceUser");
                });

            modelBuilder.Entity("HomeFinanace.DataAccess.Core.DBModels.Wallet", b =>
                {
                    b.HasOne("HomeFinance.Domain.Models.HomeFinanceUser", "HomeFinanceUser")
                        .WithMany()
                        .HasForeignKey("HomeFinanceUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("HomeFinanceUser");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("HomeFinance.Domain.Models.HomeFinanceUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("HomeFinance.Domain.Models.HomeFinanceUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HomeFinance.Domain.Models.HomeFinanceUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("HomeFinance.Domain.Models.HomeFinanceUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("OperationTag", b =>
                {
                    b.HasOne("HomeFinanace.DataAccess.Core.DBModels.Operation", null)
                        .WithMany()
                        .HasForeignKey("OperationsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HomeFinanace.DataAccess.Core.DBModels.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("RepeatableOperationTag", b =>
                {
                    b.HasOne("HomeFinanace.DataAccess.Core.DBModels.RepeatableOperation", null)
                        .WithMany()
                        .HasForeignKey("RepeatableOperationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("HomeFinanace.DataAccess.Core.DBModels.Tag", null)
                        .WithMany()
                        .HasForeignKey("TagsName")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
