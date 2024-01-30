﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Stock.Infrastructure.Persistence;

#nullable disable

namespace Stock.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(StockDbContext))]
    partial class StockDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Stock.Core.Models.Stock.StockEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("createdat");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("createdby");

                    b.Property<DateTime?>("Deactivated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deactivated");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("isactive");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("modifiedat");

                    b.Property<string>("ModifiedBy")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("modifiedby");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasMaxLength(10)
                        .HasColumnType("character varying(10)")
                        .HasColumnName("symbol");

                    b.HasKey("Id")
                        .HasName("pk_stocks");

                    b.HasIndex("Symbol")
                        .IsUnique()
                        .HasDatabaseName("ix_stocks_symbol");

                    b.ToTable("stocks", (string)null);
                });

            modelBuilder.Entity("Stock.Core.Models.Stock.StockPriceEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("createdat");

                    b.Property<string>("CreatedBy")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("createdby");

                    b.Property<DateTime?>("Deactivated")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("deactivated");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("isactive");

                    b.Property<DateTime>("ModifiedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("modifiedat");

                    b.Property<string>("ModifiedBy")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("modifiedby");

                    b.Property<decimal>("Price")
                        .HasPrecision(18, 2)
                        .HasColumnType("numeric(18,2)")
                        .HasColumnName("price");

                    b.Property<int>("StockId")
                        .HasColumnType("integer")
                        .HasColumnName("stockid");

                    b.HasKey("Id")
                        .HasName("pk_stocks_prices");

                    b.HasIndex("StockId")
                        .HasDatabaseName("ix_stocks_prices_stockid");

                    b.ToTable("stocks_prices", (string)null);
                });

            modelBuilder.Entity("Stock.Core.Models.Stock.StockWithPriceTagReadModel", b =>
                {
                    b.Property<decimal>("Price")
                        .HasColumnType("numeric")
                        .HasColumnName("price");

                    b.Property<int>("StockId")
                        .HasColumnType("integer")
                        .HasColumnName("stockid");

                    b.Property<string>("Symbol")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("symbol");

                    b.ToTable((string)null);

                    b.ToView("stock_with_price_tag", (string)null);
                });

            modelBuilder.Entity("Stock.Core.Models.Stock.StockPriceEntity", b =>
                {
                    b.HasOne("Stock.Core.Models.Stock.StockEntity", "StockEntity")
                        .WithMany("Prices")
                        .HasForeignKey("StockId")
                        .OnDelete(DeleteBehavior.Restrict)
                        .IsRequired()
                        .HasConstraintName("fk_stocks_prices_stocks_stockid");

                    b.Navigation("StockEntity");
                });

            modelBuilder.Entity("Stock.Core.Models.Stock.StockEntity", b =>
                {
                    b.Navigation("Prices");
                });
#pragma warning restore 612, 618
        }
    }
}
