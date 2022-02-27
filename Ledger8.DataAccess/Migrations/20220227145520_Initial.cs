using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ledger8.DataAccess.Migrations;

public partial class Initial : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "AccountNumbers",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                AccountId = table.Column<int>(type: "int", nullable: false),
                StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                StopDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                Salt = table.Column<byte[]>(type: "varbinary(64)", nullable: false),
                Number = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_AccountNumbers", x => x.Id));

        migrationBuilder.CreateTable(
            name: "AccountTypes",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_AccountTypes", x => x.Id));

        migrationBuilder.CreateTable(
            name: "Companies",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Address1 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Address2 = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                City = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                State = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                PostalCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                URL = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                IsPayee = table.Column<bool>(type: "bit", nullable: false),
                Comments = table.Column<string>(type: "nvarchar(max)", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Companies", x => x.Id));

        migrationBuilder.CreateTable(
            name: "Pools",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                Date = table.Column<DateTime>(type: "date", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Amount = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                Balance = table.Column<decimal>(type: "decimal(12,2)", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Pools", x => x.Id));

        migrationBuilder.CreateTable(
            name: "Settings",
            columns: table => new
            {
                Lock = table.Column<int>(type: "int", nullable: false),
                SystemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                Theme = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                BackupDirectory = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Salt = table.Column<byte[]>(type: "varbinary(64)", nullable: true),
                Hash = table.Column<byte[]>(type: "varbinary(64)", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Settings", x => new { x.Lock, x.SystemId });
                table.CheckConstraint("CK_Settings_SettingsCheck", "[Lock] = 1");
            });

        migrationBuilder.CreateTable(
            name: "Transactions",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                AccountId = table.Column<int>(type: "int", nullable: false),
                Date = table.Column<DateTime>(type: "date", nullable: false),
                Balance = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                Payment = table.Column<decimal>(type: "decimal(12,2)", nullable: false),
                Reference = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Transactions", x => x.Id));

        migrationBuilder.CreateTable(
            name: "Accounts",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                CompanyId = table.Column<int>(type: "int", nullable: false),
                AccountTypeId = table.Column<int>(type: "int", nullable: false),
                DueDateType = table.Column<int>(type: "int", nullable: false),
                Month = table.Column<int>(type: "int", nullable: false),
                Day = table.Column<int>(type: "int", nullable: false),
                IsPayable = table.Column<bool>(type: "bit", nullable: false),
                IsClosed = table.Column<bool>(type: "bit", nullable: false),
                ClosedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                IsAutoPaid = table.Column<bool>(type: "bit", nullable: false),
                Comments = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Tag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                AccountNumberId = table.Column<int>(type: "int", nullable: true)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Accounts", x => x.Id);
                table.ForeignKey(
                    name: "FK_Accounts_AccountNumbers_AccountNumberId",
                    column: x => x.AccountNumberId,
                    principalTable: "AccountNumbers",
                    principalColumn: "Id");
                table.ForeignKey(
                    name: "FK_Accounts_AccountTypes_AccountTypeId",
                    column: x => x.AccountTypeId,
                    principalTable: "AccountTypes",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Allotments",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                PoolId = table.Column<int>(type: "int", nullable: false),
                CompanyId = table.Column<int>(type: "int", nullable: false),
                Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Amount = table.Column<decimal>(type: "decimal(12,2)", nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Allotments", x => x.Id);
                table.ForeignKey(
                    name: "FK_Allotments_Companies_CompanyId",
                    column: x => x.CompanyId,
                    principalTable: "Companies",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateTable(
            name: "Identities",
            columns: table => new
            {
                Id = table.Column<int>(type: "int", nullable: false)
                    .Annotation("SqlServer:Identity", "1, 1"),
                CompanyId = table.Column<int>(type: "int", nullable: false),
                URL = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                UserSalt = table.Column<byte[]>(type: "varbinary(64)", nullable: false),
                UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                PasswordSalt = table.Column<byte[]>(type: "varbinary(64)", nullable: false),
                Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                Tag = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
            },
            constraints: table =>
            {
                table.PrimaryKey("PK_Identities", x => x.Id);
                table.ForeignKey(
                    name: "FK_Identities_Companies_CompanyId",
                    column: x => x.CompanyId,
                    principalTable: "Companies",
                    principalColumn: "Id",
                    onDelete: ReferentialAction.Cascade);
            });

        migrationBuilder.CreateIndex(
            name: "IX_AccountNumbers_AccountId",
            table: "AccountNumbers",
            column: "AccountId")
            .Annotation("SqlServer:Clustered", false);

        migrationBuilder.CreateIndex(
            name: "IX_AccountNumbers_StartDate",
            table: "AccountNumbers",
            column: "StartDate")
            .Annotation("SqlServer:Clustered", false);

        migrationBuilder.CreateIndex(
            name: "IX_AccountNumbers_StopDate",
            table: "AccountNumbers",
            column: "StopDate")
            .Annotation("SqlServer:Clustered", false);

        migrationBuilder.CreateIndex(
            name: "IX_Accounts_AccountNumberId",
            table: "Accounts",
            column: "AccountNumberId");

        migrationBuilder.CreateIndex(
            name: "IX_Accounts_AccountTypeId",
            table: "Accounts",
            column: "AccountTypeId")
            .Annotation("SqlServer:Clustered", false);

        migrationBuilder.CreateIndex(
            name: "IX_Accounts_CompanyId",
            table: "Accounts",
            column: "CompanyId")
            .Annotation("SqlServer:Clustered", false);

        migrationBuilder.CreateIndex(
            name: "IX_Accounts_Tag",
            table: "Accounts",
            column: "Tag")
            .Annotation("SqlServer:Clustered", false);

        migrationBuilder.CreateIndex(
            name: "IX_AccountTypes_Description",
            table: "AccountTypes",
            column: "Description",
            unique: true)
            .Annotation("SqlServer:Clustered", false);

        migrationBuilder.CreateIndex(
            name: "IX_Allotments_CompanyId",
            table: "Allotments",
            column: "CompanyId")
            .Annotation("SqlServer:Clustered", false);

        migrationBuilder.CreateIndex(
            name: "IX_Allotments_PoolId",
            table: "Allotments",
            column: "PoolId")
            .Annotation("SqlServer:Clustered", false);

        migrationBuilder.CreateIndex(
            name: "IX_Companies_Name",
            table: "Companies",
            column: "Name",
            unique: true)
            .Annotation("SqlServer:Clustered", false);

        migrationBuilder.CreateIndex(
            name: "IX_Identities_CompanyId",
            table: "Identities",
            column: "CompanyId")
            .Annotation("SqlServer:Clustered", false);

        migrationBuilder.CreateIndex(
            name: "IX_Identities_Tag",
            table: "Identities",
            column: "Tag")
            .Annotation("SqlServer:Clustered", false);

        migrationBuilder.CreateIndex(
            name: "IX_Pools_Name",
            table: "Pools",
            column: "Name",
            unique: true)
            .Annotation("SqlServer:Clustered", false);

        migrationBuilder.CreateIndex(
            name: "IX_Settings_Lock",
            table: "Settings",
            column: "Lock",
            unique: true)
            .Annotation("SqlServer:Clustered", false);

        migrationBuilder.CreateIndex(
            name: "IX_Settings_SystemId",
            table: "Settings",
            column: "SystemId",
            unique: true)
            .Annotation("SqlServer:Clustered", false);

        migrationBuilder.CreateIndex(
            name: "IX_Transactions_AccountId",
            table: "Transactions",
            column: "AccountId")
            .Annotation("SqlServer:Clustered", false);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(
            name: "Accounts");

        migrationBuilder.DropTable(
            name: "Allotments");

        migrationBuilder.DropTable(
            name: "Identities");

        migrationBuilder.DropTable(
            name: "Pools");

        migrationBuilder.DropTable(
            name: "Settings");

        migrationBuilder.DropTable(
            name: "Transactions");

        migrationBuilder.DropTable(
            name: "AccountNumbers");

        migrationBuilder.DropTable(
            name: "AccountTypes");

        migrationBuilder.DropTable(
            name: "Companies");
    }
}
