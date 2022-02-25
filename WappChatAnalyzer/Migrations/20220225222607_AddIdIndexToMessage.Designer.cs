﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WappChatAnalyzer.Domain;

#nullable disable

namespace WappChatAnalyzer.Migrations
{
    [DbContext(typeof(MainDbContext))]
    [Migration("20220225222607_AddIdIndexToMessage")]
    partial class AddIdIndexToMessage
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

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

                    b.Property<int>("WorkspaceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WorkspaceId");

                    b.ToTable("CustomStatistics");
                });

            modelBuilder.Entity("WappChatAnalyzer.Domain.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateOnly>("Date")
                        .HasColumnType("date");

                    b.Property<string>("Emoji")
                        .HasColumnType("longtext");

                    b.Property<int>("EventGroupId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<int>("WorkspaceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EventGroupId");

                    b.HasIndex("WorkspaceId");

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

                    b.Property<int>("WorkspaceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WorkspaceId");

                    b.ToTable("ImportHistories");
                });

            modelBuilder.Entity("WappChatAnalyzer.Domain.Message", b =>
                {
                    b.Property<int>("Id")
                        .HasColumnType("int");

                    b.Property<int>("WorkspaceId")
                        .HasColumnType("int");

                    b.Property<bool>("IsMedia")
                        .HasColumnType("tinyint(1)");

                    b.Property<int>("SenderId")
                        .HasColumnType("int");

                    b.Property<DateOnly>("SentDate")
                        .HasColumnType("date");

                    b.Property<TimeOnly>("SentTime")
                        .HasColumnType("time(6)");

                    b.Property<string>("Text")
                        .HasColumnType("longtext");

                    b.HasKey("Id", "WorkspaceId");

                    b.HasIndex("Id");

                    b.HasIndex("SenderId");

                    b.HasIndex("WorkspaceId");

                    b.HasIndex("SentDate", "SentTime");

                    b.ToTable("Messages");
                });

            modelBuilder.Entity("WappChatAnalyzer.Domain.Sender", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<int>("WorkspaceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WorkspaceId");

                    b.ToTable("Senders");
                });

            modelBuilder.Entity("WappChatAnalyzer.Domain.StatisticCache", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateOnly>("ForDate")
                        .HasColumnType("date");

                    b.Property<string>("StatisticName")
                        .HasColumnType("varchar(255)");

                    b.Property<int>("WorkspaceId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("WorkspaceId");

                    b.HasIndex(new[] { "StatisticName", "ForDate", "WorkspaceId" }, "IX_Name_Date_Workspace")
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

                    b.Property<int?>("SelectedWorkspaceId")
                        .HasColumnType("int");

                    b.Property<string>("Username")
                        .HasColumnType("longtext");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("WappChatAnalyzer.Domain.Workspace", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OwnerId");

                    b.ToTable("Workspaces");
                });

            modelBuilder.Entity("WappChatAnalyzer.Domain.CustomStatistic", b =>
                {
                    b.HasOne("WappChatAnalyzer.Domain.Workspace", "Workspace")
                        .WithMany()
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Workspace");
                });

            modelBuilder.Entity("WappChatAnalyzer.Domain.Event", b =>
                {
                    b.HasOne("WappChatAnalyzer.Domain.EventGroup", "EventGroup")
                        .WithMany()
                        .HasForeignKey("EventGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WappChatAnalyzer.Domain.Workspace", "Workspace")
                        .WithMany()
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EventGroup");

                    b.Navigation("Workspace");
                });

            modelBuilder.Entity("WappChatAnalyzer.Domain.ImportHistory", b =>
                {
                    b.HasOne("WappChatAnalyzer.Domain.Workspace", "Workspace")
                        .WithMany()
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Workspace");
                });

            modelBuilder.Entity("WappChatAnalyzer.Domain.Message", b =>
                {
                    b.HasOne("WappChatAnalyzer.Domain.Sender", "Sender")
                        .WithMany()
                        .HasForeignKey("SenderId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WappChatAnalyzer.Domain.Workspace", "Workspace")
                        .WithMany()
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Sender");

                    b.Navigation("Workspace");
                });

            modelBuilder.Entity("WappChatAnalyzer.Domain.Sender", b =>
                {
                    b.HasOne("WappChatAnalyzer.Domain.Workspace", "Workspace")
                        .WithMany()
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Workspace");
                });

            modelBuilder.Entity("WappChatAnalyzer.Domain.StatisticCache", b =>
                {
                    b.HasOne("WappChatAnalyzer.Domain.Workspace", "Workspace")
                        .WithMany()
                        .HasForeignKey("WorkspaceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Workspace");
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

            modelBuilder.Entity("WappChatAnalyzer.Domain.Workspace", b =>
                {
                    b.HasOne("WappChatAnalyzer.Domain.User", "Owner")
                        .WithMany("Workspaces")
                        .HasForeignKey("OwnerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Owner");
                });

            modelBuilder.Entity("WappChatAnalyzer.Domain.StatisticCache", b =>
                {
                    b.Navigation("ForSenders");
                });

            modelBuilder.Entity("WappChatAnalyzer.Domain.User", b =>
                {
                    b.Navigation("Workspaces");
                });
#pragma warning restore 612, 618
        }
    }
}
