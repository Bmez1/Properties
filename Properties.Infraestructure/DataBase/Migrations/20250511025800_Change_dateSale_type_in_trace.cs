using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Properties.Infraestructure.DataBase.Migrations
{
    /// <inheritdoc />
    public partial class Change_dateSale_type_in_trace : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateSale",
                table: "PropertyTraces",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateOnly),
                oldType: "date");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateOnly>(
                name: "DateSale",
                table: "PropertyTraces",
                type: "date",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");
        }
    }
}
