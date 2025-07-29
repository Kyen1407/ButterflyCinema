using System;
using System.Collections;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ButterflyCinema.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cinemas",
                columns: table => new
                {
                    cinema_id = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    cinema_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    cinema_address = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    address_url = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("cinemas_pkey", x => x.cinema_id);
                });

            migrationBuilder.CreateTable(
                name: "combo",
                columns: table => new
                {
                    combo_id = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    combo_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    combo_price = table.Column<int>(type: "integer", nullable: true),
                    combo_status = table.Column<BitArray>(type: "bit(1)", nullable: true),
                    combo_img = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("combo_pkey", x => x.combo_id);
                });

            migrationBuilder.CreateTable(
                name: "concessions",
                columns: table => new
                {
                    concession_id = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    concession_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    concession_price = table.Column<int>(type: "integer", nullable: true),
                    concession_status = table.Column<BitArray>(type: "bit(1)", nullable: true),
                    concession_img = table.Column<string>(type: "text", nullable: true),
                    concession_item = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("concessions_pkey", x => x.concession_id);
                });

            migrationBuilder.CreateTable(
                name: "movies",
                columns: table => new
                {
                    movie_id = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    movie_name = table.Column<string>(type: "text", nullable: true),
                    genre = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    director = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    actor = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: true),
                    description = table.Column<string>(type: "text", nullable: true),
                    age_restriction = table.Column<int>(type: "integer", nullable: true),
                    release_date = table.Column<DateOnly>(type: "date", nullable: true),
                    language_option = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    duration = table.Column<int>(type: "integer", nullable: true),
                    country = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    rating = table.Column<double>(type: "double precision", nullable: true),
                    poster = table.Column<string>(type: "text", nullable: true),
                    trailer = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("movies_pkey", x => x.movie_id);
                });

            migrationBuilder.CreateTable(
                name: "news",
                columns: table => new
                {
                    new_id = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    title = table.Column<string>(type: "text", nullable: true),
                    content = table.Column<string>(type: "text", nullable: true),
                    post_date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("news_pkey", x => x.new_id);
                });

            migrationBuilder.CreateTable(
                name: "roles",
                columns: table => new
                {
                    role_id = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    role_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("roles_pkey", x => x.role_id);
                });

            migrationBuilder.CreateTable(
                name: "rooms",
                columns: table => new
                {
                    room_id = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: false),
                    cinema_id = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    room_name = table.Column<int>(type: "integer", nullable: true),
                    total_seats = table.Column<int>(type: "integer", nullable: true),
                    room_status = table.Column<BitArray>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("rooms_pkey", x => x.room_id);
                    table.ForeignKey(
                        name: "rooms_cinema_id_fkey",
                        column: x => x.cinema_id,
                        principalTable: "cinemas",
                        principalColumn: "cinema_id");
                });

            migrationBuilder.CreateTable(
                name: "comboitems",
                columns: table => new
                {
                    combo_id = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    concession_id = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("comboitems_pkey", x => new { x.combo_id, x.concession_id });
                    table.ForeignKey(
                        name: "comboitems_combo_id_fkey",
                        column: x => x.combo_id,
                        principalTable: "combo",
                        principalColumn: "combo_id");
                    table.ForeignKey(
                        name: "comboitems_concession_id_fkey",
                        column: x => x.concession_id,
                        principalTable: "concessions",
                        principalColumn: "concession_id");
                });

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    user_name = table.Column<string>(type: "character varying(70)", maxLength: 70, nullable: true),
                    email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    phone = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    password = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    gender = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    birthday = table.Column<DateOnly>(type: "date", nullable: true),
                    role_id = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    creation_date = table.Column<DateOnly>(type: "date", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("users_pkey", x => x.user_id);
                    table.ForeignKey(
                        name: "users_role_id_fkey",
                        column: x => x.role_id,
                        principalTable: "roles",
                        principalColumn: "role_id");
                });

            migrationBuilder.CreateTable(
                name: "seats",
                columns: table => new
                {
                    seat_id = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    room_id = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    seat_name = table.Column<string>(type: "character varying(3)", maxLength: 3, nullable: true),
                    seat_type = table.Column<BitArray>(type: "bit(1)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("seats_pkey", x => x.seat_id);
                    table.ForeignKey(
                        name: "seats_room_id_fkey",
                        column: x => x.room_id,
                        principalTable: "rooms",
                        principalColumn: "room_id");
                });

            migrationBuilder.CreateTable(
                name: "showtime",
                columns: table => new
                {
                    showtime_id = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    movie_id = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: true),
                    room_id = table.Column<string>(type: "character varying(8)", maxLength: 8, nullable: true),
                    start_time = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    end_time = table.Column<TimeOnly>(type: "time without time zone", nullable: true),
                    show_date = table.Column<DateOnly>(type: "date", nullable: true),
                    format = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("showtime_pkey", x => x.showtime_id);
                    table.ForeignKey(
                        name: "showtime_movie_id_fkey",
                        column: x => x.movie_id,
                        principalTable: "movies",
                        principalColumn: "movie_id");
                    table.ForeignKey(
                        name: "showtime_room_id_fkey",
                        column: x => x.room_id,
                        principalTable: "rooms",
                        principalColumn: "room_id");
                });

            migrationBuilder.CreateTable(
                name: "invoice",
                columns: table => new
                {
                    invoice_id = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    showtime_id = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    user_id = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: true),
                    invoice_date = table.Column<DateOnly>(type: "date", nullable: true),
                    payment_method = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    discount = table.Column<int>(type: "integer", nullable: true),
                    vat = table.Column<int>(type: "integer", nullable: true),
                    invoice_status = table.Column<BitArray>(type: "bit(1)", nullable: true),
                    total_amount = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("invoice_pkey", x => x.invoice_id);
                    table.ForeignKey(
                        name: "invoice_showtime_id_fkey",
                        column: x => x.showtime_id,
                        principalTable: "showtime",
                        principalColumn: "showtime_id");
                    table.ForeignKey(
                        name: "invoice_user_id_fkey",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "user_id");
                });

            migrationBuilder.CreateTable(
                name: "ticket",
                columns: table => new
                {
                    ticket_id = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    showtime_id = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    seat_id = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    is_booked = table.Column<bool>(type: "boolean", nullable: true),
                    ticket_price = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("ticket_pkey", x => x.ticket_id);
                    table.ForeignKey(
                        name: "ticket_seat_id_fkey",
                        column: x => x.seat_id,
                        principalTable: "seats",
                        principalColumn: "seat_id");
                    table.ForeignKey(
                        name: "ticket_showtime_id_fkey",
                        column: x => x.showtime_id,
                        principalTable: "showtime",
                        principalColumn: "showtime_id");
                });

            migrationBuilder.CreateTable(
                name: "invoicedetails",
                columns: table => new
                {
                    invoicedetail_id = table.Column<string>(type: "character varying(12)", maxLength: 12, nullable: false),
                    invoice_id = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    item_type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    item_id = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true),
                    quantity = table.Column<int>(type: "integer", nullable: true),
                    unit_price = table.Column<int>(type: "integer", nullable: true),
                    sub_total = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("invoicedetails_pkey", x => x.invoicedetail_id);
                    table.ForeignKey(
                        name: "invoicedetails_invoice_id_fkey",
                        column: x => x.invoice_id,
                        principalTable: "invoice",
                        principalColumn: "invoice_id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_comboitems_concession_id",
                table: "comboitems",
                column: "concession_id");

            migrationBuilder.CreateIndex(
                name: "IX_invoice_showtime_id",
                table: "invoice",
                column: "showtime_id");

            migrationBuilder.CreateIndex(
                name: "IX_invoice_user_id",
                table: "invoice",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_invoicedetails_invoice_id",
                table: "invoicedetails",
                column: "invoice_id");

            migrationBuilder.CreateIndex(
                name: "IX_rooms_cinema_id",
                table: "rooms",
                column: "cinema_id");

            migrationBuilder.CreateIndex(
                name: "IX_seats_room_id",
                table: "seats",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_showtime_movie_id",
                table: "showtime",
                column: "movie_id");

            migrationBuilder.CreateIndex(
                name: "IX_showtime_room_id",
                table: "showtime",
                column: "room_id");

            migrationBuilder.CreateIndex(
                name: "IX_ticket_seat_id",
                table: "ticket",
                column: "seat_id");

            migrationBuilder.CreateIndex(
                name: "IX_ticket_showtime_id",
                table: "ticket",
                column: "showtime_id");

            migrationBuilder.CreateIndex(
                name: "IX_users_role_id",
                table: "users",
                column: "role_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "comboitems");

            migrationBuilder.DropTable(
                name: "invoicedetails");

            migrationBuilder.DropTable(
                name: "news");

            migrationBuilder.DropTable(
                name: "ticket");

            migrationBuilder.DropTable(
                name: "combo");

            migrationBuilder.DropTable(
                name: "concessions");

            migrationBuilder.DropTable(
                name: "invoice");

            migrationBuilder.DropTable(
                name: "seats");

            migrationBuilder.DropTable(
                name: "showtime");

            migrationBuilder.DropTable(
                name: "users");

            migrationBuilder.DropTable(
                name: "movies");

            migrationBuilder.DropTable(
                name: "rooms");

            migrationBuilder.DropTable(
                name: "roles");

            migrationBuilder.DropTable(
                name: "cinemas");
        }
    }
}
