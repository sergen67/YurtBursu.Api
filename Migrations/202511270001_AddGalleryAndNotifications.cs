using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YurtBursu.Api.Migrations
{
	/// <inheritdoc />
	public partial class AddGalleryAndNotifications : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.CreateTable(
				name: "Gallery",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					ImageUrl = table.Column<string>(type: "nvarchar(2048)", maxLength: 2048, nullable: false),
					CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Gallery", x => x.Id);
				});

			migrationBuilder.CreateTable(
				name: "NotificationTokens",
				columns: table => new
				{
					Id = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					StudentId = table.Column<int>(type: "int", nullable: false),
					Token = table.Column<string>(type: "nvarchar(4096)", maxLength: 4096, nullable: false),
					CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_NotificationTokens", x => x.Id);
					table.ForeignKey(
						name: "FK_NotificationTokens_Students_StudentId",
						column: x => x.StudentId,
						principalTable: "Students",
						principalColumn: "Id",
						onDelete: ReferentialAction.Cascade);
				});

			migrationBuilder.CreateIndex(
				name: "IX_NotificationTokens_Token",
				table: "NotificationTokens",
				column: "Token",
				unique: true);

			migrationBuilder.CreateIndex(
				name: "IX_NotificationTokens_StudentId",
				table: "NotificationTokens",
				column: "StudentId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "Gallery");

			migrationBuilder.DropTable(
				name: "NotificationTokens");
		}
	}
}


