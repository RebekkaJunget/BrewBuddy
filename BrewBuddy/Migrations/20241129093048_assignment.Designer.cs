﻿// <auto-generated />
using System;
using BrewBuddy.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BrewBuddy.Migrations
{
    [DbContext(typeof(BrewBuddyContext))]
    [Migration("20241129093048_assignment")]
    partial class assignment
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BrewBuddy.Models.Assignment", b =>
                {
                    b.Property<int>("AssignmentId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("AssignmentId"));

                    b.Property<string>("AssignmentName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime?>("FinishedDateAndTime")
                        .HasColumnType("datetime");

                    b.Property<bool>("IsComplete")
                        .HasColumnType("bit");

                    b.HasKey("AssignmentId")
                        .HasName("PK__Tasks__7C6949B1D001C0CB");

                    b.ToTable("Assignments", t =>
                        {
                            t.HasTrigger("SetDateAndTimeOnComplete");
                        });

                    b.HasAnnotation("SqlServer:UseSqlOutputClause", false);
                });

            modelBuilder.Entity("BrewBuddy.Models.CoffieMachine", b =>
                {
                    b.Property<int>("MachineId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("MachineId"));

                    b.Property<string>("Location")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("MachineId")
                        .HasName("PK__CoffieMa__44EE5B38A8A30860");

                    b.ToTable("CoffieMachine", (string)null);
                });

            modelBuilder.Entity("BrewBuddy.Models.MachineInfo", b =>
                {
                    b.Property<int>("InfoId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("InfoId"));

                    b.Property<int>("AssignmentId")
                        .HasColumnType("int");

                    b.Property<int>("MachineId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("InfoId")
                        .HasName("PK__MachineI__4DEC9D7A52EF3225");

                    b.HasIndex("AssignmentId");

                    b.HasIndex("MachineId");

                    b.HasIndex("UserId");

                    b.ToTable("MachineInfo", (string)null);
                });

            modelBuilder.Entity("BrewBuddy.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("UserId"));

                    b.Property<DateOnly>("BirthDate")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(255)
                        .HasColumnType("nvarchar(255)");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<DateTime>("RegistrationDate")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("Role")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("nvarchar(10)");

                    b.HasKey("UserId")
                        .HasName("PK__Users__1788CC4CD5065536");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("BrewBuddy.Models.MachineInfo", b =>
                {
                    b.HasOne("BrewBuddy.Models.Assignment", "Assignment")
                        .WithMany("MachineInfos")
                        .HasForeignKey("AssignmentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__MachineIn__TaskI__2F10007B");

                    b.HasOne("BrewBuddy.Models.CoffieMachine", "Machine")
                        .WithMany("MachineInfos")
                        .HasForeignKey("MachineId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__MachineIn__Machi__2D27B809");

                    b.HasOne("BrewBuddy.Models.User", "User")
                        .WithMany("MachineInfos")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("FK__MachineIn__UserI__2E1BDC42");

                    b.Navigation("Assignment");

                    b.Navigation("Machine");

                    b.Navigation("User");
                });

            modelBuilder.Entity("BrewBuddy.Models.Assignment", b =>
                {
                    b.Navigation("MachineInfos");
                });

            modelBuilder.Entity("BrewBuddy.Models.CoffieMachine", b =>
                {
                    b.Navigation("MachineInfos");
                });

            modelBuilder.Entity("BrewBuddy.Models.User", b =>
                {
                    b.Navigation("MachineInfos");
                });
#pragma warning restore 612, 618
        }
    }
}
