using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UMS.Api.Migrations
{
    /// <inheritdoc />
    public partial class PlainTextTemporaryPasswords : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "MustChangePassword",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.Sql("""
                UPDATE Users
                SET PasswordHash = 'amrita2026',
                    MustChangePassword = CASE WHEN Role IN ('Faculty', 'Student') THEN 1 ELSE 0 END
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                UPDATE Users
                SET PasswordHash = '$2a$11$SUGWfqpmVZMDq1CflE5wn.REKjlyQQtIghM8H6HES6061SpWh.ObC'
                """);

            migrationBuilder.DropColumn(
                name: "MustChangePassword",
                table: "Users");
        }
    }
}
