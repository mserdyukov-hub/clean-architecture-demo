using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddShemaInboxMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_InboxMessage",
                table: "InboxMessage");

            migrationBuilder.RenameTable(
                name: "InboxMessage",
                newName: "inbox_messages",
                newSchema: "integration");

            migrationBuilder.RenameIndex(
                name: "IX_InboxMessage_topic_partition_offset",
                schema: "integration",
                table: "inbox_messages",
                newName: "IX_inbox_messages_topic_partition_offset");

            migrationBuilder.AddPrimaryKey(
                name: "PK_inbox_messages",
                schema: "integration",
                table: "inbox_messages",
                column: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_inbox_messages",
                schema: "integration",
                table: "inbox_messages");

            migrationBuilder.RenameTable(
                name: "inbox_messages",
                schema: "integration",
                newName: "InboxMessage");

            migrationBuilder.RenameIndex(
                name: "IX_inbox_messages_topic_partition_offset",
                table: "InboxMessage",
                newName: "IX_InboxMessage_topic_partition_offset");

            migrationBuilder.AddPrimaryKey(
                name: "PK_InboxMessage",
                table: "InboxMessage",
                column: "Id");
        }
    }
}
