﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Stock.Infrastructure.Persistence;

#nullable disable

namespace Stock.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(StockDbContext))]
    [Migration("20240502112917_Add_outbox_pattern_tables")]
    partial class Add_outbox_pattern_tables
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("MassTransit.EntityFrameworkCoreIntegration.InboxState", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTime?>("Consumed")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("consumed");

                    b.Property<Guid>("ConsumerId")
                        .HasColumnType("uuid")
                        .HasColumnName("consumerid");

                    b.Property<DateTime?>("Delivered")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("delivered");

                    b.Property<DateTime?>("ExpirationTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expirationtime");

                    b.Property<long?>("LastSequenceNumber")
                        .HasColumnType("bigint")
                        .HasColumnName("lastsequencenumber");

                    b.Property<Guid>("LockId")
                        .HasColumnType("uuid")
                        .HasColumnName("lockid");

                    b.Property<Guid>("MessageId")
                        .HasColumnType("uuid")
                        .HasColumnName("messageid");

                    b.Property<int>("ReceiveCount")
                        .HasColumnType("integer")
                        .HasColumnName("receivecount");

                    b.Property<DateTime>("Received")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("received");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("bytea")
                        .HasColumnName("rowversion");

                    b.HasKey("Id")
                        .HasName("pk_inboxstate");

                    b.HasAlternateKey("MessageId", "ConsumerId")
                        .HasName("ak_inboxstate_messageid_consumerid");

                    b.HasIndex("Delivered")
                        .HasDatabaseName("ix_inboxstate_delivered");

                    b.ToTable("inboxstate", (string)null);
                });

            modelBuilder.Entity("MassTransit.EntityFrameworkCoreIntegration.OutboxMessage", b =>
                {
                    b.Property<long>("SequenceNumber")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("sequencenumber");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("SequenceNumber"));

                    b.Property<string>("Body")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("body");

                    b.Property<string>("ContentType")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("contenttype");

                    b.Property<Guid?>("ConversationId")
                        .HasColumnType("uuid")
                        .HasColumnName("conversationid");

                    b.Property<Guid?>("CorrelationId")
                        .HasColumnType("uuid")
                        .HasColumnName("correlationid");

                    b.Property<string>("DestinationAddress")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("destinationaddress");

                    b.Property<DateTime?>("EnqueueTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("enqueuetime");

                    b.Property<DateTime?>("ExpirationTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("expirationtime");

                    b.Property<string>("FaultAddress")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("faultaddress");

                    b.Property<string>("Headers")
                        .HasColumnType("text")
                        .HasColumnName("headers");

                    b.Property<Guid?>("InboxConsumerId")
                        .HasColumnType("uuid")
                        .HasColumnName("inboxconsumerid");

                    b.Property<Guid?>("InboxMessageId")
                        .HasColumnType("uuid")
                        .HasColumnName("inboxmessageid");

                    b.Property<Guid?>("InitiatorId")
                        .HasColumnType("uuid")
                        .HasColumnName("initiatorid");

                    b.Property<Guid>("MessageId")
                        .HasColumnType("uuid")
                        .HasColumnName("messageid");

                    b.Property<string>("MessageType")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("messagetype");

                    b.Property<Guid?>("OutboxId")
                        .HasColumnType("uuid")
                        .HasColumnName("outboxid");

                    b.Property<string>("Properties")
                        .HasColumnType("text")
                        .HasColumnName("properties");

                    b.Property<Guid?>("RequestId")
                        .HasColumnType("uuid")
                        .HasColumnName("requestid");

                    b.Property<string>("ResponseAddress")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("responseaddress");

                    b.Property<DateTime>("SentTime")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("senttime");

                    b.Property<string>("SourceAddress")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("sourceaddress");

                    b.HasKey("SequenceNumber")
                        .HasName("pk_outboxmessage");

                    b.HasIndex("EnqueueTime")
                        .HasDatabaseName("ix_outboxmessage_enqueuetime");

                    b.HasIndex("ExpirationTime")
                        .HasDatabaseName("ix_outboxmessage_expirationtime");

                    b.HasIndex("OutboxId", "SequenceNumber")
                        .IsUnique()
                        .HasDatabaseName("ix_outboxmessage_outboxid_sequencenumber");

                    b.HasIndex("InboxMessageId", "InboxConsumerId", "SequenceNumber")
                        .IsUnique()
                        .HasDatabaseName("ix_outboxmessage_inboxmessageid_inboxconsumerid_sequencenumber");

                    b.ToTable("outboxmessage", (string)null);
                });

            modelBuilder.Entity("MassTransit.EntityFrameworkCoreIntegration.OutboxState", b =>
                {
                    b.Property<Guid>("OutboxId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("outboxid");

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created");

                    b.Property<DateTime?>("Delivered")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("delivered");

                    b.Property<long?>("LastSequenceNumber")
                        .HasColumnType("bigint")
                        .HasColumnName("lastsequencenumber");

                    b.Property<Guid>("LockId")
                        .HasColumnType("uuid")
                        .HasColumnName("lockid");

                    b.Property<byte[]>("RowVersion")
                        .IsConcurrencyToken()
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("bytea")
                        .HasColumnName("rowversion");

                    b.HasKey("OutboxId")
                        .HasName("pk_outboxstate");

                    b.HasIndex("Created")
                        .HasDatabaseName("ix_outboxstate_created");

                    b.ToTable("outboxstate", (string)null);
                });

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

            modelBuilder.Entity("Stock.Core.Models.Stock.StockWithPriceTag", b =>
                {
                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("createdat");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("isactive");

                    b.Property<DateTime?>("LastPriceUpdate")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("lastpriceupdate");

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