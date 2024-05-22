﻿// <auto-generated />
using System;
using KangarooParty.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace KangarooParty.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("KangarooParty.Models.Kangaroo", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int?>("AttendingPartyId")
                        .HasColumnType("int");

                    b.Property<int?>("HostingPartyId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Pic")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("AttendingPartyId");

                    b.HasIndex("HostingPartyId")
                        .IsUnique()
                        .HasFilter("[HostingPartyId] IS NOT NULL");

                    b.ToTable("Kangaroos");
                });

            modelBuilder.Entity("KangarooParty.Models.Party", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Prestige")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Parties");
                });

            modelBuilder.Entity("KangarooParty.Models.Kangaroo", b =>
                {
                    b.HasOne("KangarooParty.Models.Party", "AttendingParty")
                        .WithMany("Attendees")
                        .HasForeignKey("AttendingPartyId");

                    b.HasOne("KangarooParty.Models.Party", "HostingParty")
                        .WithOne("Host")
                        .HasForeignKey("KangarooParty.Models.Kangaroo", "HostingPartyId");

                    b.Navigation("AttendingParty");

                    b.Navigation("HostingParty");
                });

            modelBuilder.Entity("KangarooParty.Models.Party", b =>
                {
                    b.Navigation("Attendees");

                    b.Navigation("Host")
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
