﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace HotelManagementSystem.Migrations
{
    [DbContext(typeof(HotelManagementContext))]
    [Migration("20240405075555_HMS4.3")]
    partial class HMS43
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            MySqlModelBuilderExtensions.AutoIncrementColumns(modelBuilder);

            modelBuilder.Entity("Bill", b =>
                {
                    b.Property<int>("bill_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("bill_id"));

                    b.Property<DateTime>("bill_bookTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("bill_checkInTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("bill_checkOutTime")
                        .HasColumnType("datetime(6)");

                    b.Property<bool>("bill_ifChecked")
                        .HasColumnType("tinyint(1)");

                    b.Property<bool>("bill_ifPaid")
                        .HasColumnType("tinyint(1)");

                    b.Property<DateTime?>("bill_payTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("bill_people")
                        .HasColumnType("int");

                    b.Property<int>("bill_price")
                        .HasColumnType("int");

                    b.Property<DateTime?>("bill_tureCheckInTime")
                        .HasColumnType("datetime(6)");

                    b.HasKey("bill_id");

                    b.ToTable("Bills");
                });

            modelBuilder.Entity("BillRoom", b =>
                {
                    b.Property<int>("billsbill_id")
                        .HasColumnType("int");

                    b.Property<int>("roomsroom_id")
                        .HasColumnType("int");

                    b.HasKey("billsbill_id", "roomsroom_id");

                    b.HasIndex("roomsroom_id");

                    b.ToTable("BillRoom");
                });

            modelBuilder.Entity("Class", b =>
                {
                    b.Property<int>("class_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("class_id"));

                    b.Property<int>("class_capacity")
                        .HasColumnType("int");

                    b.Property<string>("class_name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<int>("class_price")
                        .HasColumnType("int");

                    b.HasKey("class_id");

                    b.ToTable("Classes");
                });

            modelBuilder.Entity("Client", b =>
                {
                    b.Property<int>("client_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("client_id"));

                    b.Property<int>("Billbill_id")
                        .HasColumnType("int");

                    b.Property<string>("client_name")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.Property<string>("client_tel")
                        .HasColumnType("longtext");

                    b.Property<string>("client_trueId")
                        .IsRequired()
                        .HasColumnType("longtext");

                    b.HasKey("client_id");

                    b.HasIndex("Billbill_id");

                    b.ToTable("Clients");
                });

            modelBuilder.Entity("Room", b =>
                {
                    b.Property<int>("room_id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    MySqlPropertyBuilderExtensions.UseMySqlIdentityColumn(b.Property<int>("room_id"));

                    b.Property<int>("Classclass_id")
                        .HasColumnType("int");

                    b.Property<int>("room_floor")
                        .HasColumnType("int");

                    b.Property<bool>("room_ifStayIn")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("room_number")
                        .HasColumnType("int");

                    b.HasKey("room_id");

                    b.HasIndex("Classclass_id");

                    b.ToTable("Rooms");
                });

            modelBuilder.Entity("BillRoom", b =>
                {
                    b.HasOne("Bill", null)
                        .WithMany()
                        .HasForeignKey("billsbill_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Room", null)
                        .WithMany()
                        .HasForeignKey("roomsroom_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Client", b =>
                {
                    b.HasOne("Bill", null)
                        .WithMany("clients")
                        .HasForeignKey("Billbill_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Room", b =>
                {
                    b.HasOne("Class", "roomclass")
                        .WithMany("rooms")
                        .HasForeignKey("Classclass_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("roomclass");
                });

            modelBuilder.Entity("Bill", b =>
                {
                    b.Navigation("clients");
                });

            modelBuilder.Entity("Class", b =>
                {
                    b.Navigation("rooms");
                });
#pragma warning restore 612, 618
        }
    }
}
