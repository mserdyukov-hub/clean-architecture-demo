using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddTopicName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Topic",
                schema: "integration",
                table: "outbox_messages",
                newName: "topc");

            migrationBuilder.AlterColumn<string>(
                name: "topc",
                schema: "integration",
                table: "outbox_messages",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "topc",
                schema: "integration",
                table: "outbox_messages",
                newName: "Topic");

            migrationBuilder.AlterColumn<string>(
                name: "Topic",
                schema: "integration",
                table: "outbox_messages",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);
        }
    }
}
