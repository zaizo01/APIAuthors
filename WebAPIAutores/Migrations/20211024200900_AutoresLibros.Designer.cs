﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebAPIAutores;

namespace WebAPIAutores.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20211024200900_AutoresLibros")]
    partial class AutoresLibros
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.11")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("WebAPIAutores.Entities.Autor", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(120)
                        .HasColumnType("nvarchar(120)");

                    b.HasKey("Id");

                    b.ToTable("Autores");
                });

            modelBuilder.Entity("WebAPIAutores.Entities.AutorLibro", b =>
                {
                    b.Property<int>("AutorId")
                        .HasColumnType("int");

                    b.Property<int>("LibrosId")
                        .HasColumnType("int");

                    b.Property<int?>("LibroId")
                        .HasColumnType("int");

                    b.Property<int>("Orden")
                        .HasColumnType("int");

                    b.HasKey("AutorId", "LibrosId");

                    b.HasIndex("LibroId");

                    b.ToTable("AutoresLibros");
                });

            modelBuilder.Entity("WebAPIAutores.Entities.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LibroId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("LibroId");

                    b.ToTable("Comentarios");
                });

            modelBuilder.Entity("WebAPIAutores.Entities.Libro", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Tittle")
                        .IsRequired()
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.HasKey("Id");

                    b.ToTable("Libros");
                });

            modelBuilder.Entity("WebAPIAutores.Entities.AutorLibro", b =>
                {
                    b.HasOne("WebAPIAutores.Entities.Autor", "Autor")
                        .WithMany("AutoresLibros")
                        .HasForeignKey("AutorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebAPIAutores.Entities.Libro", "Libro")
                        .WithMany("AutoresLibros")
                        .HasForeignKey("LibroId");

                    b.Navigation("Autor");

                    b.Navigation("Libro");
                });

            modelBuilder.Entity("WebAPIAutores.Entities.Comment", b =>
                {
                    b.HasOne("WebAPIAutores.Entities.Libro", "Libro")
                        .WithMany("Comentarios")
                        .HasForeignKey("LibroId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Libro");
                });

            modelBuilder.Entity("WebAPIAutores.Entities.Autor", b =>
                {
                    b.Navigation("AutoresLibros");
                });

            modelBuilder.Entity("WebAPIAutores.Entities.Libro", b =>
                {
                    b.Navigation("AutoresLibros");

                    b.Navigation("Comentarios");
                });
#pragma warning restore 612, 618
        }
    }
}