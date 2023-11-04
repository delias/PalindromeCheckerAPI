using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PalindromeCheckerAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdatedSchema : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "Palindromes",
                newName: "DateRegistered");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DateRegistered",
                table: "Palindromes",
                newName: "CreatedAt");
        }
    }
}
