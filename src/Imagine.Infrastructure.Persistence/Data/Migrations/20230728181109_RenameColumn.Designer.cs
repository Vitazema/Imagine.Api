﻿// <auto-generated />
using System;
using Imagine.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Imagine.Infrastructure.Persistence.Data.Migrations
{
    [DbContext(typeof(ArtDbContext))]
    [Migration("20230728181109_RenameColumn")]
    partial class RenameColumn
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Imagine.Core.Entities.Art", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ArtSetting")
                        .HasColumnType("text");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Model")
                        .HasColumnType("text");

                    b.Property<string>("NegativePrompt")
                        .HasColumnType("text");

                    b.Property<int>("Progress")
                        .HasColumnType("integer");

                    b.Property<string>("Prompt")
                        .HasColumnType("text");

                    b.Property<Guid>("TaskId")
                        .HasColumnType("uuid");

                    b.Property<string>("Title")
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.Property<string>("Url")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("Arts");
                });

            modelBuilder.Entity("Imagine.Core.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("FullName")
                        .HasColumnType("text");

                    b.Property<int>("Role")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CreatedAt = new DateTime(2023, 7, 28, 18, 11, 8, 988, DateTimeKind.Utc).AddTicks(2086),
                            FullName = "System",
                            Role = 0
                        },
                        new
                        {
                            Id = 2,
                            CreatedAt = new DateTime(2023, 7, 28, 18, 11, 8, 988, DateTimeKind.Utc).AddTicks(2297),
                            FullName = "Guest",
                            Role = 1
                        },
                        new
                        {
                            Id = 3,
                            CreatedAt = new DateTime(2023, 7, 28, 18, 11, 8, 988, DateTimeKind.Utc).AddTicks(2314),
                            FullName = "UserName",
                            Role = 2
                        },
                        new
                        {
                            Id = 4,
                            CreatedAt = new DateTime(2023, 7, 28, 18, 11, 8, 988, DateTimeKind.Utc).AddTicks(2335),
                            FullName = "TrialUser",
                            Role = 3
                        },
                        new
                        {
                            Id = 5,
                            CreatedAt = new DateTime(2023, 7, 28, 18, 11, 8, 988, DateTimeKind.Utc).AddTicks(2348),
                            FullName = "PaidUser",
                            Role = 4
                        });
                });

            modelBuilder.Entity("Imagine.Core.Entities.Art", b =>
                {
                    b.HasOne("Imagine.Core.Entities.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });
#pragma warning restore 612, 618
        }
    }
}
