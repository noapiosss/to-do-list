﻿// <auto-generated />
using System;
using Domain.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Domain.Migrations
{
    [DbContext(typeof(ToDoListDbContext))]
    partial class ToDoListDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0-preview.5.23280.1")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Contracts.Database.ToDoList", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CompletionDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("completion_date_time");

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("creation_date_time");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int>("UserId")
                        .HasColumnType("integer")
                        .HasColumnName("user_id");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("to_do_lists", "public");
                });

            modelBuilder.Entity("Contracts.Database.ToDoTask", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("CompletionDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("completion_date_time");

                    b.Property<DateTime>("CreationDateTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("creation_date_time");

                    b.Property<string>("Description")
                        .HasColumnType("text")
                        .HasColumnName("description");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<int>("ToDoListId")
                        .HasColumnType("integer")
                        .HasColumnName("to_do_list_id");

                    b.HasKey("Id");

                    b.HasIndex("ToDoListId");

                    b.ToTable("to_do_tasks", "public");
                });

            modelBuilder.Entity("Contracts.Database.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Email")
                        .HasColumnType("text")
                        .HasColumnName("email");

                    b.Property<string>("Password")
                        .HasColumnType("text")
                        .HasColumnName("password");

                    b.Property<string>("Username")
                        .HasColumnType("text")
                        .HasColumnName("username");

                    b.HasKey("Id");

                    b.ToTable("users", "public");
                });

            modelBuilder.Entity("Contracts.Database.ToDoList", b =>
                {
                    b.HasOne("Contracts.Database.User", "User")
                        .WithMany("ToDoLists")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Contracts.Database.ToDoTask", b =>
                {
                    b.HasOne("Contracts.Database.ToDoList", "ToDoList")
                        .WithMany("ToDoTasks")
                        .HasForeignKey("ToDoListId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("ToDoList");
                });

            modelBuilder.Entity("Contracts.Database.ToDoList", b =>
                {
                    b.Navigation("ToDoTasks");
                });

            modelBuilder.Entity("Contracts.Database.User", b =>
                {
                    b.Navigation("ToDoLists");
                });
#pragma warning restore 612, 618
        }
    }
}
