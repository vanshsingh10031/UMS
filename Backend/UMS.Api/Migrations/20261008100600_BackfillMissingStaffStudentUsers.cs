using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UMS.Api.Migrations
{
    /// <inheritdoc />
    [Migration("20261008100600_BackfillMissingStaffStudentUsers")]
    public partial class BackfillMissingStaffStudentUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                INSERT INTO Users (CreatedAt, UpdatedAt, Username, Email, Role, RoleMasterId, DisplayName, PasswordHash, MustChangePassword)
                SELECT GETUTCDATE(), GETUTCDATE(), s.EmployeeId, s.Email, 'Faculty', 2, s.Name, 'amrita2026', 1
                FROM Staff s
                WHERE s.UserId IS NULL
                  AND NOT EXISTS (SELECT 1 FROM Users u WHERE u.Username = s.EmployeeId OR u.Email = s.Email);

                UPDATE s
                SET UserId = u.Id
                FROM Staff s
                INNER JOIN Users u ON u.Username = s.EmployeeId
                WHERE s.UserId IS NULL;

                INSERT INTO Users (CreatedAt, UpdatedAt, Username, Email, Role, RoleMasterId, DisplayName, PasswordHash, MustChangePassword)
                SELECT GETUTCDATE(), GETUTCDATE(), st.StudentId, st.Email, 'Student', 3, st.Name, 'amrita2026', 1
                FROM Students st
                WHERE st.UserId IS NULL
                  AND NOT EXISTS (SELECT 1 FROM Users u WHERE u.Username = st.StudentId OR u.Email = st.Email);

                UPDATE st
                SET UserId = u.Id
                FROM Students st
                INNER JOIN Users u ON u.Username = st.StudentId
                WHERE st.UserId IS NULL;
                """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
