using System;
using Microsoft.EntityFrameworkCore.Migrations;
using MySql.EntityFrameworkCore.Metadata;

#nullable disable

namespace Veygo.Platform.Shared.Infrastructure.Persistence.EntityFrameworkCore.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "blocked_dates",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    vehicle_id = table.Column<int>(type: "int", nullable: false),
                    date = table.Column<DateTime>(type: "date", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_blocked_dates", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "bookings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    vehicle_id = table.Column<int>(type: "int", nullable: false),
                    renter_id = table.Column<int>(type: "int", nullable: false),
                    owner_id = table.Column<int>(type: "int", nullable: false),
                    start_date = table.Column<DateTime>(type: "date", nullable: false),
                    end_date = table.Column<DateTime>(type: "date", nullable: false),
                    price_per_day = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    total_price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    payment_method = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    confirmed_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_bookings", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "conversations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    renter_id = table.Column<int>(type: "int", nullable: false),
                    owner_id = table.Column<int>(type: "int", nullable: false),
                    vehicle_id = table.Column<int>(type: "int", nullable: true),
                    last_message_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    last_message_preview = table.Column<string>(type: "varchar(120)", maxLength: 120, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_conversations", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "favorites",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    renter_id = table.Column<int>(type: "int", nullable: false),
                    vehicle_id = table.Column<int>(type: "int", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_favorites", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    conversation_id = table.Column<int>(type: "int", nullable: false),
                    sender_id = table.Column<int>(type: "int", nullable: false),
                    content = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: false),
                    read = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_messages", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    @params = table.Column<string>(name: "params", type: "text", nullable: false),
                    link = table.Column<string>(type: "text", nullable: true),
                    read = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_notifications", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "reviews",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    booking_id = table.Column<int>(type: "int", nullable: false),
                    vehicle_id = table.Column<int>(type: "int", nullable: false),
                    owner_id = table.Column<int>(type: "int", nullable: false),
                    renter_id = table.Column<int>(type: "int", nullable: false),
                    rating = table.Column<int>(type: "int", nullable: false),
                    comment = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_reviews", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "vehicles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySQL:ValueGenerationStrategy", MySQLValueGenerationStrategy.IdentityColumn),
                    owner_id = table.Column<int>(type: "int", nullable: false),
                    brand = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    model = table.Column<string>(type: "varchar(80)", maxLength: 80, nullable: false),
                    year = table.Column<int>(type: "int", nullable: false),
                    category = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    body_type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true),
                    transmission = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    fuel_type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false),
                    seats = table.Column<int>(type: "int", nullable: false),
                    doors = table.Column<int>(type: "int", nullable: false),
                    price_per_day = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    price_per_week = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    price_per_month = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    security_deposit = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    description = table.Column<string>(type: "varchar(2000)", maxLength: 2000, nullable: false),
                    features = table.Column<string>(type: "longtext", nullable: false),
                    photos = table.Column<string>(type: "longtext", nullable: false),
                    location_address = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false),
                    location_district = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    location_city = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: false),
                    location_latitude = table.Column<double>(type: "double", nullable: true),
                    location_longitude = table.Column<double>(type: "double", nullable: true),
                    published = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    rating = table.Column<double>(type: "double", nullable: false),
                    reviews_count = table.Column<int>(type: "int", nullable: false),
                    plate = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false),
                    color = table.Column<string>(type: "varchar(30)", maxLength: 30, nullable: false),
                    mileage = table.Column<int>(type: "int", nullable: false),
                    last_maintenance_at = table.Column<DateTime>(type: "date", nullable: true),
                    insurance_provider = table.Column<string>(type: "varchar(60)", maxLength: 60, nullable: true),
                    insurance_valid_until = table.Column<DateTime>(type: "date", nullable: true),
                    technical_inspection_until = table.Column<DateTime>(type: "date", nullable: true),
                    created_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true),
                    updated_at = table.Column<DateTimeOffset>(type: "datetime", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("p_k_vehicles", x => x.id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "i_x_blocked_dates_vehicle_id_date",
                table: "blocked_dates",
                columns: new[] { "vehicle_id", "date" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_bookings_owner_id",
                table: "bookings",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "i_x_bookings_renter_id",
                table: "bookings",
                column: "renter_id");

            migrationBuilder.CreateIndex(
                name: "i_x_bookings_vehicle_id",
                table: "bookings",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "i_x_conversations_owner_id",
                table: "conversations",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "i_x_conversations_renter_id",
                table: "conversations",
                column: "renter_id");

            migrationBuilder.CreateIndex(
                name: "i_x_favorites_renter_id_vehicle_id",
                table: "favorites",
                columns: new[] { "renter_id", "vehicle_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_messages_conversation_id",
                table: "messages",
                column: "conversation_id");

            migrationBuilder.CreateIndex(
                name: "i_x_notifications_user_id",
                table: "notifications",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "i_x_reviews_booking_id",
                table: "reviews",
                column: "booking_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "i_x_reviews_owner_id",
                table: "reviews",
                column: "owner_id");

            migrationBuilder.CreateIndex(
                name: "i_x_reviews_vehicle_id",
                table: "reviews",
                column: "vehicle_id");

            migrationBuilder.CreateIndex(
                name: "i_x_vehicles_owner_id",
                table: "vehicles",
                column: "owner_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "blocked_dates");

            migrationBuilder.DropTable(
                name: "bookings");

            migrationBuilder.DropTable(
                name: "conversations");

            migrationBuilder.DropTable(
                name: "favorites");

            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "reviews");

            migrationBuilder.DropTable(
                name: "vehicles");
        }
    }
}
