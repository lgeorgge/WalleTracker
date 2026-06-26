using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WalletTracker.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class addedthewalletstotheuser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Wallets_Name",
                table: "Wallets",
                column: "Name",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Wallets_Name",
                table: "Wallets");
        }
    }
}
