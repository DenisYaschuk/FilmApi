﻿// <auto-generated />
using FilmApi.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace FilmApi.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20210530124638_BookModel")]
    partial class BookModel
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 64)
                .HasAnnotation("ProductVersion", "5.0.6");

            modelBuilder.Entity("FilmApi.Models.Book.BookResponse", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("authors")
                        .HasColumnType("longtext");

                    b.Property<string>("isbn_10")
                        .HasColumnType("longtext");

                    b.Property<string>("publish_date")
                        .HasColumnType("longtext");

                    b.Property<string>("title")
                        .HasColumnType("longtext");

                    b.Property<string>("url")
                        .HasColumnType("longtext");

                    b.HasKey("id");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("FilmApi.Models.Films.FilmResponse", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("genre_ids")
                        .HasColumnType("longtext");

                    b.Property<string>("original_title")
                        .HasColumnType("longtext");

                    b.Property<string>("overview")
                        .HasColumnType("longtext");

                    b.Property<string>("poster_path")
                        .HasColumnType("longtext");

                    b.Property<string>("release_date")
                        .HasColumnType("longtext");

                    b.Property<float>("vote_average")
                        .HasColumnType("float");

                    b.HasKey("id");

                    b.ToTable("Films");
                });

            modelBuilder.Entity("FilmApi.Models.Games.GameResponse", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("background_image")
                        .HasColumnType("longtext");

                    b.Property<string>("genres")
                        .HasColumnType("longtext");

                    b.Property<string>("name")
                        .HasColumnType("longtext");

                    b.Property<float>("rating")
                        .HasColumnType("float");

                    b.Property<string>("released")
                        .HasColumnType("longtext");

                    b.HasKey("id");

                    b.ToTable("Games");
                });
#pragma warning restore 612, 618
        }
    }
}
