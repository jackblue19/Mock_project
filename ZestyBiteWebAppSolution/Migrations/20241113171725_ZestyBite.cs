using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ZestyBiteWebAppSolution.Migrations
{
    /// <inheritdoc />
    public partial class ZestyBite : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "item",
                columns: table => new
                {
                    Item_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Item_Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Item_Category = table.Column<string>(type: "enum('Main course','Dessert','Drink','Salad','Fruit')", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Item_Status = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Item_Description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Suggested_Price = table.Column<decimal>(type: "decimal(10)", precision: 10, nullable: false),
                    Item_Image = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Is_Served = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Item_ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "payment",
                columns: table => new
                {
                    Payment_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Payment_Method = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Payment_ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "role",
                columns: table => new
                {
                    Role_ID = table.Column<sbyte>(type: "tinyint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Role_Description = table.Column<string>(type: "enum('Manager','Order Taker','Procurement Manager','Server Staff','Customer Service Staff','Food Runner','Customer')", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Role_ID);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "account",
                columns: table => new
                {
                    Account_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    PasswordHash = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    LastName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    FirstName = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Email = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Role_ID = table.Column<sbyte>(type: "tinyint", nullable: false),
                    Gender = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Id = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    UserName = table.Column<string>(type: "varchar(255)", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedUserName = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    NormalizedEmail = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EmailConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    SecurityStamp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    ConcurrencyStamp = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumber = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    PhoneNumberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    TwoFactorEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    LockoutEnd = table.Column<DateTimeOffset>(type: "datetime(6)", nullable: true),
                    LockoutEnabled = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    AccessFailedCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Account_ID);
                    table.ForeignKey(
                        name: "account_ibfk_1",
                        column: x => x.Role_ID,
                        principalTable: "role",
                        principalColumn: "Role_ID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "feedback",
                columns: table => new
                {
                    Fb_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Fb_Content = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Fb_Datetime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Account_ID = table.Column<int>(type: "int", nullable: false),
                    Item_ID = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Fb_ID);
                    table.ForeignKey(
                        name: "FK_feedback_account_Account_ID",
                        column: x => x.Account_ID,
                        principalTable: "account",
                        principalColumn: "Account_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "feedback_ibfk_2",
                        column: x => x.Item_ID,
                        principalTable: "item",
                        principalColumn: "Item_ID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "bill",
                columns: table => new
                {
                    Bill_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Bill_Status = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Payment_ID = table.Column<int>(type: "int", nullable: false),
                    Account_ID = table.Column<int>(type: "int", nullable: false),
                    Table_ID = table.Column<int>(type: "int", nullable: false),
                    Total_Cost = table.Column<decimal>(type: "decimal(10)", precision: 10, nullable: false),
                    Bill_Datetime = table.Column<DateTime>(type: "datetime", nullable: false),
                    Bill_Type = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Bill_ID);
                    table.ForeignKey(
                        name: "FK_bill_account_Account_ID",
                        column: x => x.Account_ID,
                        principalTable: "account",
                        principalColumn: "Account_ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "bill_ibfk_1",
                        column: x => x.Payment_ID,
                        principalTable: "payment",
                        principalColumn: "Payment_ID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "profit",
                columns: table => new
                {
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    Supply_ID = table.Column<int>(type: "int", nullable: false),
                    Bill_ID = table.Column<int>(type: "int", nullable: false),
                    Profit_Ammount = table.Column<decimal>(type: "decimal(10)", precision: 10, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Date);
                    table.ForeignKey(
                        name: "profit_ibfk_2",
                        column: x => x.Bill_ID,
                        principalTable: "bill",
                        principalColumn: "Bill_ID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "reservation",
                columns: table => new
                {
                    Reservation_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Bill_ID = table.Column<int>(type: "int", nullable: false),
                    Table_ID = table.Column<int>(type: "int", nullable: false),
                    Reservation_Start = table.Column<DateTime>(type: "datetime", nullable: false),
                    Reservation_End = table.Column<DateTime>(type: "datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Reservation_ID);
                    table.ForeignKey(
                        name: "reservation_ibfk_2",
                        column: x => x.Bill_ID,
                        principalTable: "bill",
                        principalColumn: "Bill_ID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "table",
                columns: table => new
                {
                    Table_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Table_Capacity = table.Column<int>(type: "int", nullable: false),
                    Table_Maintenance = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Reservation_ID = table.Column<int>(type: "int", nullable: false),
                    Item_ID = table.Column<int>(type: "int", nullable: false),
                    Table_Type = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Table_Status = table.Column<string>(type: "enum('Served','Empty','Waiting','Deposit')", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Table_Note = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Table_ID);
                    table.ForeignKey(
                        name: "table_ibfk_1",
                        column: x => x.Item_ID,
                        principalTable: "item",
                        principalColumn: "Item_ID");
                    table.ForeignKey(
                        name: "table_ibfk_2",
                        column: x => x.Reservation_ID,
                        principalTable: "reservation",
                        principalColumn: "Reservation_ID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "supply",
                columns: table => new
                {
                    Supply_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Product_Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Supply_Quantity = table.Column<int>(type: "int", nullable: false),
                    Supply_Price = table.Column<decimal>(type: "decimal(10)", precision: 10, nullable: false),
                    Supply_Status = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    Date_Import = table.Column<DateTime>(type: "datetime", nullable: false),
                    Date_Expiration = table.Column<DateTime>(type: "datetime", nullable: false),
                    Table_ID = table.Column<int>(type: "int", nullable: false),
                    Item_ID = table.Column<int>(type: "int", nullable: false),
                    Vendor_Name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Vendor_Phone = table.Column<string>(type: "varchar(255)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Vendor_Address = table.Column<int>(type: "int", nullable: false),
                    Supply_Category = table.Column<string>(type: "enum('Food','Drink','Facility')", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.Supply_ID);
                    table.ForeignKey(
                        name: "supply_ibfk_1",
                        column: x => x.Table_ID,
                        principalTable: "table",
                        principalColumn: "Table_ID");
                    table.ForeignKey(
                        name: "supply_ibfk_2",
                        column: x => x.Item_ID,
                        principalTable: "item",
                        principalColumn: "Item_ID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "table_details",
                columns: table => new
                {
                    Table_ID = table.Column<int>(type: "int", nullable: false),
                    Item_ID = table.Column<int>(type: "int", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => new { x.Table_ID, x.Item_ID });
                    table.ForeignKey(
                        name: "table_details_ibfk_1",
                        column: x => x.Table_ID,
                        principalTable: "table",
                        principalColumn: "Table_ID");
                    table.ForeignKey(
                        name: "table_details_ibfk_2",
                        column: x => x.Item_ID,
                        principalTable: "item",
                        principalColumn: "Item_ID");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "Role_ID",
                table: "account",
                column: "Role_ID");

            migrationBuilder.CreateIndex(
                name: "Username",
                table: "account",
                column: "UserName",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Account_ID",
                table: "bill",
                column: "Account_ID");

            migrationBuilder.CreateIndex(
                name: "Payment_ID",
                table: "bill",
                column: "Payment_ID");

            migrationBuilder.CreateIndex(
                name: "Table_ID",
                table: "bill",
                column: "Table_ID");

            migrationBuilder.CreateIndex(
                name: "Account_ID1",
                table: "feedback",
                column: "Account_ID");

            migrationBuilder.CreateIndex(
                name: "Item_ID",
                table: "feedback",
                column: "Item_ID");

            migrationBuilder.CreateIndex(
                name: "Bill_ID",
                table: "profit",
                column: "Bill_ID");

            migrationBuilder.CreateIndex(
                name: "Supply_ID",
                table: "profit",
                column: "Supply_ID");

            migrationBuilder.CreateIndex(
                name: "Bill_ID1",
                table: "reservation",
                column: "Bill_ID");

            migrationBuilder.CreateIndex(
                name: "Table_ID1",
                table: "reservation",
                column: "Table_ID");

            migrationBuilder.CreateIndex(
                name: "Item_ID1",
                table: "supply",
                column: "Item_ID");

            migrationBuilder.CreateIndex(
                name: "Table_ID2",
                table: "supply",
                column: "Table_ID");

            migrationBuilder.CreateIndex(
                name: "Vendor_Phone",
                table: "supply",
                column: "Vendor_Phone",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Item_ID2",
                table: "table",
                column: "Item_ID");

            migrationBuilder.CreateIndex(
                name: "Reservation_ID",
                table: "table",
                column: "Reservation_ID");

            migrationBuilder.CreateIndex(
                name: "Item_ID3",
                table: "table_details",
                column: "Item_ID");

            migrationBuilder.AddForeignKey(
                name: "bill_ibfk_3",
                table: "bill",
                column: "Table_ID",
                principalTable: "table",
                principalColumn: "Table_ID");

            migrationBuilder.AddForeignKey(
                name: "profit_ibfk_1",
                table: "profit",
                column: "Supply_ID",
                principalTable: "supply",
                principalColumn: "Supply_ID");

            migrationBuilder.AddForeignKey(
                name: "reservation_ibfk_1",
                table: "reservation",
                column: "Table_ID",
                principalTable: "table",
                principalColumn: "Table_ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "account_ibfk_1",
                table: "account");

            migrationBuilder.DropForeignKey(
                name: "FK_bill_account_Account_ID",
                table: "bill");

            migrationBuilder.DropForeignKey(
                name: "bill_ibfk_1",
                table: "bill");

            migrationBuilder.DropForeignKey(
                name: "bill_ibfk_3",
                table: "bill");

            migrationBuilder.DropForeignKey(
                name: "reservation_ibfk_1",
                table: "reservation");

            migrationBuilder.DropTable(
                name: "feedback");

            migrationBuilder.DropTable(
                name: "profit");

            migrationBuilder.DropTable(
                name: "table_details");

            migrationBuilder.DropTable(
                name: "supply");

            migrationBuilder.DropTable(
                name: "role");

            migrationBuilder.DropTable(
                name: "account");

            migrationBuilder.DropTable(
                name: "payment");

            migrationBuilder.DropTable(
                name: "table");

            migrationBuilder.DropTable(
                name: "item");

            migrationBuilder.DropTable(
                name: "reservation");

            migrationBuilder.DropTable(
                name: "bill");
        }
    }
}
