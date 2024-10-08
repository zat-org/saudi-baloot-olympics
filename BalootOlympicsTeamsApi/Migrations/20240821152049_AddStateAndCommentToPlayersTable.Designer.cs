﻿// <auto-generated />
using System;
using BalootOlympicsTeamsApi.DbContexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BalootOlympicsTeamsApi.Migrations
{
    [DbContext(typeof(OlympicsContext))]
    [Migration("20240821152049_AddStateAndCommentToPlayersTable")]
    partial class AddStateAndCommentToPlayersTable
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.HasPostgresExtension(modelBuilder, "uuid-ossp");
            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("BalootOlympicsTeamsApi.Entities.ConfirmationRequest", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id")
                        .HasDefaultValueSql("uuid_generate_v4()");

                    b.Property<DateTimeOffset?>("ConfirmedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("confirmed_at");

                    b.Property<string>("FirstPlayerId")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("first_player_id");

                    b.Property<string>("FirstPlayerOtp")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("character varying(6)")
                        .HasColumnName("first_player_otp");

                    b.Property<string>("SecondPlayerId")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("second_player_id");

                    b.Property<string>("SecondPlayerOtp")
                        .IsRequired()
                        .HasMaxLength(6)
                        .HasColumnType("character varying(6)")
                        .HasColumnName("second_player_otp");

                    b.Property<DateTimeOffset>("SentAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("sent_at");

                    b.HasKey("Id")
                        .HasName("confirmation_request_pkey");

                    b.HasIndex("FirstPlayerId");

                    b.HasIndex("SecondPlayerId");

                    b.ToTable("confirmation_request", (string)null);
                });

            modelBuilder.Entity("BalootOlympicsTeamsApi.Entities.Player", b =>
                {
                    b.Property<string>("Id")
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("id");

                    b.Property<string>("Comment")
                        .IsRequired()
                        .HasMaxLength(512)
                        .HasColumnType("character varying(512)")
                        .HasColumnName("comment");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("email");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("phone");

                    b.Property<string>("State")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)")
                        .HasColumnName("state");

                    b.Property<int?>("TeamId")
                        .HasColumnType("INT")
                        .HasColumnName("team_id");

                    b.HasKey("Id")
                        .HasName("players_pkey");

                    b.HasIndex("TeamId");

                    b.HasIndex(new[] { "Email" }, "player_email_key")
                        .IsUnique();

                    b.HasIndex(new[] { "Phone" }, "player_phone_key")
                        .IsUnique();

                    b.ToTable("players", (string)null);
                });

            modelBuilder.Entity("BalootOlympicsTeamsApi.Entities.Team", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.HasKey("Id")
                        .HasName("teams_pkey");

                    b.ToTable("teams", (string)null);
                });

            modelBuilder.Entity("BalootOlympicsTeamsApi.Entities.ConfirmationRequest", b =>
                {
                    b.HasOne("BalootOlympicsTeamsApi.Entities.Player", "FirstPlayer")
                        .WithMany()
                        .HasForeignKey("FirstPlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_first_player_id");

                    b.HasOne("BalootOlympicsTeamsApi.Entities.Player", "SecondPlayer")
                        .WithMany()
                        .HasForeignKey("SecondPlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_second_player_id");

                    b.Navigation("FirstPlayer");

                    b.Navigation("SecondPlayer");
                });

            modelBuilder.Entity("BalootOlympicsTeamsApi.Entities.Player", b =>
                {
                    b.HasOne("BalootOlympicsTeamsApi.Entities.Team", "Team")
                        .WithMany("Players")
                        .HasForeignKey("TeamId")
                        .OnDelete(DeleteBehavior.SetNull);

                    b.Navigation("Team");
                });

            modelBuilder.Entity("BalootOlympicsTeamsApi.Entities.Team", b =>
                {
                    b.Navigation("Players");
                });
#pragma warning restore 612, 618
        }
    }
}
