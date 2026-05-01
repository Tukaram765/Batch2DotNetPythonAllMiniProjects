using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace EMS.API.Migrations
{
    /// <inheritdoc />
    public partial class start : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Department = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Designation = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Salary = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    JoinDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CreatedAt", "Department", "Designation", "Email", "FirstName", "JoinDate", "LastName", "Phone", "Salary", "Status", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(2025, 12, 18, 19, 19, 13, 423, DateTimeKind.Utc).AddTicks(4039), "Engineering", "Software Engineer", "amiat.sharma31@example.com", "Amit", new DateTime(2025, 12, 18, 19, 19, 13, 423, DateTimeKind.Utc).AddTicks(4028), "Sharma", "9876543210", 60000m, "Active", new DateTime(2025, 12, 18, 19, 19, 13, 423, DateTimeKind.Utc).AddTicks(4040) },
                    { 2, new DateTime(2025, 9, 29, 19, 19, 13, 423, DateTimeKind.Utc).AddTicks(4045), "HR", "HR Manager", "pribya.mehta32@example.com", "Priya", new DateTime(2025, 9, 29, 19, 19, 13, 423, DateTimeKind.Utc).AddTicks(4044), "Mehta", "9123456780", 55000m, "Active", new DateTime(2025, 9, 29, 19, 19, 13, 423, DateTimeKind.Utc).AddTicks(4046) },
                    { 3, new DateTime(2026, 1, 17, 19, 19, 13, 423, DateTimeKind.Utc).AddTicks(4058), "Finance", "Accountant", "rcahul.verma33@example.com", "Rahul", new DateTime(2026, 1, 17, 19, 19, 13, 423, DateTimeKind.Utc).AddTicks(4050), "Verma", "9988776655", 50000m, "Inactive", new DateTime(2026, 1, 17, 19, 19, 13, 423, DateTimeKind.Utc).AddTicks(4059) },
                    { 4, new DateTime(2026, 2, 16, 19, 19, 13, 423, DateTimeKind.Utc).AddTicks(4063), "Marketing", "Marketing Executive", "nehda.kapoor34@example.com", "Neha", new DateTime(2026, 2, 16, 19, 19, 13, 423, DateTimeKind.Utc).AddTicks(4062), "Kapoor", "9090909090", 45000m, "Active", new DateTime(2026, 2, 16, 19, 19, 13, 423, DateTimeKind.Utc).AddTicks(4064) },
                    { 5, new DateTime(2025, 6, 21, 19, 19, 13, 423, DateTimeKind.Utc).AddTicks(4164), "Operations", "Operations Manager", "sueresh.patil35@example.com", "Suresh", new DateTime(2025, 6, 21, 19, 19, 13, 423, DateTimeKind.Utc).AddTicks(4163), "Patil", "8888888888", 70000m, "Active", new DateTime(2025, 6, 21, 19, 19, 13, 423, DateTimeKind.Utc).AddTicks(4164) },
                    { 6, new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Engineering", "Software Engineer", "amfit.sharma36@example.com", "Amit", new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Sharma", "9876543210", 60000m, "Active", new DateTime(2025, 1, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 7, new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Marketing", "Marketing Executive", "prgiya.verma37@example.com", "Priya", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "Verma", "9876543211", 45000m, "Active", new DateTime(2025, 2, 15, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 8, new DateTime(2024, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "HR", "HR Manager", "rahhul.kumar38@example.com", "Rahul", new DateTime(2024, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), "Kuhmar", "9876543212", 70000m, "Active", new DateTime(2024, 12, 5, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 9, new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Finance", "Accountant", "snehia.patil39@example.com", "Sneha", new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "Patil", "9876543213", 52000m, "Inactive", new DateTime(2024, 11, 20, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 10, new DateTime(2024, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Operations", "Operations Manager", "vikrjam.singh40@example.com", "Vikram", new DateTime(2024, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "Singh", "9876543214", 80000m, "Active", new DateTime(2024, 10, 12, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 11, new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Engineering", "Backend Developer", "nehka.gupta41@example.com", "Neha", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Gupta", "9876543215", 65000m, "Active", new DateTime(2025, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 12, new DateTime(2025, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Engineering", "Frontend Developer", "arljun.mehta42@example.com", "Arjun", new DateTime(2025, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mehta", "9876543216", 62000m, "Inactive", new DateTime(2025, 1, 25, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 13, new DateTime(2024, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Marketing", "SEO Analyst", "kavmya.nair43@example.com", "Kavya", new DateTime(2024, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified), "Nair", "9876543217", 48000m, "Active", new DateTime(2024, 9, 18, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 14, new DateTime(2024, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "HR", "Recruiter", "ronhit.yadav44@example.com", "Rohit", new DateTime(2024, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "Yadav", "9876543218", 43000m, "Active", new DateTime(2024, 8, 30, 0, 0, 0, 0, DateTimeKind.Unspecified) },
                    { 15, new DateTime(2024, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Finance", "Financial Analyst", "anjoali.deshmukh45@example.com", "Anjali", new DateTime(2024, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "Deshmukh", "9876543219", 55000m, "Inactive", new DateTime(2024, 7, 10, 0, 0, 0, 0, DateTimeKind.Unspecified) }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "CreatedAt", "PasswordHash", "Role", "Username" },
                values: new object[,]
                {
                    { 20, new DateTime(2026, 4, 17, 19, 19, 13, 233, DateTimeKind.Utc).AddTicks(5460), "$2a$11$xwlNs416ilRBB6ym7YjwQuEAzIRYXQR02pvJmPm9V8aG4BhrKJBKO", "Admin", "admin" },
                    { 21, new DateTime(2026, 4, 17, 19, 19, 13, 423, DateTimeKind.Utc).AddTicks(2932), "$2a$11$cRcBmRPTP8MzKY2NI6.jP.9vyYEMilDgHuqBgvOGtxOCqooc3Xhpu", "Viewer", "viewer" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Employees_Email",
                table: "Employees",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
