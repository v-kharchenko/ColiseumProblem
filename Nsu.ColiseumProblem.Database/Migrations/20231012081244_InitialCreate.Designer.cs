﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Nsu.ColiseumProblem.Database;

#nullable disable

namespace Nsu.ColiseumProblem.Database.Migrations
{
    [DbContext(typeof(ColiseumContext))]
    [Migration("20231012081244_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.12");

            modelBuilder.Entity("Nsu.ColiseumProblem.Database.Card", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ConditionId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("DeckPosition")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("ConditionId");

                    b.ToTable("Cards");
                });

            modelBuilder.Entity("Nsu.ColiseumProblem.Database.Condition", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.ToTable("Conditions");
                });

            modelBuilder.Entity("Nsu.ColiseumProblem.Database.Card", b =>
                {
                    b.HasOne("Nsu.ColiseumProblem.Database.Condition", "Condition")
                        .WithMany("Cards")
                        .HasForeignKey("ConditionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Condition");
                });

            modelBuilder.Entity("Nsu.ColiseumProblem.Database.Condition", b =>
                {
                    b.Navigation("Cards");
                });
#pragma warning restore 612, 618
        }
    }
}
