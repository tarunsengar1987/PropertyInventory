using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PropertyInventory.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FirstName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Properties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfRegistration = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CurrentPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Properties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PropertyOwnerships",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContactId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EffectiveFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EffectiveTill = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AcquisitionPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyOwnerships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyOwnerships_Contacts_ContactId",
                        column: x => x.ContactId,
                        principalTable: "Contacts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PropertyOwnerships_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PropertyPriceHistories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Currency = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ChangedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PropertyPriceHistories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PropertyPriceHistories_Properties_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Properties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PropertyOwnerships_ContactId",
                table: "PropertyOwnerships",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyOwnerships_PropertyId",
                table: "PropertyOwnerships",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_PropertyPriceHistories_PropertyId",
                table: "PropertyPriceHistories",
                column: "PropertyId");

            // Seed data (same as original PropertyInventoryDbContext)
            var contactCarmen = new Guid("11111111-1111-1111-1111-111111111111");
            var contactJoshua = new Guid("22222222-2222-2222-2222-222222222222");
            var contactJoe = new Guid("33333333-3333-3333-3333-333333333333");
            var propMaisonette = new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
            var propPenthouse = new Guid("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");

            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "Id", "FirstName", "LastName", "PhoneNumber", "Email" },
                values: new object[] { contactCarmen, "Carmen", "Attard", "+35679000001", "carmen.attard@example.com" });
            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "Id", "FirstName", "LastName", "PhoneNumber", "Email" },
                values: new object[] { contactJoshua, "Joshua", "Mifsud", "+35679000002", "joshua.mifsud@example.com" });
            migrationBuilder.InsertData(
                table: "Contacts",
                columns: new[] { "Id", "FirstName", "LastName", "PhoneNumber", "Email" },
                values: new object[] { contactJoe, "Joe", "Borg", "+35679000003", "joe.borg@example.com" });

            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Id", "Name", "Address", "DateOfRegistration", "CurrentPrice" },
                values: new object[] { propMaisonette, "Maisonette", "12 St. Paul's Street, Valletta, Malta", new DateTime(2023, 7, 25, 0, 0, 0, DateTimeKind.Utc), 130000m });
            migrationBuilder.InsertData(
                table: "Properties",
                columns: new[] { "Id", "Name", "Address", "DateOfRegistration", "CurrentPrice" },
                values: new object[] { propPenthouse, "Penthouse", "3 Tower Road, Sliema, Malta", new DateTime(2023, 5, 6, 0, 0, 0, DateTimeKind.Utc), 430000m });

            migrationBuilder.InsertData(
                table: "PropertyOwnerships",
                columns: new[] { "Id", "PropertyId", "ContactId", "EffectiveFrom", "EffectiveTill", "AcquisitionPrice", "Currency" },
                values: new object[] { new Guid("cc000001-0000-0000-0000-000000000001"), propMaisonette, contactJoshua, new DateTime(2023, 7, 25, 0, 0, 0, DateTimeKind.Utc), new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc), 100000m, "EUR" });
            migrationBuilder.InsertData(
                table: "PropertyOwnerships",
                columns: new[] { "Id", "PropertyId", "ContactId", "EffectiveFrom", "EffectiveTill", "AcquisitionPrice", "Currency" },
                values: new object[] { new Guid("cc000001-0000-0000-0000-000000000002"), propMaisonette, contactCarmen, new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc), null, 120000m, "EUR" });
            migrationBuilder.InsertData(
                table: "PropertyOwnerships",
                columns: new[] { "Id", "PropertyId", "ContactId", "EffectiveFrom", "EffectiveTill", "AcquisitionPrice", "Currency" },
                values: new object[] { new Guid("cc000002-0000-0000-0000-000000000001"), propPenthouse, contactJoe, new DateTime(2023, 5, 6, 0, 0, 0, DateTimeKind.Utc), null, 400000m, "EUR" });

            migrationBuilder.InsertData(
                table: "PropertyPriceHistories",
                columns: new[] { "Id", "PropertyId", "Price", "Currency", "ChangedOn" },
                values: new object[] { new Guid("dd000001-0000-0000-0000-000000000001"), propMaisonette, 110000m, "EUR", new DateTime(2023, 7, 25, 0, 0, 0, DateTimeKind.Utc) });
            migrationBuilder.InsertData(
                table: "PropertyPriceHistories",
                columns: new[] { "Id", "PropertyId", "Price", "Currency", "ChangedOn" },
                values: new object[] { new Guid("dd000001-0000-0000-0000-000000000002"), propMaisonette, 130000m, "EUR", new DateTime(2024, 1, 15, 0, 0, 0, DateTimeKind.Utc) });
            migrationBuilder.InsertData(
                table: "PropertyPriceHistories",
                columns: new[] { "Id", "PropertyId", "Price", "Currency", "ChangedOn" },
                values: new object[] { new Guid("dd000002-0000-0000-0000-000000000001"), propPenthouse, 430000m, "EUR", new DateTime(2023, 5, 6, 0, 0, 0, DateTimeKind.Utc) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PropertyOwnerships");

            migrationBuilder.DropTable(
                name: "PropertyPriceHistories");

            migrationBuilder.DropTable(
                name: "Contacts");

            migrationBuilder.DropTable(
                name: "Properties");          
        }
    }
}
