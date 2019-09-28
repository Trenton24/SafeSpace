﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SafeSpace.Models;

namespace SafeSpace.Migrations
{
    [DbContext(typeof(MyContext))]
    [Migration("20190627172856_zz")]
    partial class zz
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.1.4-rtm-31024")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("SafeSpace.Models.Friends", b =>
                {
                    b.Property<int>("FriendId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("user1Id");

                    b.Property<int>("user2Id");

                    b.HasKey("FriendId");

                    b.ToTable("Friends");
                });

            modelBuilder.Entity("SafeSpace.Models.UserHaveFriends", b =>
                {
                    b.Property<int>("RelationshipId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("Accepted");

                    b.Property<int>("RequestedId");

                    b.Property<int>("SentById");

                    b.HasKey("RelationshipId");

                    b.HasIndex("RequestedId");

                    b.HasIndex("SentById");

                    b.ToTable("UserhasFriends");
                });

            modelBuilder.Entity("SafeSpace.Models.Users", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<DateTime>("CreatedAt");

                    b.Property<string>("Email")
                        .IsRequired();

                    b.Property<string>("FirstName")
                        .IsRequired();

                    b.Property<string>("LastName")
                        .IsRequired();

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<DateTime>("UpdatedAt");

                    b.HasKey("UserId");

                    b.ToTable("users");
                });

            modelBuilder.Entity("SafeSpace.Models.UserHaveFriends", b =>
                {
                    b.HasOne("SafeSpace.Models.Users", "Requested")
                        .WithMany("Pending")
                        .HasForeignKey("RequestedId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("SafeSpace.Models.Users", "SentBy")
                        .WithMany("Requested")
                        .HasForeignKey("SentById")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}