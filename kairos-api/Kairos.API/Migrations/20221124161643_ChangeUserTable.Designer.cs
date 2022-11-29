﻿// <auto-generated />
using System;
using Kairos.API.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Kairos.API.Migrations
{
    [DbContext(typeof(KairosContext))]
    [Migration("20221124161643_ChangeUserTable")]
    partial class ChangeUserTable
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("CompanionItem", b =>
                {
                    b.Property<int>("CompanionsCompanionId")
                        .HasColumnType("integer");

                    b.Property<int>("ItemsItemId")
                        .HasColumnType("integer");

                    b.HasKey("CompanionsCompanionId", "ItemsItemId");

                    b.HasIndex("ItemsItemId");

                    b.ToTable("CompanionItem");
                });

            modelBuilder.Entity("EventLabel", b =>
                {
                    b.Property<int>("EventsEventId")
                        .HasColumnType("integer");

                    b.Property<int>("LabelsLabelId")
                        .HasColumnType("integer");

                    b.HasKey("EventsEventId", "LabelsLabelId");

                    b.HasIndex("LabelsLabelId");

                    b.ToTable("EventLabel");
                });

            modelBuilder.Entity("GroupLabel", b =>
                {
                    b.Property<int>("GroupsGroupId")
                        .HasColumnType("integer");

                    b.Property<int>("LabelsLabelId")
                        .HasColumnType("integer");

                    b.HasKey("GroupsGroupId", "LabelsLabelId");

                    b.HasIndex("LabelsLabelId");

                    b.ToTable("GroupLabel");
                });

            modelBuilder.Entity("GroupUser", b =>
                {
                    b.Property<int>("GroupsGroupId")
                        .HasColumnType("integer");

                    b.Property<int>("UsersUserId")
                        .HasColumnType("integer");

                    b.HasKey("GroupsGroupId", "UsersUserId");

                    b.HasIndex("UsersUserId");

                    b.ToTable("GroupUser");
                });

            modelBuilder.Entity("Kairos.API.Models.Companion", b =>
                {
                    b.Property<int>("CompanionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("CompanionId"));

                    b.Property<int>("CompanionAge")
                        .HasColumnType("integer");

                    b.Property<string>("CompanionName")
                        .HasColumnType("text");

                    b.Property<int>("CompanionType")
                        .HasColumnType("integer");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("CompanionId");

                    b.HasIndex("UserId");

                    b.ToTable("Companions");
                });

            modelBuilder.Entity("Kairos.API.Models.Event", b =>
                {
                    b.Property<int>("EventId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("EventId"));

                    b.Property<DateTime>("EventCreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("EventDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("EventDescription")
                        .HasColumnType("text");

                    b.Property<string>("EventTitle")
                        .HasColumnType("text");

                    b.HasKey("EventId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Kairos.API.Models.Group", b =>
                {
                    b.Property<int>("GroupId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("GroupId"));

                    b.Property<int?>("EventId")
                        .HasColumnType("integer");

                    b.Property<string>("GroupName")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<bool>("GroupsIsPrivate")
                        .HasColumnType("boolean");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("GroupId");

                    b.HasIndex("EventId");

                    b.ToTable("Groups");
                });

            modelBuilder.Entity("Kairos.API.Models.Item", b =>
                {
                    b.Property<int>("ItemId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ItemId"));

                    b.Property<string>("ItemName")
                        .HasColumnType("text");

                    b.Property<int>("ItemType")
                        .HasColumnType("integer");

                    b.HasKey("ItemId");

                    b.ToTable("Items");
                });

            modelBuilder.Entity("Kairos.API.Models.Label", b =>
                {
                    b.Property<int>("LabelId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("LabelId"));

                    b.Property<string>("LabelTitle")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("LabelId");

                    b.HasIndex("UserId");

                    b.ToTable("Labels");
                });

            modelBuilder.Entity("Kairos.API.Models.OAuth2Credentials", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AccessToken")
                        .HasColumnType("text");

                    b.Property<int>("UserId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("OAuth2Credentials");
                });

            modelBuilder.Entity("Kairos.API.Models.Reminder", b =>
                {
                    b.Property<int>("ReminderId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ReminderId"));

                    b.Property<DateTime>("ReminderTime")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("StudiesId")
                        .HasColumnType("integer");

                    b.HasKey("ReminderId");

                    b.HasIndex("StudiesId");

                    b.ToTable("Reminders");
                });

            modelBuilder.Entity("Kairos.API.Models.Studies", b =>
                {
                    b.Property<int>("StudiesId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("StudiesId"));

                    b.Property<int>("GroupId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("StudiesCreatedDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("StudiesNumber")
                        .HasColumnType("text");

                    b.Property<string>("StudiesTime")
                        .HasColumnType("text");

                    b.HasKey("StudiesId");

                    b.HasIndex("GroupId");

                    b.ToTable("Studies");
                });

            modelBuilder.Entity("Kairos.API.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("UserId"));

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .HasColumnType("text");

                    b.Property<string>("LastName")
                        .HasColumnType("text");

                    b.Property<DateTime>("LastUpdatedAt")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ServiceId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("LabelStudies", b =>
                {
                    b.Property<int>("LabelsLabelId")
                        .HasColumnType("integer");

                    b.Property<int>("StudiesId")
                        .HasColumnType("integer");

                    b.HasKey("LabelsLabelId", "StudiesId");

                    b.HasIndex("StudiesId");

                    b.ToTable("LabelStudies");
                });

            modelBuilder.Entity("CompanionItem", b =>
                {
                    b.HasOne("Kairos.API.Models.Companion", null)
                        .WithMany()
                        .HasForeignKey("CompanionsCompanionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kairos.API.Models.Item", null)
                        .WithMany()
                        .HasForeignKey("ItemsItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("EventLabel", b =>
                {
                    b.HasOne("Kairos.API.Models.Event", null)
                        .WithMany()
                        .HasForeignKey("EventsEventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kairos.API.Models.Label", null)
                        .WithMany()
                        .HasForeignKey("LabelsLabelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GroupLabel", b =>
                {
                    b.HasOne("Kairos.API.Models.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupsGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kairos.API.Models.Label", null)
                        .WithMany()
                        .HasForeignKey("LabelsLabelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("GroupUser", b =>
                {
                    b.HasOne("Kairos.API.Models.Group", null)
                        .WithMany()
                        .HasForeignKey("GroupsGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kairos.API.Models.User", null)
                        .WithMany()
                        .HasForeignKey("UsersUserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Kairos.API.Models.Companion", b =>
                {
                    b.HasOne("Kairos.API.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Kairos.API.Models.Group", b =>
                {
                    b.HasOne("Kairos.API.Models.Event", "Event")
                        .WithMany()
                        .HasForeignKey("EventId");

                    b.Navigation("Event");
                });

            modelBuilder.Entity("Kairos.API.Models.Label", b =>
                {
                    b.HasOne("Kairos.API.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Kairos.API.Models.OAuth2Credentials", b =>
                {
                    b.HasOne("Kairos.API.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Kairos.API.Models.Reminder", b =>
                {
                    b.HasOne("Kairos.API.Models.Studies", "Studies")
                        .WithMany()
                        .HasForeignKey("StudiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Studies");
                });

            modelBuilder.Entity("Kairos.API.Models.Studies", b =>
                {
                    b.HasOne("Kairos.API.Models.Group", "Group")
                        .WithMany()
                        .HasForeignKey("GroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Group");
                });

            modelBuilder.Entity("LabelStudies", b =>
                {
                    b.HasOne("Kairos.API.Models.Label", null)
                        .WithMany()
                        .HasForeignKey("LabelsLabelId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Kairos.API.Models.Studies", null)
                        .WithMany()
                        .HasForeignKey("StudiesId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
