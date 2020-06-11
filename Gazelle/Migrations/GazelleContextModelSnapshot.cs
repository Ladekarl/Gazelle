﻿// <auto-generated />
using System;
using Gazelle.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Gazelle.Migrations
{
    [DbContext(typeof(GazelleContext))]
    partial class GazelleContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Gazelle.Models.City", b =>
                {
                    b.Property<int>("CityId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CityName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("CountryId")
                        .HasColumnType("int");

                    b.HasKey("CityId");

                    b.HasIndex("CountryId");

                    b.ToTable("Cities");
                });

            modelBuilder.Entity("Gazelle.Models.Connection", b =>
                {
                    b.Property<int>("ConnectionId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Company")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("EndCityCityId")
                        .HasColumnType("int");

                    b.Property<int>("Price")
                        .HasColumnType("int");

                    b.Property<int?>("RouteId")
                        .HasColumnType("int");

                    b.Property<int?>("StartCityCityId")
                        .HasColumnType("int");

                    b.Property<int>("Time")
                        .HasColumnType("int");

                    b.HasKey("ConnectionId");

                    b.HasIndex("EndCityCityId");

                    b.HasIndex("RouteId");

                    b.HasIndex("StartCityCityId");

                    b.ToTable("Connections");
                });

            modelBuilder.Entity("Gazelle.Models.Country", b =>
                {
                    b.Property<int>("CountryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<bool>("Conflict")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("CountryId");

                    b.ToTable("Countries");
                });

            modelBuilder.Entity("Gazelle.Models.Delivery", b =>
                {
                    b.Property<int>("DeliveryId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ApprovedRouteRouteId")
                        .HasColumnType("int");

                    b.Property<int?>("DeliveryTypeId")
                        .HasColumnType("int");

                    b.Property<double>("DriverId")
                        .HasColumnType("float");

                    b.Property<int?>("EndCityCityId")
                        .HasColumnType("int");

                    b.Property<double>("Length")
                        .HasColumnType("float");

                    b.Property<int?>("StartCityCityId")
                        .HasColumnType("int");

                    b.Property<double>("Weight")
                        .HasColumnType("float");

                    b.HasKey("DeliveryId");

                    b.HasIndex("ApprovedRouteRouteId");

                    b.HasIndex("DeliveryTypeId");

                    b.HasIndex("EndCityCityId");

                    b.HasIndex("StartCityCityId");

                    b.ToTable("Deliveries");
                });

            modelBuilder.Entity("Gazelle.Models.DeliveryType", b =>
                {
                    b.Property<int>("DeliveryTypeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.HasKey("DeliveryTypeId");

                    b.ToTable("DeliveryTypes");
                });

            modelBuilder.Entity("Gazelle.Models.Route", b =>
                {
                    b.Property<int>("RouteId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Companies")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("DeliveryId")
                        .HasColumnType("int");

                    b.Property<double>("Price")
                        .HasColumnType("float");

                    b.Property<double>("Time")
                        .HasColumnType("float");

                    b.HasKey("RouteId");

                    b.HasIndex("DeliveryId");

                    b.ToTable("Routes");
                });

            modelBuilder.Entity("Gazelle.Models.City", b =>
                {
                    b.HasOne("Gazelle.Models.Country", "Country")
                        .WithMany("Cities")
                        .HasForeignKey("CountryId");
                });

            modelBuilder.Entity("Gazelle.Models.Connection", b =>
                {
                    b.HasOne("Gazelle.Models.City", "EndCity")
                        .WithMany()
                        .HasForeignKey("EndCityCityId");

                    b.HasOne("Gazelle.Models.Route", null)
                        .WithMany("Connections")
                        .HasForeignKey("RouteId");

                    b.HasOne("Gazelle.Models.City", "StartCity")
                        .WithMany()
                        .HasForeignKey("StartCityCityId");
                });

            modelBuilder.Entity("Gazelle.Models.Delivery", b =>
                {
                    b.HasOne("Gazelle.Models.Route", "ApprovedRoute")
                        .WithMany()
                        .HasForeignKey("ApprovedRouteRouteId");

                    b.HasOne("Gazelle.Models.DeliveryType", "DeliveryType")
                        .WithMany("Deliveries")
                        .HasForeignKey("DeliveryTypeId");

                    b.HasOne("Gazelle.Models.City", "EndCity")
                        .WithMany()
                        .HasForeignKey("EndCityCityId");

                    b.HasOne("Gazelle.Models.City", "StartCity")
                        .WithMany()
                        .HasForeignKey("StartCityCityId");
                });

            modelBuilder.Entity("Gazelle.Models.Route", b =>
                {
                    b.HasOne("Gazelle.Models.Delivery", null)
                        .WithMany("Routes")
                        .HasForeignKey("DeliveryId");
                });
#pragma warning restore 612, 618
        }
    }
}
