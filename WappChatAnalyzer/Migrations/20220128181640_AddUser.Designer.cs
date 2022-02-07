﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WappChatAnalyzer.Domain;

namespace WappChatAnalyzer.Migrations
{
    [DbContext(typeof(MainDbContext))]
    [Migration("20220128181640_AddUser")]
    partial class AddUser
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.5");

            modelBuilder.Entity("WappChatAnalyzer.Domain.CustomStatistic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasMaxLength(60)
                        .HasColumnType("varchar(60)");

                    b.Property<string>("Regex")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("CustomStatistics");
                });

            modelBuilder.Entity("WappChatAnalyzer.Domain.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("DateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Emoji")
                        .HasColumnType("longtext");

                    b.Property<int>("EventGroupId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EventGroupId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("WappChatAnalyzer.Domain.EventGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("EventGroups");
                });

            modelBuilder.Entity("WappChatAnalyzer.Domain.ImportHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("FirstMessageDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("FromMessageId")
                        .HasColumnType("int");

                    b.Property<DateTime>("ImportDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<DateTime>("LastMessageDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<int>("MessageCount")
                        .HasColumnType("int");

                    b.Property<int>("Overlap")
                        .HasColumnType("int");

                    b.Property<int>("ToMessageId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("ImportHistories");
                });

            modelBuilder.Entity("WappChatAnalyzer.Domain.Message", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<bool>("IsMedia")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.Property<DateTime>("SentDateTime")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("Text")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.HasIndex("SenderId");

                    b.HasIndex("SentDateTime");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("WappChatAnalyzer.Domain.Sender", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Senders");
                });

            modelBuilder.Entity("WappChatAnalyzer.Domain.StatisticCache", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("ForDate")
                        .HasColumnType("datetime(6)");

                    b.Property<string>("StatisticName")
                        .HasColumnType("varchar(255)");

                    b.HasKey("Id");

                    b.HasIndex(new[] { "StatisticName", "ForDate" }, "IX_Name_Date")
                        .IsUnique();

                    b.ToTable("StatisticCaches");
                });

            modelBuilder.Entity("WappChatAnalyzer.Domain.StatisticCacheForSender", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.Property<int>("StatisticCacheId")
                        .HasColumnType("int");

                    b.Property<int>("Total")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SenderId");

                    b.HasIndex("StatisticCacheId");

                    b.ToTable("StatisticCacheForSender");
                });

            modelBuilder.Entity("WappChatAnalyzer.Domain.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Email")
                        .HasColumnType("longtext");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("longtext");

                    b.Property<byte[]>("Salt")
                        .HasColumnType("longblob");

                    b.Property<string>("Username")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WappChatAnalyzer.Domain.Event", b =>
                {
                    b.HasOne("WappChatAnalyzer.Domain.EventGroup", "EventGroup")
                        .WithMany()
                        .HasForeignKey("EventGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EventGroup");
                });

            modelBuilder.Entity("WappChatAnalyzer.Domain.Message", b =>
                {
                    b.HasOne("WappChatAnalyzer.Domain.Sender", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("WappChatAnalyzer.Domain.StatisticCacheForSender", b =>
                {
                    b.HasOne("WappChatAnalyzer.Domain.Sender", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WappChatAnalyzer.Domain.StatisticCache", null)
                        .WithMany("ForSenders")
                        .HasForeignKey("StatisticCacheId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sender");
                });

            modelBuilder.Entity("WappChatAnalyzer.Domain.StatisticCache", b =>
                {
                    b.Navigation("ForSenders");
                });
#pragma warning restore 612, 618
        }
    }
}
