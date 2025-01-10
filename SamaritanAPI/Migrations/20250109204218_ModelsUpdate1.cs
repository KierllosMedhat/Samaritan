using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SamaritanAPI.Migrations
{
    /// <inheritdoc />
    public partial class ModelsUpdate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "NoOfDonationsRecieved",
                table: "Patients",
                newName: "NoOfDonationsReceived");

            migrationBuilder.RenameColumn(
                name: "LastDonationRecieved",
                table: "Patients",
                newName: "LastDonationReceived");

            migrationBuilder.AddColumn<string>(
                name: "RequestTimeline",
                table: "Requests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Notifications",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "ServantCompanionId",
                table: "Notes",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ServantDiallerId",
                table: "Calls",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Notes_ServantCompanionId",
                table: "Notes",
                column: "ServantCompanionId");

            migrationBuilder.CreateIndex(
                name: "IX_Calls_ServantDiallerId",
                table: "Calls",
                column: "ServantDiallerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Calls_AspNetUsers_ServantDiallerId",
                table: "Calls",
                column: "ServantDiallerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Notes_ServantCompanions_ServantCompanionId",
                table: "Notes",
                column: "ServantCompanionId",
                principalTable: "ServantCompanions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Calls_AspNetUsers_ServantDiallerId",
                table: "Calls");

            migrationBuilder.DropForeignKey(
                name: "FK_Notes_ServantCompanions_ServantCompanionId",
                table: "Notes");

            migrationBuilder.DropForeignKey(
                name: "FK_Notifications_AspNetUsers_UserId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notifications_UserId",
                table: "Notifications");

            migrationBuilder.DropIndex(
                name: "IX_Notes_ServantCompanionId",
                table: "Notes");

            migrationBuilder.DropIndex(
                name: "IX_Calls_ServantDiallerId",
                table: "Calls");

            migrationBuilder.DropColumn(
                name: "RequestTimeline",
                table: "Requests");

            migrationBuilder.DropColumn(
                name: "ServantCompanionId",
                table: "Notes");

            migrationBuilder.RenameColumn(
                name: "NoOfDonationsReceived",
                table: "Patients",
                newName: "NoOfDonationsRecieved");

            migrationBuilder.RenameColumn(
                name: "LastDonationReceived",
                table: "Patients",
                newName: "LastDonationRecieved");

            migrationBuilder.AlterColumn<int>(
                name: "UserId",
                table: "Notifications",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<int>(
                name: "ServantDiallerId",
                table: "Calls",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
